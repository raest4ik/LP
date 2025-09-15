#include <windows.h>
#include <iostream>
#include <string>
#include <vector>
#include <cstdlib>
#include <ctime>
#include <cfloat>

const int M = 10;
const int N = 20;

float mtx[M][N];
float col_sums[N];
int col_numbers[N];

DWORD WINAPI col_sum(LPVOID param) {
    int* pcol_num = (int*)param;
    int col_num = *pcol_num;
    col_sums[col_num] = 0;
    for (int i = 0; i < M; i++)
        col_sums[col_num] += mtx[i][col_num];
    return 0;
}

DWORD WINAPI row_fill(LPVOID param) {
    int row = *(int*)param;
    for (int j = 0; j < N; j++) {
        mtx[row][j] = static_cast<float>(rand()) / RAND_MAX * 100.0f;
    }
    return 0;
}

float det3x3(float mat[3][3]) {
    return mat[0][0] * (mat[1][1] * mat[2][2] - mat[1][2] * mat[2][1]) -
           mat[0][1] * (mat[1][0] * mat[2][2] - mat[1][2] * mat[2][0]) +
           mat[0][2] * (mat[1][0] * mat[2][1] - mat[1][1] * mat[2][0]);
}

float det4x4(float mat[4][4]) {
    float det = 0;
    for (int c = 0; c < 4; c++) {
        float submat[3][3];
        for (int i = 1; i < 4; i++) {
            int subcol = 0;
            for (int j = 0; j < 4; j++) {
                if (j == c) continue;
                submat[i-1][subcol++] = mat[i][j];
            }
        }
        float subdet = det3x3(submat);
        det += (c % 2 == 0 ? 1 : -1) * mat[0][c] * subdet;
    }
    return det;
}

std::string encrypt(const std::string& text, const std::string& key) {
    int n = (int)text.length();
    int k = (int)key.length();
    std::string encrypted;
    for (int start = 0; start < n; start += k) {
        std::string block = text.substr(start, k);
        std::reverse(block.begin(), block.end());
        for (size_t i = 0; i < block.size(); i++) {
            block[i] = block[i] ^ key[i];
        }
        encrypted += block;
    }
    return encrypted;
}

std::string decrypt(const std::string& encrypted, const std::string& key) {
    int n = (int)encrypted.length();
    int k = (int)key.length();
    std::string decrypted;
    for (int start = 0; start < n; start += k) {
        std::string block = encrypted.substr(start, k);
        for (size_t i = 0; i < block.size(); i++) {
            block[i] = block[i] ^ key[i];
        }
        std::reverse(block.begin(), block.end());
        decrypted += block;
    }
    return decrypted;
}

struct TreeNode {
    int val;
    TreeNode* left;
    TreeNode* right;
    TreeNode(int v) : val(v), left(nullptr), right(nullptr) {}
};

int sumTree(TreeNode* root) {
    if (!root) return 0;
    return root->val + sumTree(root->left) + sumTree(root->right);
}

void fillMatrixParallel() {
    HANDLE hThreads[M];
    int row_nums[M];
    DWORD threadIDs[M];
    for (int i = 0; i < M; i++) {
        row_nums[i] = i;
        hThreads[i] = CreateThread(NULL, 0, row_fill, &row_nums[i], 0, &threadIDs[i]);
    }
    WaitForMultipleObjects(M, hThreads, TRUE, INFINITE);
    for (int i = 0; i < M; i++) {
        CloseHandle(hThreads[i]);
    }
    std::cout << "Матрица заполнена параллельно.\n";
}

void findRowMaxAvg() {
    int row_max = 0;
    float max_avg = -FLT_MAX;
    for (int i = 0; i < M; i++) {
        float sum = 0;
        for (int j = 0; j < N; j++) {
            sum += mtx[i][j];
        }
        float avg = sum / N;
        if (avg > max_avg) {
            max_avg = avg;
            row_max = i;
        }
    }
    std::cout << "Номер строки с максимальным средним значением: " << row_max << "\n";
}

void bubbleSortColumns() {
    for (int col = 0; col < N; col++) {
        for (int i = 0; i < M - 1; i++) {
            for (int j = 0; j < M - i - 1; j++) {
                if (mtx[j][col] > mtx[j+1][col]) {
                    std::swap(mtx[j][col], mtx[j+1][col]);
                }
            }
        }
    }
    std::cout << "Каждый столбец отсортирован методом обмена.\n";
}

void findMinColSum() {
    for (int i = 0; i < N; i++) {
        col_sums[i] = 0;
        col_numbers[i] = i;
    }

    HANDLE hThreads[N];
    DWORD threadIDs[N];
    for (int i = 0; i < N; i++) {
        hThreads[i] = CreateThread(NULL, 0, col_sum, &(col_numbers[i]), 0, &threadIDs[i]);
    }
    WaitForMultipleObjects(N, hThreads, TRUE, INFINITE);
    for (int i = 0; i < N; i++) {
        CloseHandle(hThreads[i]);
    }

    int num_min = 0;
    float min_sum = col_sums[0];
    for (int i = 1; i < N; i++) {
        if (col_sums[i] < min_sum) {
            min_sum = col_sums[i];
            num_min = i;
        }
    }
    std::cout << "Столбец с минимальной суммой: " << num_min << "\n";
}

void determinant4x4Menu() {
    float mat[4][4];
    std::cout << "Введите элементы квадр. матрицы 4x4:\n";
    for(int i = 0; i < 4; i++)
        for(int j = 0; j < 4; j++) {
            std::cout << "m[" << i << "][" << j << "] = ";
            std::cin >> mat[i][j];
        }
    float det = det4x4(mat);
    std::cout << "Определитель матрицы: " << det << "\n";
}

void encryptMenu() {
    std::cin.ignore(); // чистим буфер
    std::string text, key;
    std::cout << "Введите текст для шифрования:\n";
    std::getline(std::cin, text);
    std::cout << "Введите ключ:\n";
    std::getline(std::cin, key);
    std::string encrypted = encrypt(text, key);
    std::cout << "Зашифрованный текст (возможно с непечатаемыми символами):\n";
    for(auto c : encrypted) std::cout << std::hex << ((int)(unsigned char)c) << " ";
    std::cout << "\n";
}

void decryptMenu() {
    std::cin.ignore();
    std::string encrypted_hex, key;
    std::cout << "Введите зашифрованный текст в шестнадцатеричном формате (через пробел):\n";
    std::getline(std::cin, encrypted_hex);
    std::cout << "Введите ключ:\n";
    std::getline(std::cin, key);

    // Разбор hex строки в std::string с байтами
    std::string encrypted_bytes;
    size_t pos = 0;
    while (pos < encrypted_hex.size()) {
        while (pos < encrypted_hex.size() && encrypted_hex[pos] == ' ') pos++;
        if (pos + 2 > encrypted_hex.size()) break;
        std::string byte_str = encrypted_hex.substr(pos, 2);
        char byte = (char)strtol(byte_str.c_str(), nullptr, 16);
        encrypted_bytes.push_back(byte);
        pos += 2;
    }
    std::string decrypted = decrypt(encrypted_bytes, key);
    std::cout << "Расшифрованный текст:\n" << decrypted << "\n";
}

TreeNode* createSampleTree() {
    // Пример дерева: 
    TreeNode* root = new TreeNode(10);
    root->left = new TreeNode(5);
    root->right = new TreeNode(15);
    root->left->left = new TreeNode(2);
    root->left->right = new TreeNode(7);
    root->right->right = new TreeNode(20);
    return root;
}

void sumTreeMenu() {
    TreeNode* root = createSampleTree();
    int total = sumTree(root);
    std::cout << "Сумма элементов бинарного дерева: " << total << "\n";
    // Очистка дерева пропущена для простоты
}

int main() {
    srand((unsigned)time(NULL));
    int choice;
    do {
        std::cout << "\nМеню:\n"
                  << "1) Заполнить матрицу параллельно\n"
                  << "2) Найти строку с максимальным средним значением\n"
                  << "3) Отсортировать каждый столбец по возрастанию\n"
                  << "4) Найти столбец с минимальной суммой (параллельно)\n"
                  << "5) Найти определитель 4x4 матрицы\n"
                  << "6) Зашифровать текст\n"
                  << "7) Расшифровать текст\n"
                  << "8) Найти сумму элементов бинарного дерева\n"
                  << "0) Выход\n"
                  << "Выберите пункт меню: ";
        std::cin >> choice;

        switch (choice) {
            case 1: fillMatrixParallel(); break;
            case 2: findRowMaxAvg(); break;
            case 3: bubbleSortColumns(); break;
            case 4: findMinColSum(); break;
            case 5: determinant4x4Menu(); break;
            case 6: encryptMenu(); break;
            case 7: decryptMenu(); break;
            case 8: sumTreeMenu(); break;
            case 0: std::cout << "Выход из программы.\n"; break;
            default: std::cout << "Неверный выбор.\n"; break;
        }
    } while (choice != 0);

    return 0;
}

#include <windows.h>
#include <iostream>
#include <cstdlib>
#include <ctime>

const int n = 10;  // число строк
const int m = 20;  // число столбцов

float mtx[n][m];      // матрица

// --- Общие вспомогательные данные ---
int row_numbers[n];
int col_numbers[m];
float minor3x3[4][3][3];  // Миноры для 4 слагаемых
float results[4] = {0};   // Результаты для каждого минорного определителя
float matrix[4][4];

DWORD WINAPI sort_column(LPVOID param)
{
    int col_num = *(int*)param;
    for (int i = 0; i < n - 1; i++)
    {
        for (int j = 0; j < n - 1 - i; j++)
        {
            if (mtx[j][col_num] > mtx[j + 1][col_num])
            {
                // Обмен элементов
                float temp = mtx[j][col_num];
                mtx[j][col_num] = mtx[j + 1][col_num];
                mtx[j + 1][col_num] = temp;
            }
        }
    }
    return 0;
}

void fill_matrix_parallel()
{
    // Потоки для заполнения строк
    HANDLE hFillThreads[n];

    // Функция потока для заполнения строки
    auto fill_row = [](LPVOID param) -> DWORD {
        int row_num = *(int*)param;
        for (int j = 0; j < m; j++)
            mtx[row_num][j] = (float)rand();
        return 0;
    };

    for (int i = 0; i < n; i++)
        row_numbers[i] = i;

    for (int i = 0; i < n; i++) {
        hFillThreads[i] = CreateThread(NULL, 0, fill_row, &row_numbers[i], 0, NULL);
        if (hFillThreads[i] == NULL)
            std::cout << "Ошибка создания потока заполнения строки " << i << "\n";
    }
    WaitForMultipleObjects(n, hFillThreads, TRUE, INFINITE);

    for (int i = 0; i < n; i++) 
        CloseHandle(hFillThreads[i]);
}

// Задание 1: найти столбец с минимальной суммой
void task1_find_min_col_sum()
{
    fill_matrix_parallel();

    float col_sums[m] = {0};
    col_numbers[0] = 0;
    for (int i = 1; i < m; i++) col_numbers[i] = i;

    HANDLE hColThreads[m];

    auto col_sum = [](LPVOID param) -> DWORD {
        int col_num = *(int*)param;
        float sum = 0;
        for (int i = 0; i < n; i++)
            sum += mtx[i][col_num];
        float* sums_addr = nullptr;
        sums_addr = &((float*)param)[-col_num];  // Чтобы получить col_sums, переделано ниже для простоты
        col_sums[col_num] = sum; // col_sums - объявлен в main - сделаем глобальным
        return 0;
    };

    // Для упрощения сделаем col_sums глобальной, чтобы поток мог писать
    static float static_col_sums[m] = {0};

    // Обновляем лямбду с использованием static_col_sums
    auto col_sum_fixed = [](LPVOID param) -> DWORD {
        int col_num = *(int*)param;
        float sum = 0;
        for (int i = 0; i < n; i++)
            sum += mtx[i][col_num];
        static_col_sums[col_num] = sum;
        return 0;
    };

    for (int i = 0; i < m; i++) {
        hColThreads[i] = CreateThread(NULL, 0, col_sum_fixed, &col_numbers[i], 0, NULL);
        if (hColThreads[i] == NULL)
            std::cout << "Ошибка создания потока подсчёта для столбца " << i << "\n";
    }

    WaitForMultipleObjects(m, hColThreads, TRUE, INFINITE);

    for (int i = 0; i < m; i++)
        CloseHandle(hColThreads[i]);

    int num_min = 0;
    float min = static_col_sums[0];
    for (int i = 1; i < m; i++) {
        if (static_col_sums[i] < min) {
            min = static_col_sums[i];
            num_min = i;
        }
    }

    std::cout << "Задание 1: Столбец с минимальной суммой: " << num_min << "\n";
}

// Задание 2: найти строку с максимальным средним значением
void task2_find_max_row_avg()
{
    fill_matrix_parallel();

    float row_avgs[n] = {0};

    HANDLE hAvgThreads[n];
    for (int i = 0; i < n; i++)
        row_numbers[i] = i;

    auto row_avg = [](LPVOID param) -> DWORD {
        int row_num = *(int*)param;
        float sum = 0;
        for (int j = 0; j < m; j++)
            sum += mtx[row_num][j];
        row_avgs[row_num] = sum / m;
        return 0;
    };

    for (int i = 0; i < n; i++) {
        hAvgThreads[i] = CreateThread(NULL, 0, row_avg, &row_numbers[i], 0, NULL);
        if (hAvgThreads[i] == NULL)
            std::cout << "Ошибка создания потока вычисления среднего строки " << i << "\n";
    }

    WaitForMultipleObjects(n, hAvgThreads, TRUE, INFINITE);

    for (int i = 0; i < n; i++)
        CloseHandle(hAvgThreads[i]);

    int max_row = 0;
    float max_avg = row_avgs[0];
    for (int i = 1; i < n; i++) {
        if (row_avgs[i] > max_avg) {
            max_avg = row_avgs[i];
            max_row = i;
        }
    }

    std::cout << "Задание 2: Строка с максимальным средним значением: " << max_row << "\n";
}
void task3_sort_columns()
{
    fill_matrix_parallel(); // Заполняем матрицу случайно, как обычно

    HANDLE hSortThreads[m];
    for (int i = 0; i < m; i++)
        col_numbers[i] = i;

    for (int i = 0; i < m; i++)
    {
        hSortThreads[i] = CreateThread(NULL, 0, sort_column, &col_numbers[i], 0, NULL);
        if (hSortThreads[i] == NULL)
            std::cout << "Ошибка создания потока сортировки столбца " << i << "\n";
    }

    WaitForMultipleObjects(m, hSortThreads, TRUE, INFINITE);

    for (int i = 0; i < m; i++)
        CloseHandle(hSortThreads[i]);

    std::cout << "Задание 3: Сортировка столбцов методом обмена завершена.\n";

    // Дополнительно можно вывести отсортированную матрицу, если нужно
}
float det3x3(float mat[3][3])
{
    return mat[0][0]*(mat[1][1]*mat[2][2] - mat[1][2]*mat[2][1])
         - mat[0][1]*(mat[1][0]*mat[2][2] - mat[1][2]*mat[2][0])
         + mat[0][2]*(mat[1][0]*mat[2][1] - mat[1][1]*mat[2][0]);
}

// Функция потока для вычисления слагаемого определителя
DWORD WINAPI determinant_term(LPVOID param)
{
    int index = *(int*)param;
    float val = matrix[0][index] * det3x3(minor3x3[index]);
    if (index % 2 == 1) val = -val;
    results[index] = val;
    return 0;
}

// Создание миноров (3x3) для разложения по первой строке
void create_minors()
{
    for (int col = 0; col < 4; col++)
    {
        int r = 0;
        for (int i = 1; i < 4; i++)  // пропускаем 0-ю строку
        {
            int c = 0;
            for (int j = 0; j < 4; j++)
            {
                if (j == col) continue;  // пропускаем столбец col
                minor3x3[col][r][c] = matrix[i][j];
                c++;
            }
            r++;
        }
    }
}

void task4_find_determinant_with_input()
{
    std::cout << "Введите элементы матрицы 4x4 построчно (через пробел):\n";
    for (int i = 0; i < 4; i++)
        for (int j = 0; j < 4; j++)
            std::cin >> matrix[i][j];

    create_minors();

    HANDLE hThreads[4];
    int indices[4] = {0, 1, 2, 3};

    for (int i = 0; i < 4; i++)
    {
        hThreads[i] = CreateThread(NULL, 0, determinant_term, &indices[i], 0, NULL);
        if (hThreads[i] == NULL)
            std::cout << "Ошибка создания потока для слагаемого " << i << "\n";
    }

    WaitForMultipleObjects(4, hThreads, TRUE, INFINITE);

    for (int i = 0; i < 4; i++)
        CloseHandle(hThreads[i]);

    float det = 0;
    for (int i = 0; i < 4; i++)
        det += results[i];

    std::cout << "Определитель введённой матрицы = " << det << "\n";
}
int main()
{
    srand((unsigned)time(NULL));

    std::cout << "Выберите задание (1 - найти столбец с минимальной суммой, 2 - найти строку с максимальным средним): ";
    int task;
    std::cin >> task;

    switch (task)
    {
        case 1:
            task1_find_min_col_sum();
            break;
        case 2:
            task2_find_max_row_avg();
            break;
        case 3:
            task3_sort_columns();
            break;
        case 4:
            task4_find_determinant_with_input();
            break;
        default:
            std::cout << "Задание с таким номером не предусмотрено.\n";
            break;
    }
    return 0;
}

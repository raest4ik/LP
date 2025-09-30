#include <iostream>
using namespace std;
#include <windows.h>
#include <ctime>

const int N = 4;
float matrix[N][N];     // исходная матрица

// Структура для передачи параметров в поток
struct ThreadData {
    int col;           // столбец для которого вычисляем минор
    float result;      // результат вычисления минора
};

// Функция для вычисления определителя матрицы 3x3
float determinant3x3(float a[3][3]) {
    return a[0][0] * (a[1][1] * a[2][2] - a[1][2] * a[2][1]) -
        a[0][1] * (a[1][0] * a[2][2] - a[1][2] * a[2][0]) +
        a[0][2] * (a[1][0] * a[2][1] - a[1][1] * a[2][0]);
}

// Функция для получения минора 3x3
void getMinor(float source[N][N], float dest[3][3], int row, int col) {
    int di = 0, dj = 0;
    for (int i = 0; i < N; i++) {
        if (i == row) continue;
        dj = 0;
        for (int j = 0; j < N; j++) {
            if (j == col) continue;
            dest[di][dj] = source[i][j];
            dj++;
        }
        di++;
    }
}

// Функция потока - вычисление одного элемента разложения
DWORD WINAPI calculate_minor(LPVOID param) {
    ThreadData* data = (ThreadData*)param;
    int col = data->col;

    // Получаем минор 3x3
    float minor[3][3];
    getMinor(matrix, minor, 0, col);

    // Вычисляем определитель минора
    float minor_det = determinant3x3(minor);

    // Учитываем знак (-1)^(i+j)
    float sign = (col % 2 == 0) ? 1.0f : -1.0f;

    // Вычисляем элемент разложения
    data->result = sign * matrix[0][col] * minor_det;

    cout << "Поток для столбца " << col+1 << ": " << data->result << endl;
    return 0;
}

// Функция для заполнения матрицы случайными числами
void fillMatrixRandom() {
    for (int i = 0; i < N; i++) {
        for (int j = 0; j < N; j++) {
            matrix[i][j] = rand() % 10;
        }
    }
}

int main() {
    setlocale(LC_ALL, "ru_RU.UTF-8");

    // Инициализация генератора случайных чисел
    srand((unsigned int)time(NULL));

    // Заполнение матрицы случайными числами
    fillMatrixRandom();

    // Вывод матрицы
    cout << "\nСгенерированная матрица:\n";
    for (int i = 0; i < N; i++) {
        for (int j = 0; j < N; j++) {
            cout << matrix[i][j] << "\t";
        }
        cout << endl;
    }

    // Подготовка данных для потоков
    ThreadData thread_data[N];
    HANDLE hThreads[N];
    DWORD dwThreadID[N];

    // Запуск потоков для вычисления элементов разложения
    for (int j = 0; j < N; j++) {
        thread_data[j].col = j;
        thread_data[j].result = 0;

        hThreads[j] = CreateThread(
            NULL, 0, calculate_minor, &thread_data[j], 0, &dwThreadID[j]);

        if (hThreads[j] == NULL) {
            cout << "Ошибка создания потока для столбца " << j << endl;
        }
    }

    // Ожидаем завершения всех потоков
    WaitForMultipleObjects(N, hThreads, TRUE, INFINITE);

    // Суммируем результаты для получения определителя
    float determinant = 0;
    for (int j = 0; j < N; j++) {
        determinant += thread_data[j].result;
    }

    cout << "Определитель матрицы: " << determinant << endl;

    // Закрытие потоков
    for (int j = 0; j < N; j++) {
        CloseHandle(hThreads[j]);
    }

    return 0;
}
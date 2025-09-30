#include <windows.h>
#include <iostream>
#include <ctime>

const int m = 10, n = 20; // m - число строк, n - число столбцов

float mtx[m][n];        // матрица вещественных чисел
float row_avgs[m];      // массив средних по строкам
int row_numbers[m];     // номера строк 

// Функция потока - заполнение одной строки случайными вещественными числами
DWORD WINAPI row_fill(LPVOID param)
{

    srand(time(0));
    
    int* prow_num = (int*)param;
    int row = *prow_num;

    for (int j = 0; j < n; ++j)
    {
        mtx[row][j] = (rand()) + (float)rand()/100;
    }
    return 0;
}

// Функция потока - вычисление среднего значения по одной строке
DWORD WINAPI row_avg(LPVOID param)
{
    int* prow_num = (int*)param;
    int row = *prow_num;

    float sum = 0.0f;
    for (int j = 0; j < n; ++j)
        sum += mtx[row][j];
    row_avgs[row] = sum / n;
    return 0;
}

void print_m(const float a[m][n]) {
    for (int i = 0; i < m; i++) {
        for (int j = 0; j < n; j++) {
            std::cout << " " << a[i][j];
        }
        std::cout << "\n";
    }
}

int main()
{
    setlocale(LC_ALL, "rus");
    srand(static_cast<unsigned>(time(NULL)));

    // Инициализация номеров строк
    for (int i = 0; i < m; ++i) {
        row_numbers[i] = i;
        row_avgs[i] = 0.0f;
    }

    // Потоки для заполнения строк
    HANDLE fillThreads[m];
    DWORD fillIDs[m];

    for (int i = 0; i < m; ++i) {
        fillThreads[i] = CreateThread(
            NULL,               // безопасность по умолчанию
            0,                  // размер стека по умолчанию
            row_fill,           // функция
            &row_numbers[i],    // параметр (номер строки)
            0,                  // флаги создания
            &fillIDs[i]);       // id потока

        // Проверям - создан ли поток
        if (fillThreads[i] == NULL)
        {
            std::cout << "Поток № " << i
                << "не был создан\n"
                << "Ошибка "
                << GetLastError();
        }
    }

    // Ждём завершения всех потоков заполнения
    WaitForMultipleObjects(m, fillThreads, TRUE, INFINITE);

    // Закрытие потоков
    for (int i = 0; i < m; ++i)
        CloseHandle(fillThreads[i]);

    std::cout << "Вывод матрицы:\n";
    print_m(mtx);

    // Потоки для вычисления среднего по строкам
    HANDLE avgThreads[m];
    DWORD avgIDs[m];

    for (int i = 0; i < m; ++i) {
        avgThreads[i] = CreateThread(
            NULL,
            0,
            row_avg,
            &row_numbers[i],
            0,
            &avgIDs[i]);

        if (avgThreads[i] == NULL)
        {
            std::cout << "Поток № " << i
                << "не был создан\n"
                << "Ошибка "
                << GetLastError();
        }
    }

    // Ждём завершения всех потоков вычисления
    WaitForMultipleObjects(m, avgThreads, TRUE, INFINITE);

    // Закрытие потоков
    for (int i = 0; i < m; ++i)
        CloseHandle(avgThreads[i]);

    // Находим номер строки с максимальным средним значением
    int num_max = 0;
    float max_avg = row_avgs[0];
    for (int i = 1; i < m; ++i) {
        if (row_avgs[i] > max_avg) {
            max_avg = row_avgs[i];
            num_max = i;
        }
    }


    std::cout << "Искомая строка с максимальным средним: № " << num_max
        << " (" << max_avg << ")\n";

    return 0;
}
#include <windows.h>
#include <iostream>
#include <ctime>

const int m = 10, n = 20; // m - число строк, n - число столбцов

float mtx[m][n];        // матрица вещественных чисел
int row_numbers[m];     // номера строк 
int col_numbers[n];     // номера столбцов

// Функция потока - заполнение одной строки случайными вещественными числами
DWORD WINAPI row_fill(LPVOID param)
{

    int* prow_num = (int*)param;
    int row = *prow_num;

    srand(time(0)+row);

    for (int j = 0; j < n; ++j)
    {
        mtx[row][j] = (rand()) + (float)rand() / 100;
    }
    return 0;
}



DWORD WINAPI col_sort(LPVOID param)
{
    int col = *(int*)param;
    for (int i = 0; i < m - 1; ++i) {
        for (int k = i + 1; k < m; ++k) {
            if (mtx[i][col] > mtx[k][col]) {
                float tmp = mtx[i][col];
                mtx[i][col] = mtx[k][col];
                mtx[k][col] = tmp;
            }
        }
    }
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
    // Инициализация номеров строк и столбцов
    for (int i = 0; i < m; ++i) row_numbers[i] = i;
    for (int j = 0; j < n; ++j) col_numbers[j] = j;
    // Потоки для заполнения строк
    HANDLE fillThreads[m];
    DWORD fillIDs[m];
    for (int i = 0; i < m; ++i) {
        fillThreads[i] = CreateThread(
            NULL,
            0,
            row_fill,
            &row_numbers[i],
            0,
            &fillIDs[i]);
        if (fillThreads[i] == NULL)
        {
            std::cout << "Поток заполнения № " << i << " не был создан. Ошибка " << GetLastError() << "\n";
            return 1;
        }
    }
    // Ждём завершения всех потоков заполнения
    WaitForMultipleObjects(m, fillThreads, TRUE, INFINITE);
    for (int i = 0; i < m; ++i) 
        CloseHandle(fillThreads[i]);
    std::cout << "Матрица до сортировки столбцов:\n";
    print_m(mtx);
    // Потоки для параллельной сортировки столбцов
    HANDLE sortThreads[n];
    DWORD sortIDs[n];
    for (int j = 0; j < n; ++j) {
        sortThreads[j] = CreateThread(
            NULL,
            0,
            col_sort,
            &col_numbers[j],
            0,
            &sortIDs[j]);
        if (sortThreads[j] == NULL)
        {
            std::cerr << "Поток сортировки столбца № " << j << " не был создан. Ошибка " << GetLastError() << "\n";
            // В случае ошибки закрываем уже созданные и выходим
            for (int k = 0; k < j; ++k) CloseHandle(sortThreads[k]);
            return 1;
        }
    }
    // Ждём завершения всех потоков сортировки
    WaitForMultipleObjects(n, sortThreads, TRUE, INFINITE);
    for (int j = 0; j < n; ++j) CloseHandle(sortThreads[j]);
    std::cout << "\nМатрица сортировки столбцов:\n";
    print_m(mtx);
    return 0;
}
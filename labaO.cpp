#include <windows.h>
#include <iostream>


const int m = 10, n = 20;
float mtx[m][n], // матрица
col_sums[n]; // массив сумм

int col_numbers[n]; // номера столбцов

// Функция потока
DWORD WINAPI col_sum(LPVOID param)
{
 // Получаем значение параметра 
 int* pcol_num = (int*)param;
 int col_num = *pcol_num;

 // Находим искомую сумму
 col_sums[col_num] = 0;
 for (int i = 0; i < m; i++)
  col_sums[col_num] += (mtx[i][col_num]);

 return 0;
}


DWORD WINAPI matrix_create(LPVOID param) {
 srand(time(0));
 // Получаем значение параметра 
 int* pcol_num = (int*)param;
 int i = *pcol_num;

 // Создание матрицы
 for (int j = 0; j < n; j++)
  mtx[i][j] = (float)(rand());

 return 0;
}


void print_m(float mtx[m][n]) {
 for (int i = 0; i < m; i++) {
  for (int j = 0; j < n; j++) {
   std::cout <<" " << mtx[i][j];
  }
  std::cout << "\n";
 }
}

int main(int argc) {

 setlocale(LC_ALL, "rus");
 // Описание переменных для работы с потоками
 // массив из n указателей потоков
 HANDLE hThread[n];
 // массив из n идентификаторов потоков
 DWORD dwThreadID[n];

 // массив из n указателей потоков для заполнения матрицы
 HANDLE createhThread[n];
 // массив из n идентификаторов потоков для заполнения матрицы
 DWORD createdwThreadID[n];

 // Заполнение массивов исходными значениями
 for (int i = 0; i < n; i++)
 {
  col_sums[i] = 0;
  col_numbers[i] = i;
 }



 //Запуск потоков создания матрицы 
 for (int i = 0; i < m; i++)
 {
  createhThread[i] = CreateThread(
   // атрибуты безопасности по умолчанию
   NULL,
   // размер стека по умолчанию
   0,
   // имя функции      
   matrix_create,
   // указатель на параметры    
   &(col_numbers[i]),
   // флаг создания = 0
   0,
   // адрес переменной для идентификатора   
   &(createdwThreadID[i]));

  // Проверям - создан ли поток
  if (createhThread[i] == NULL)
  {
   std::cout << "Поток № " << i
    << "не был создан\n"
    << "Ошибка "
    << GetLastError();
  }
 }

 

 // Ожидаем завершения потоков
 WaitForMultipleObjects(n, createhThread,
  true, INFINITE);

 // Закрытие потоков
 for (int i = 0; i < n; i++)
  CloseHandle(createhThread[i]);


 std::cout << "вывод матрицы: \n";
 print_m(mtx);



 //Запуск потоков подсчёта 
 for (int i = 0; i < n; i++)
 {
  hThread[i] = CreateThread(
   // атрибуты безопасности по умолчанию
   NULL,
   // размер стека по умолчанию
   0,
   // имя функции      
   col_sum,
   // указатель на параметры    
   &(col_numbers[i]),
   // флаг создания = 0
   0,
   // адрес переменной для идентификатора   
   &(dwThreadID[i]));

  // Проверям - создан ли поток
  if (hThread[i] == NULL)
  {
   std::cout << "Поток № " << i
    << "не был создан\n"
    << "Ошибка "
    << GetLastError();
  }
 }

 // Ожидаем завершения потоков
 WaitForMultipleObjects(n, hThread,
  true, INFINITE);

 // Находим номер столбца с минимальной суммой
 int num_min = 0;
 float min = col_sums[0];

 for (int i = 1; i < n; i++)
 {
  if (min > col_sums[i])
  {
   min = col_sums[i];
   num_min = i;
  }
 }

 // Вывод результата
 std::cout << "Искомый столбец № "
  << num_min << '\n';
 // Закрытие потоков
 for (int i = 0; i < n; i++)
  CloseHandle(hThread[i]);

 return 0;
}
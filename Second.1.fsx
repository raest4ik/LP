open System

// Функция для получения максимальной цифры из числа
let rec maxDigit (n: int) =
    // Извлекаем последнюю цифру числа
    let digit = abs n % 10
    // Если число состоит из одной цифры, возвращаем эту цифру
    if n / 10 = 0 then digit
    // Иначе, рекурсивно сравниваем текущую цифру с максимальной цифрой остальной части числа
    else max digit (maxDigit (n / 10))

// Функция для обработки списка чисел
let processList numbers =
    // Применяем функцию maxDigit к каждому числу в списке
    numbers
    |> List.map maxDigit

// Функция для генерации случайного списка чисел
let generateRandomList (count: int) =
    // Создаем генератор случайных чисел
    let rand = Random()
    // Генерируем список из count случайных чисел от 1 до 1000
    [1 .. count]
    |> List.map (fun _ -> rand.Next(1, 1000))

// Функция для вывода меню и получения ввода пользователя
let getUserInput () =
    // Выводим меню для пользователя
    printfn "Введите 'stop', чтобы завершить программу."
    printfn "Введите 'random', чтобы сгенерировать случайный список."
    printfn "Или введите список чисел через пробел: "
    // Читаем ввод пользователя
    Console.ReadLine()

// Функция для обработки ввода пользователя
let processUserInput (input: string) =
    // Проверяем, хочет ли пользователь завершить программу
    if input.ToLower() = "stop" then
        // Выводим сообщение о завершении и возвращаем false
        printfn "Программа завершена."
        false
    // Проверяем, хочет ли пользователь сгенерировать случайный список
    elif input.ToLower() = "random" then
        // Генерируем случайный список и обрабатываем его
        let randomList = generateRandomList 10
        printfn "Случайный список: %A" randomList
        printfn "Список максимальных цифр: %A" (processList randomList)
        // Возвращаем true для продолжения работы программы
        true
    else
        try
            // Разбиваем ввод на список чисел
            let numbers = input.Split([|' '|], StringSplitOptions.RemoveEmptyEntries)//сплитуем по пробелу и убираем лишние пробелы
                        |> Array.map int
                        |> Array.toList
            // Выводим введенный список и список максимальных цифр
            printfn "Введенный список: %A" numbers
            printfn "Список максимальных цифр: %A" (processList numbers)
            // Возвращаем true для продолжения работы программы
            true
        with
        // Обрабатываем исключение, если ввод некорректен
        | :? FormatException ->
            // Выводим сообщение об ошибке и возвращаем true для повторного ввода
            printfn "Некорректный ввод. Пожалуйста, попробуйте еще раз."
            true

// Основная функция программы
let rec main () =
    // Получаем ввод пользователя
    let input = getUserInput()
    // Если программа должна продолжать работать, вызываем main рекурсивно
    if processUserInput input then
        main ()




open System

// Функция для получения максимальной цифры из числа
let rec maxDigit (n: int) =
    let digit = abs n % 10
    if n / 10 = 0 then digit
    else
        if digit > maxDigit (n / 10) then digit
        else maxDigit (n / 10)

// Функция для обработки списка чисел
let processList numbers =
    numbers
    |> List.map maxDigit

// Функция для генерации случайного списка чисел
let generateRandomList (count: int) =
    let rand = Random()
    // Генерируем список из count случайных чисел от 1 до 1000
    [1 .. count]
    |> List.map (fun _ -> rand.Next(1, 1000))

// Функция для вывода меню и получения ввода пользователя
let getUserInput () =
    printfn "Введите 'stop', чтобы завершить программу."
    printfn "Введите 'random', чтобы сгенерировать случайный список."
    printfn "Или введите список чисел через пробел: "
    Console.ReadLine()

// Функция для обработки ввода пользователя
let processUserInput (input: string) =
    if input.ToLower() = "stop" then
        printfn "Программа завершена."
        false
    elif input.ToLower() = "random" then
        let randomList = generateRandomList 10
        printfn "Случайный список: %A" randomList
        printfn "Список максимальных цифр: %A" (processList randomList)
        true
    else
        try
            // Разбиваем ввод на список чисел
            let numbers = input.Split([|' '|], StringSplitOptions.RemoveEmptyEntries)//сплитуем по пробелу и убираем лишние пробелы
                        |> Array.map int//преобразуем строку в массив интовых значений
                        |> Array.toList//преобразовываем масив в список 
            printfn "Введенный список: %A" numbers
            printfn "Список максимальных цифр: %A" (processList numbers)
            true
        with
        | :? FormatException ->
            printfn "Некорректный ввод. Пожалуйста, попробуйте еще раз."
            true

let rec main () =
    let input = getUserInput()
    if processUserInput input then
        main ()
main()



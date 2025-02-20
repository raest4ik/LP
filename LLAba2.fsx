open System

let rec countDigits (n: int) : int = 
    if n < 10 then 1 
    else 1 + countDigits (n / 10) //подсчет кол-ва цифр числа рекурсивно целочисленным делением на 10



let mutable continueInput = true // Переменная для управления циклом
while continueInput do
    printf "Введите натуральное число (или 'stop' для выхода): "
    let input = Console.ReadLine() // Читаем ввод пользователя

    if input.Trim().ToLower() = "stop" then
        continueInput <- false // Завершаем цикл
    else
        match Int32.TryParse(input) with
        | (true, number) when number > 0 -> // Проверяем, является ли ввод натуральным числом
            let rec countDigits n =
                if n < 10 then 1 else 1 + countDigits (n / 10) // Рекурсивная функция подсчета цифр
            printfn "Количество цифр в числе %d: %d" number (countDigits number)
        | _ ->
            printfn "Пожалуйста, введите корректное натуральное число."

printfn "Программа завершена."

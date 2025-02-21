open System

// Функция для получения противоположных чисел
let getOppositeNumbers numbers =
    List.map (fun x -> -x) numbers

// Рекурсивная функция для обработки ввода
let rec processInput () =
    Console.WriteLine("Введите числа через пробел (или 'stop' для завершения):")
    let input = Console.ReadLine()
    
    // Проверка на команду 'stop'
    if input.Trim().ToLower() = "stop" then
        ()
    else
        // Преобразуем ввод в список чисел
        let numbers =
            input.Split(' ') // разбили по пробелу
            |> Array.toList // преобразовали в список строк
            |> List.choose (fun str -> 
                match Int32.TryParse(str) with // строка в целое число 
                | (true, num) -> Some num
                | _ -> None)

        // Получаем список противоположных чисел
        let opposites = getOppositeNumbers numbers

        // Выводим результат
        printfn "Противоположные числа: %A" opposites 

        // Рекурсивный вызов для следующего ввода
        processInput()

// Запускаем процесс ввода
processInput()

open System
// Определяем функцию для генерации случайной строки
let generateRandomString (length: int) =
    // Определяем строку с символами, из которых будет состоять случайная строка
    let chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
    let random = Random()
    // Создаем новую строку из массива случайных символов
    new string(Array.init length (fun _ -> chars.[random.Next(chars.Length)]))//массив длины lendth,генерим список из случайных

// Определяем функцию для подсчета суммарной длины строк в списке
let sumStringLengths (strings: string list) =
    // Используем List.fold для накопления суммы длин строк
    List.fold (fun (acc: int) (s: string) -> acc + s.Length) 0 strings//акумулятор хранит текущую длину строк,s-текущая строка

let checkInput (input: string) =
    if input.ToLower() = "stop" then
        None
    else
        Some input

let generateRandomList (count: int) (maxLength: int) =
    let random = Random()
    // Генерируем список случайных строк
    List.init count (fun _ -> generateRandomString (random.Next(maxLength + 1)))//список длины count, генерация рандом строки от индекса 0 до макс длины

let rec main (strings: string list) =
    printfn "Выберите вариант:"
    printfn "1. Ввести строку вручную"
    printfn "2. Сгенерировать случайную строку"
    printfn "Введите номер варианта (или 'stop' для выхода): "
    let input = Console.ReadLine()
    match checkInput input with
    | Some "1" ->
        printfn "Введите строку: "
        let userString = Console.ReadLine()
        let newStrings = userString :: strings
        printfn "Суммарная длина всех введенных строк: %d" (sumStringLengths newStrings)
        // Рекурсивно вызываем функцию main с обновленным списком
        main newStrings
    | Some "2" ->
        let randomString = generateRandomString 10
        let newStrings = randomString :: strings
        printfn "Сгенерированная строка: %s" randomString
        printfn "Суммарная длина всех введенных строк: %d" (sumStringLengths newStrings)
        main newStrings
    | Some _ ->
        printfn "Неправильный выбор. Пожалуйста, выберите 1 или 2."
        main strings
    | None ->
        printfn "Суммарная длина всех введенных строк: %d" (sumStringLengths strings)
        printfn "Программа завершена."

// Запускаем функцию main с пустым списком
main[]

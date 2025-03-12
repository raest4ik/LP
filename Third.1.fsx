open System
open System.IO

// ввод целых чисел
let rec INN count collected =
    if count = 0 then
        Some collected
    else
        printf "Введите целое число или 'stop' для выхода: "
        let input = Console.ReadLine().Trim()
        if input.ToLower() = "stop" then None
        else
            match System.Int32.TryParse(input) with
            | true, n -> INN (count - 1) (collected @ [n])
            | _ ->
                printfn "Ошибка: введите целое число!"
                INN count collected

// чтение чисел из файла
let readNumbersFromFileLazy (path: string) =
    lazy (
        seq {
            use reader = new StreamReader(path)
            while not reader.EndOfStream do
                let line = reader.ReadLine().Trim()
                match System.Int32.TryParse(line) with
                | true, n ->
                    printfn "Вычислено и выдано число: %d" n
                    yield n
                | _ -> printfn "Пропуск некорректной строки: %s" line
        } |> Seq.cache
    )


// получение последовательности максимальных цифр
let getMaxDigitsList (numbers: seq<int>) =
    let rec findMaxDigit n maxDigit =
        if n = 0 then maxDigit
        else
            let digit = abs (n % 10)
            let newMax = if digit > maxDigit then digit else maxDigit
            findMaxDigit (abs (n / 10)) newMax
    
    numbers |> Seq.map (fun n -> findMaxDigit n 0)


let rec main () =
    printfn "Выберите способ ввода списка:"
    printfn "1 - Ручной ввод"
    printfn "2 - Ввод из файла"
    printfn "0 - Выход"
    match Console.ReadLine() with
    | "1" ->
        printf "Введите количество чисел: "
        match System.Int32.TryParse(Console.ReadLine()) with
        | true, count when count > 0 ->
            match INN count [] with
            | Some numbers when numbers.Length > 0 ->
                let maxDigits = getMaxDigitsList (Seq.ofList numbers)
                printfn "Список максимальных цифр: %A" (Seq.toList maxDigits)
            | None -> printfn "Выход."
            main()
        | _ ->
            printfn "Ошибка: введите положительное целое число!"
            main()
    | "2" ->
        let path = "number.txt" 
        let numbers = readNumbersFromFileLazy path
        let maxDigits = getMaxDigitsList numbers.Value
        printfn "Список максимальных цифр из файла: %A" (Seq.toList maxDigits)
        main()
    | "0" -> printfn "Выход из программы"
    | _ ->
        printfn "Ошибка: неверный выбор"
        main()

main()

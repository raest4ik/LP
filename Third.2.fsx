open System
open System.IO

// подсчет суммарной длины строк
let getTotalStringLength (strings: seq<string>) =
    strings |> Seq.fold (fun acc s -> acc + s.Length) 0

// чтение строк из файла
let RSF (path: string) =
    lazy (
        seq {
            use reader = new StreamReader(path : string)
            while not reader.EndOfStream do
                yield reader.ReadLine()
        }
    )

//ввод строк с клавиатуры
let RS count =
    lazy (
        seq {
            for i in 1 .. count do
                printf "Введите строку или 'stop' для выхода: "
                let input = Console.ReadLine().Trim()
                if input.ToLower() = "stop" then yield! Seq.empty else yield input
        }
    )


let rec main () =
    printfn "Выберите способ ввода строк:"
    printfn "1 - Ручной ввод"
    printfn "2 - Ввод из файла"
    printfn "0 - Выход"
    match Console.ReadLine() with
    | "1" ->
        printf "Введите количество строк: "
        match System.Int32.TryParse(Console.ReadLine()) with
        | true, count when count > 0 ->
            let strings = RS count
            let totalLength = getTotalStringLength strings.Value
            printfn "Суммарная длина строк: %d" totalLength
            main()
        | _ ->
            printfn "Ошибка: введите положительное целое число!"
            main()
    | "2" ->
        let path = "number.txt" 
        let strings = RSF path
        let totalLength = getTotalStringLength strings.Value
        printfn "Суммарная длина строк из файла: %d" totalLength
        main()
    | "0" -> printfn "Выход из программы"
    | _ ->
        printfn "Ошибка: неверный выбор"
        main()

main()

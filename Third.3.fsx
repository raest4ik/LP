open System
open System.IO

// чтение файлов в каталоге и поиск первого по алфавиту 
let AlphaSearch (directoryPath: string) =
    try
        Directory.GetFiles(directoryPath)
        |> Seq.ofArray
        |> Seq.map Path.GetFileName//достали имя
        |> Seq.filter (fun file -> file.Length > 0 && Char.IsLetter(file.[0]))
        |> Seq.sortBy (fun file -> file.ToLower())
        |> Seq.tryHead
    with
    | :? DirectoryNotFoundException ->
        printfn "Ошибка: каталог не найден."
        None
    | ex ->
        printfn "Ошибка: %s" ex.Message
        None

let main () =
    printf "Введите путь к каталогу: "
    let directoryPath = Console.ReadLine().Trim()
    match AlphaSearch directoryPath with
    | Some file -> printfn "Первый по алфавиту файл : %s" (Path.GetFileName(file))
    | None -> printfn "Файл не найден или произошла ошибка."
    printfn "Выход из программы"

main()
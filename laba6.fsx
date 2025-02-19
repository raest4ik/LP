open System

let findArithmeticProgressionTerms s n d =
    let a1 = (float s - (float (n - 1) * float n / 2.0) * float d) / float n
    [ for i in 0 .. n - 1 -> a1 + float i * float d ]

let readInput () =
    printfn "Введите сумму членов арифметической прогрессии:"
    let s = Console.ReadLine() |> int

    printfn "Введите количество членов арифметической прогрессии:"
    let n = Console.ReadLine() |> int

    printfn "Введите разность арифметической прогрессии:"
    let d = Console.ReadLine() |> int

    (s, n, d) // Return the values as a tuple


let (s, n, d) = readInput ()  // Call the input function and unpack the tuple

let terms = findArithmeticProgressionTerms s n d

printfn "Арифметическая прогрессия: %A" terms

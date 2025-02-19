open System

let findGeometricProgressionTerms a1 q n =
    [ for i in 0 .. n - 1 -> a1 * (pown q i) ]

let readInput () =
    printfn "Введите первый член геометрической прогрессии (a1):"
    let a1 = Console.ReadLine() |> int

    printfn "Введите знаменатель геометрической прогрессии (q):"
    let q = Console.ReadLine() |> int

    printfn "Введите количество членов геометрической прогрессии (n):"
    let n = Console.ReadLine() |> int

    (a1, q, n)


let (a1, q, n) = readInput ()

let terms = findGeometricProgressionTerms a1 q n

printfn "Геометрическая прогрессия: %A" terms
open System

let geometricProgression () =
    printf "Введите первый член прогрессии: "
    let a = System.Console.ReadLine() |> float

    printf "Введите знаменатель прогрессии: "
    let r = System.Console.ReadLine() |> float

    printf "Введите количество членов прогрессии: "
    let n = System.Console.ReadLine() |> int

    [for i in 0..(n - 1) -> a * (r ** float i)]

let printList list =
    for term in list do
        printf "%g " term
    printfn ""

let progression = geometricProgression ()
printList progression
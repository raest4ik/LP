open System

let arithmeticProgression () =
    printf "Введите первый член прогрессии: "
    let a = System.Console.ReadLine() |> float

    printf "Введите разность прогрессии: "
    let d = System.Console.ReadLine() |> float

    printf "Введите количество членов прогрессии: "
    let n = System.Console.ReadLine() |> int

    [for i in 0..(n - 1) -> a + float i * d]

let printList list =
    for term in list do
        printf "%g " term
    printfn ""

let progression = arithmeticProgression ()
printList progression // Передаем список progression в функцию printList
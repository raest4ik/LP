let getInput () =
    printf "Введите число: "
    System.Console.ReadLine()

let Sumdel (x: int) =
    let rec calculateSum divisor sum =
        if divisor > x / 2 then
            sum + x
        else
            if x % divisor = 0 then
                calculateSum (divisor + 1) (sum + divisor)
            else
                calculateSum (divisor + 1) sum

    calculateSum 1 0

let x = int(getInput ())
let result = Sumdel x
printfn "Сумма делителей = %d " result

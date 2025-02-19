let getInput () =
    printf "Введите верхнюю границу диапазона: "
    System.Console.ReadLine()

let OTREZ (x: int) =
    let rec multiplesOfThree current x =
        if current > x then []
        else
            if current % 3 = 0 then
                current :: multiplesOfThree (current + 1) x
            else
                multiplesOfThree (current + 1) x
    multiplesOfThree 1 x



let x = int(getInput ())
let result = OTREZ x
printfn "%A" result



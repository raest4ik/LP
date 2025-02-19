let getInput () =
    printf "Введите кол-во эл-ов: "
    System.Console.ReadLine()

let rec fib n =
    match n with
    | 1 | 2 -> 1
    | n -> fib(n-1) + fib(n-2)

let printFibonacciSequence n =
    for i in 1..n do
        printf "%d " (fib i)

let n = int(getInput())
printFibonacciSequence(n)
let getInput () =
    printf "Введите длину в дюймах: "
    System.Console.ReadLine()  // Read and convert to float


let Convert(values:float)=
    let santim = 2.54 * values
    let metr = santim / 100.0
    let santimposle1 = santim % 100.0
    let millimetr = santimposle1 * 10.0 % 10.0
    (metr, santimposle1, millimetr)
printf"Введите длину в дюймах: "
let input = (float(getInput()))
let(m, sm, mm) = Convert(float(input))
printf$"{input} дюймов = {int(m)} м {int(sm)} см "
printfn "%g мм" mm
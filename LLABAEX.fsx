open System

type Tree =
    | Node of string * Tree * Tree
    | Empty


let rec insert value tree =
    match tree with
    | Empty -> Node(value, Empty, Empty)
    | Node(v, left, right) ->
        if value < v then Node(v, insert value left, right)
        elif value > v then Node(v, left, insert value right)
        else tree

let rec inputTree tree =
    printf "Введите строку (или 'stop' для завершения): "
    match Console.ReadLine() with
    | "stop" -> tree
    | input -> inputTree (insert input tree)


let randomTree size =
    let randomString () =
        let chars = "abcdefghijklmnopqrstuvwxyz123456789"
        let length = Random().Next(3, 4)
        String.init length (fun _ -> chars.[Random().Next(chars.Length)].ToString())
    let rec generate n tree =
        if n = 0 then tree
        else generate (n - 1) (insert (randomString()) tree)
    generate size Empty


let printTree tree =
    let rec printTreeIndented tree indent =
        match tree with
        | Empty -> ()
        | Node(value, left, right) ->
            printTreeIndented right (indent + 4)
            printfn "%s%s" (String.replicate indent " ") value
            printTreeIndented left (indent + 4)
    printTreeIndented tree 0

let rec mapTree f tree =
    match tree with
    | Empty -> Empty
    | Node(value, left, right) ->
        Node(f value, mapTree f left, mapTree f right)


let shiftChar c = char (int c + 1)
let shiftString s = String.map shiftChar s//для каждого в строке

let supernewt tree = mapTree shiftString tree//создание нового дерева от char+1

// Обход дерева и вывод в виде списков
let rec preOrder tree =
    match tree with
    | Empty -> []
    | Node(value, left, right) -> value :: preOrder left @ preOrder right


let rec selectTreeInput () =
    printfn "\nВыберите способ заполнения дерева:"
    printfn "1 - Вручную"
    printfn "2 - Случайные строки"
    printfn "0 - Назад"
    match Console.ReadLine() with
    | "1" -> Some(inputTree Empty)
    | "2" ->
        printf "Количество элементов: "
        match Int32.TryParse(Console.ReadLine()) with
        | true, count when count > 0 ->
            let tree = randomTree count
            printfn "\nИсходное дерево:"
            printTree tree
            Some tree
        | _ ->
            printfn "Ошибка: введите число > 0!"
            selectTreeInput()
    | "0" -> None
    | _ ->
        printfn "Ошибка выбора!"
        selectTreeInput()


let rec mainMenu () =
    printfn "\nПреобразование дерева с помощью map:"
    printfn "1 - Выбрать дерево"
    printfn "0 - Выход"

    match Console.ReadLine() with
    | "1" ->
        match selectTreeInput () with
        | Some tree ->
            printfn "\nИсходное дерево:"
            printTree tree
            printfn "\nПрямой обход (pre-order): %A" (preOrder tree)
            let mappedTree = supernewt tree
            printfn "\nПосле преобразования:"
            printTree mappedTree
            printfn "\nПрямой обход (pre-order): %A" (preOrder mappedTree)
        | None -> ()
        mainMenu()
    | "0" -> printfn "Выход."
    | _ ->
        printfn "Ошибка!"
        mainMenu()

mainMenu()

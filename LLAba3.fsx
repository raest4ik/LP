open System

// Функции для работы со списками

(* Функция для добавления элемента в список *)
let rec addElement element list =
    match list with
    | [] -> [element]  (* Если список пустой, возвращаем новый список с элементом *)
    | head :: tail -> head :: (addElement element tail)  (* Рекурсивно добавляем элемент в хвост списка *)

(* Функция для удаления первого вхождения элемента из списка *)
let rec removeElement element list =
    match list with
    | [] -> []  (* Если список пустой, возвращаем пустой список *)
    | head :: tail ->
        if head = element then tail  (* Если голова совпадает с элементом, возвращаем хвост *)
        else head :: (removeElement element tail)  (* Иначе оставляем голову и продолжаем поиск в хвосте *)

(* Функция для проверки, существует ли элемент в списке *)
let rec SUH element list =
    match list with
    | [] -> false  (* Если список пустой, элемент не найден *)
    | head :: tail ->
        if head = element then true  (* Если голова совпадает с элементом, возвращаем true *)
        else SUH element tail  (* Иначе продолжаем поиск в хвосте *)

(* Функция для объединения двух списков *)
let rec doublelist list1 list2 =
    match list1 with
    | [] -> list2  (* Если первый список пустой, возвращаем второй список *)
    | head :: tail -> head :: (doublelist tail list2)  (* Оставляем голову и объединяем оставшуюся часть с вторым списком *)

(* Функция для получения элемента по индексу в списке *)
let rec getElementAt index list =
    match list with
    | [] -> None  (* Если список пустой, возвращаем None *)
    | head :: tail ->
        if index = 0 then Some head  (* Если индекс равен нулю, возвращаем Some от головы *)
        else getElementAt (index - 1) tail  (* Иначе уменьшаем индекс и продолжаем поиск в хвосте *)

// Функция для обработки команд пользователя
let processCommand command myList =
    match command with
    | "закончи" -> 
        printfn "Завершение программы."
        None // Возвращаем None, чтобы завершить выполнение

    | "добавь" ->
        printfn "Введите элемент для добавления:"
        let element = Console.ReadLine()
        let updatedList = addElement element myList
        printfn "Список после добавления: %A" updatedList
        Some updatedList

    | "удали" ->
        printfn "Введите элемент для удаления:"
        let element = Console.ReadLine()
        let updatedList = removeElement element myList
        printfn "Список после удаления: %A" updatedList
        Some updatedList

    | "найди" ->
        printfn "Введите элемент для поиска:"
        let element = Console.ReadLine()
        let exists = SUH element myList
        printfn "Элемент %s %s в списке." element (if exists then "есть" else "нет")
        Some myList

    | "соедени" ->
        printfn "Введите элементы второго списка через пробел:"
        let secondListInput = Console.ReadLine().Split(' ')
        let secondList = Array.toList secondListInput
        let updatedList = doublelist myList secondList
        printfn "Список после сцепки: %A" updatedList
        Some updatedList

    | "номер" ->
        printfn "Введите индекс элемента:"
        let indexStr = Console.ReadLine()
        match Int32.TryParse(indexStr) with
        | (true, index) ->
            match getElementAt index myList with
            | Some value -> printfn "Элемент на индексе %d: %s" index value
            | None -> printfn "Индекс вне диапазона."
            Some myList
        | _ ->
            printfn "Некорректный индекс."
            Some myList

    | _ ->
        printfn "Неизвестная команда. Пожалуйста, попробуйте снова."
        Some myList


let mutable myList = []
let mutable running = true // Переменная для управления циклом

while running do
    printfn "Введите команду (добавь/удали/найди/соедени/номер/закончи):"
    let command = Console.ReadLine()
    
    // Обрабатываем команду и обновляем список, если нужно
    match processCommand command myList with
    | Some updatedList -> myList <- updatedList // Обновляем список, если команда вернула новый список
    | None -> running <- false // Завершаем цикл, если команда вернула None
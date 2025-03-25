
main :-
    write('Введите список целых чисел в формате [1,2,3,4,5]: '),
    read(List),
    sum_even(List, Sum),
    write('Сумма чётных элементов списка: '), write(Sum), nl,
    main.


sum_even([], 0).  

sum_even([H|T], Sum) :-
    H mod 2 =:= 0, 
    sum_even(T, Sum1),  
    Sum is H + Sum1.  

sum_even([_|T], Sum) :-
    sum_even(T, Sum).  
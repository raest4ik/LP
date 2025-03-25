
expedition(Bio, Hyd, Syn, Rad, Mech, Doc) :-
    member(Bio, [e, g]),
    member(Hyd, [b, f]),
    member(Syn, [f, g]),
    member(Rad, [c, d]),
    member(Mech, [c, h]),
    member(Doc, [a, d]),

    % Проверка уникальности каждого участника
    all_different([Bio, Hyd, Syn, Rad, Mech, Doc]),

    % Условия
    (Hyd = f -> member(b, [Bio, Syn, Rad, Mech, Doc]) ; true),  % F не может ехать без B
    (Doc = d -> member(c, [Bio, Hyd, Syn, Rad, Mech, Doc]),
                 member(h, [Bio, Hyd, Syn, Rad, Mech, Doc]) ; true),  % D не может без C и H
    \+ (Rad = c, member(g, [Bio, Hyd, Syn, Mech, Doc])),  % C и G не могут быть вместе
    \+ (Doc = a, member(b, [Bio, Hyd, Syn, Rad, Mech, Doc])).  % A и B не могут быть вместе


all_different([]).
all_different([H|T]) :-
    \+ member(H, T),
    all_different(T).


print_expeditions :-
    findall([Bio, Hyd, Syn, Rad, Mech, Doc], expedition(Bio, Hyd, Syn, Rad, Mech, Doc), Teams),
    print_results(Teams).


print_results([]) :-
    write('Больше нет возможных составов'), nl.
print_results([H|T]) :-
    write('Возможный состав: '), write(H), nl,
    print_results(T).


main :-
    print_expeditions.
    

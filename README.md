# Dokumentace

Dokumentace k generátoru pro příklad `Kocourkovské volby`[link](https://kam.mff.cuni.cz/~balko/lpko1920/ukolPrakticky.pdf) napsaném v jazyce C#.

## Sestavení

K sestavení otevřete projekt ve vámi zvoleném IDE podporující C# a spusťte build.

## Ovládání

Generátoru se jako argument pošlu path ke vstupnímu souboru a on vygeneruje souboru `vygenerovane_lp.mod` ve stejne složce, jako je samotné `.exe`, ve kterém bude odpovídající lineární program, na který stačí pustit `glpsol -m vygenerovane_lp.mod`.

## Lineární program

Příklad výstupu:
```c
// Obyvatele
set Nodes := 0..4;
// Jednotlive politicke strany
set PossibleParties := 1..5;
// Hrany jsou pouze ty, ktere chybeli v puvodnim grafu.
set Edges := {(0,4)};
var Parties, >= 1, <= 5, integer;
// Pokud je obyvatel v pol. strane >> 1, jinak 0.
var nodeInParty{i in Nodes, p in PossibleParties}, binary;
minimize obj: Parties;
// Vrcholy na hrane musi byt v rozdilne strane.
s.t. edgeCon{(i, j) in Edges, p in PossibleParties}:
  nodeInParty[i, p] + nodeInParty[j , p] <= 1;
// Kazdy vrchol musi mit prave jednu stranu.
s.t. oneParty{i in Nodes}:
  sum{p in PossibleParties} nodeInParty[i, p] = 1;
// Podminka pro prirazovani stran nejprve od 1 a pote dalsi.
s.t. smallestSet{i in Nodes, p in PossibleParties}:
  nodeInParty[i, p] * p <= Parties;
solve;
printf "#OUTPUT: %d\n", Parties;
for {i in Nodes}
{
  for {p in PossibleParties}
  {
    // Kvuli podmince `smallestSet` je p od 1, ale vypis chceme od 0.
    printf (if nodeInParty[i,p] = 1 then "v_%d: %d\n" else ""), i, (p-1);
  }
}
printf "#OUTPUT END\n";
```

Ze vstupu se vytvoří množina vrcholů `Nodes`, hran, které nejsou v původním grafu mezi nimi `Edges` a `PossibleParties`, která symbolizuje maximální množinu politických stran, je tedy omezená počtem obyvatel.

Proměnnou je indikátor, zda je vrchol (obyvatel) i v politické straně p.

Program se snaží minimalizovat počet politických stran `Parties`.

Podmínky vycházejí z toho, že řešíme problém obarvení complementárního grafu ke grafu, který je na vstupu. Protože hrany, které na vstupu nejsou v zásadě značí relaci "nesmí být spolu/nesnesou se" >> musí mít jinou barvu, respektive být v jiné politické straně.

### Nevýhoda implementace

Řešení je pomalé.

(Příklad pro `vstup-s6.txt`)
```
INTEGER OPTIMAL SOLUTION FOUND
Time used:   316.3 secs
Memory used: 15.4 Mb (16125409 bytes)
#OUTPUT: 7
```

(Příklad pro `vstup-s8.txt`)
```
INTEGER OPTIMAL SOLUTION FOUND
Time used:   517.0 secs
Memory used: 30.6 Mb (32111250 bytes)
#OUTPUT: 8
v_0: 2
v_1: 5
v_2: 0
v_3: 3
v_4: 4
v_5: 4
v_6: 1
v_7: 1
v_8: 6
v_9: 3
v_10: 3
v_11: 7
v_12: 6
v_13: 1
v_14: 2
v_15: 6
v_16: 5
v_17: 2
v_18: 2
v_19: 5
v_20: 0
#OUTPUT END
```

### Nemá optimální řešení

Program by vždy měl najít řešení, protože vždy může každý být sám v politcké straně ~ worst case.

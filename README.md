# Dokumentace

Dokumentace k generátoru pro příklad `Kocourkovské volby`[link](https://kam.mff.cuni.cz/~balko/lpko1920/ukolPrakticky.pdf) napsaném v jazyce C#.

## Sestavení

K sestavení otevřete projekt ve vámi zvoleném IDE podporující C# a spusťte build.

## Ovládání

Generátoru se jako argument pošlu path ke vstupnímu souboru a on vygeneruje souboru `vygenerovane_lp.mod` ve stejne složce, jako je samotné `.exe`, ve kterém bude odpovídající lineární program, na který stačí pustit `glpsol -m vygenerovane_lp.mod`.

## Lineární program

--todo--


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

--todo--

---
title: F# List Scan
date: "2020-06-25"
description: "Exploration of F#'s `List.scan`"
---

By now you've likely seen both the `List.fold` and `List.reduce` in action.  They're used to take a list of items and boil them down to a single item.

Take the following `data` list as an example:

```fsharp
let data = [1..10]

// Note:  `(+)` is shorthand for `fun a b -> a + b`
// `fold` requires an initial state but will not fail on an empty list.
let fold = data |> List.fold(+) 0
// `reduce` doesn't require an initial state but will fail with an empty list.
let reduce = data |> List.reduce(+)
// val data : int list = [1; 2; 3; 4; 5; 6; 7; 8; 9; 10]
// val fold : int = 55
// val reduce : int = 55
```

As expected, the sum of 1 through 10 is 55.

# Use Scan to Accumulate

> What if we need to compute the sums at each item? 

The type definition of `List.scan` is

```fsharp
val scan: 
   folder: 'State -> 'T -> 'State ->
   state : 'State   ->
   list  : list<'T> 
        -> list<'State>
```

This may be hard to read at first, but essentially this type signature says:

1. Take an initial `'State` named `state`.
2. Fold the `'State` with the item `'T` to a new `State`
3. Accumulate the results into `list<'State>`

Now, for the sums up to each item in `data`.

```fsharp
let scanned = data |> List.scan(fun a b -> a + b) 0;;
// val scanned : int list = [0; 1; 3; 6; 10; 15; 21; 28; 36; 45; 55]
```

Very nice!  The `List.scan` includes the initial state `0` in the results, and it computes the sum up to each item.

## `List.scan` Can Do More Than Fold!

Probably more appropriate to the name `scan` can be seen when providing the previous item with the next item while iterating.

```fsharp
// (Previous, Next)
let tupled =
    data
    |> List.scan(fun a b -> 
        // Create a new tuple with the 
        // second value of a and b.
        snd a, b) (0, 0)
// val tupled : (int * int) list =
//  [(0, 0); (0, 1); (1, 2); (2, 3); (3, 4); (4, 5); (5, 6); (6, 7); (7, 8);
//   (8, 9); (9, 10)]
```

## Find Sub Directories

And, finally, I've come across the need to calculate all sub directories from a path.

```fsharp
let subDirectory = @"some\deep\sample\folder\path"
let paths = subDirectory.Split(@"\", System.StringSplitOptions.None) |> List.ofSeq

let directories = 
    paths.Tail
    // Note:  `sprintf @"%s\%s"` is shorthand for `fun a b -> sprintf @"%s\%s" a b`
    |> List.scan (sprintf @"%s\%s") paths.Head

// val subDirectory : string = "some\deep\sample\folder\path"
// val paths : string list = ["some"; "deep"; "sample"; "folder"; "path"]
// val directories : string list =
//   ["some"; "some\deep"; "some\deep\sample"; "some\deep\sample\folder";
//    "some\deep\sample\folder\path"]
```
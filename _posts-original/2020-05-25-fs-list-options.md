# F# List of Options

In F# development, I frequently come across a situation where I need to get the values out of a list of options.  Take the following list of `possibleNums`:

## Filter

```fsharp
let possibleNums : int option list = 
    [
        Some 1
        None
        None
        Some 2
        Some 3
        None
        None
        Some 4
        Some 5
        None
    ]
```

An obvious way to get the values out of the options in the list is to do the following:

```fsharp
let numsFiltered = 
    possibleNums
    |> List.filter Option.isSome
    |> List.map Option.get
```

Great!  Filter the list where there is some value and then extract the value.

But wait!  There's a better way.

## Choose

The F# `List.choose` type definition is:

```fsharp
val choose: 
   chooser: 'T -> 'U option ->
   list   : list<'T>        
         -> list<'U>
```

which shows that we can pass a function that will return an option and it will extract the value from that option.  Well that sounds familiar!

Let's combine that with the `val id: 'T -> 'T` function from F#:

```fsharp
let numsChosen = 
    possibleNums
    |> List.choose id
```

So let's let F# do the work to `choose` the values out of the options.

```output
val possibleNums : int option list =
  [Some 1; None; None; Some 2; Some 3; None; None; Some 4; Some 5; None]
val numsFiltered : int list = [1; 2; 3; 4; 5]
val numsChosen : int list = [1; 2; 3; 4; 5]
```
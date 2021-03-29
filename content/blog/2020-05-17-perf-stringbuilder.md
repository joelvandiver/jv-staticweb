---
title: PERF StringBuilder
date: "2020-05-17"
description: "Numerous .NET resources reference `StringBuilder` as the preferred method for building strings rather than concatentating."
---

Numerous .NET resources reference `StringBuilder` as the preferred method for building strings rather than concatentating.  

The Microsoft docs describe it this way:

> "The String object is immutable. Every time you use one of the methods in the System.String class, you create a new string object in memory, which requires a new allocation of space for that new object. In situations where you need to perform repeated modifications to a string, the overhead associated with creating a new String object can be costly."

> ***Ref***:  https://docs.microsoft.com/en-us/dotnet/standard/base-types/stringbuilder

But, just how costly is it?  Let's setup a quick performance test to investigate.  We'll create two functions:  `stringAdder`, `stringBuilder`.  Both will ignore the final results since we are investigating performance only.  Also, let's use a simple string of the alphabet.

```fsharp
open System

// Simple concatentation
let stringAdder : string list -> unit = List.reduce (+) >> ignore
// Build up the final string with StringBuilder
let stringBuilder (xs: string list) = 
    let buf = new Text.StringBuilder()
    buf.Append xs.Head |> ignore
    xs.Tail
    |> List.iter(fun x -> buf.Append x |> ignore)
    buf 
    |> string
    |> ignore
let genText (folder: string list -> unit) count = 
    let xs = [for _ in 0..count -> "abcdefghijklmnopqrstuvwxyz"]
    xs
    |> folder

10 |> genText stringAdder
100 |> genText stringAdder
1000 |> genText stringAdder
10000 |> genText stringAdder
100000 |> genText stringAdder

10 |> genText stringBuilder
100 |> genText stringBuilder
1000 |> genText stringBuilder
10000 |> genText stringBuilder
100000 |> genText stringBuilder
1000000 |> genText stringBuilder
10000000 |> genText stringBuilder
```

Wrapping each of working lines above with `#time` in fsi generates the following stats.

| Type    | Count       | Time         |
| ------- | ----------- | ------------ |
| Adder   | 10          | 00:00:00.002 |
| Adder   | 100         | 00:00:00.000 |
| Adder   | 1,000       | 00:00:00.020 |
| Adder   | 10,000      | 00:00:01.629 |
| Adder   | 100,000     | ***Overflow  |
| Builder | 10          | 00:00:00.001 |
| Builder | 100         | 00:00:00.000 |
| Builder | 1,000       | 00:00:00.000 |
| Builder | 10,000      | 00:00:00.001 |
| Builder | 100,000     | 00:00:00.031 |
| Builder | 1,000,000   | 00:00:00.292 |
| Builder | 10,000,000  | 00:00:03.078 |
| Builder | 100,000,000 | ***Overflow  |


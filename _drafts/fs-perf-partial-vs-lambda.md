# F# Performance of Partial Application vs Lambda Functions

```fsharp
module StackFrame =
    let partial (a: int) (b:int) = "partial value"
    let lambda (a: int) = 
        printfn "Creating a lambda"
        fun (b: int) -> "lambda value"
    do
        printfn "applying lambda"
        let applied_lambda = lambda 1
        printfn "%s" (applied_lambda 2)


        printfn "directly invoke the lambda"
        lambda 1 2 |> printfn "%s"


module PERF = 
    let partial (a: int) (b:int) = "partial value"
    let lambda (a: int) = fun (b: int) -> "lambda value"


    let partially_applied = partial 1
    let applied_lambda = lambda 1
    
let data = [0..1000000]
#time
data |> List.map PERF.partially_applied |> ignore
#time
#time
data |> List.map PERF.applied_lambda |> ignore
#time

```
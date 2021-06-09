
let partial (a: int) (b: int) = 
    $"Partial Application: {a} + {b} = {a + b}"
let lambda (a: int) = fun (b: int) -> 
    $"Lambda: {a} + {b} = {a + b}"

printfn "Partial Application of 1"
let appliedPartial = partial 1
printfn "1 Partially Applied"
printfn "%s" (partial 1 2)

printfn "Creating Lambda with 1"
let appliedLambda = lambda 1
printfn "Lambda Created with 1"
printfn "%s" (appliedLambda 2)

    
let data = [0..10000000]
#time
data |> List.map appliedPartial |> ignore
#time
#time
data |> List.map appliedLambda |> ignore
#time


---
title: F# Decorators
date: "2021-04-05"
description: "Functional implementation of decorators"
---

## Python Decorators

I find the Python notion of a decorator intriguing.  Let's take an example:

```python
import functools

def logit(func):
    """Prints messages when entering and leaving the decorated function"""    
    @functools.wraps(func)    
    def wrapper(*args, **kwargs):
        print("Entering: ", func.__name__)
        result = func(*args, **kwargs)
        print("Result: ", result)
        print("Exiting: ", func.__name__)
        return result
    return wrapper

@logit
def adder(a, b):
    return a + b

def main():
    returned_value = adder(1, 2)
    print("Returned Value: " + str(returned_value))

if __name__ == '__main__':
    main()
```

> Output

```
Entering:  adder
Result:  3
Exiting:  adder
Returned Value: 3
```

In the code above, a **decorator** function, `logit`, takes a function, `func`, and wraps it with the `wrapper` logic. Any function then can be *decorated* with the `@logit` **decorator**.   

## .NET Attributes

.NET on the other hand provides attributes that you can markup your classes, methods, properties, etc. with extra logic.  Here's an example of an attribute in C#:

```csharp
[DoSomething]
public class MyClass
{

}

```

The `[DoSomething]` attribute in C# can be used to provide extra functionality on the `MyClass` type.  Though this feature of .NET does provide *decoration* of a class, it is not quite as flexible as the Python version.  The most notable differences are the `*args` and `**kwargs` that provide variable arguments and key-word arguments into the wrapper function that are *spread* to the *decorated* function.  C# doesn't have a notion of *spreading* variable arguments onto a function call (unless you drop down to obscure .NET reflection).

## Functional Decorators

Is there a way to implement a functional style of *decoration* in .NET similar to Python?  Let's take a crack at this with F#:

```fsharp
open System.Diagnostics

// Create a performance timer decorator function.
let performanceTimer func = 
    let wrapper x = 
        let timer = new Stopwatch()
        printfn "Starting Timer for Parameter:  %A" x
        let result = func x
        timer.Stop()
        printfn "Stopped:  %A" timer.Elapsed
        result
    wrapper

// Create the function to decorate.
let getSum = fun x -> [0..x] |> List.reduce (+)

// Create the decorated function.
let logAdder = getSum |> performanceTimer

// Test the decorated function.
logAdder (100000000)

```

> Output

```
Starting Timer for Parameter:  100000000
Stopped:  00:00:00
val performanceTimer : func:('a -> 'b) -> ('a -> 'b)
val getSum : x:int -> int
val logAdder : (int -> int)
val it : int = 987459712
```

Very nice!  F# allows computing new functions from functions just like in Python.  Since every function in F# takes *exactly* one parameter (taking into account partial application and tuples as a single parameter), the need of a spread operator is avoided.  Note that F# infers the type of the `performanceTimer` to be `func:('a -> 'b) -> ('a -> 'b)` which is properly generic and is only later specified to be about integers.  

Also, an added benefit of this construction is that the final, *decorated* function, `logAdder`, has the same type, `int -> int`, as the function to decorate, `getSum`.
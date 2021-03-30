---
title: F# Duck Typing
date: "2020-06-07"
description: "Duck typing follows from the phrase 'If it walks like a duck and it quacks like a duck, then it must be a duck.'"
---

## Run-time Duck Typing

In a dynamically typed language such as `JavaScript`, you would *infer* the type of an object by investigating it's members.  Since `JavaScript` is not a statically typed language, we cannot trust the type information without checking that at run-time.  It is common to see code such as this in `JavaScript`.

```javascript
if (myObj.Id) {
    doSomethingWithId(myObj.Id);
}
```

That's great that we can support dynamic member checking, but is there a way that we can have the compiler tell us this ahead of time?

## .NET Generics

Coming from `C#`, I was already comfortable with generic classes and methods.  These provide a *template* of a code that can be applied to more specific types.  Generics can lead to better code abstraction and reuse.  You can think of generics as contraints upon the types that are used by the generic class.  

F# has all of the .NET Generics just like C#.

## Compile-time Duck Typing

But, F# goes a step further.  F# offers static generic constraints.  

Whereas C#'s generics are applied to instances of types at run-time, the F#'s [static type parameters](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/generics/statically-resolved-type-parameters) are resolved by the compiler at compile time!

Let's take an example...

```fsharp
open System

type User =
    { Id : Guid
      Name : string }

let inline getId (record: ^T) : Guid = (^T : (member Id : Guid) (record))

let luke = { Id = Guid.NewGuid(); Name = "Luke Skywalker" }
let id = getId luke
```

> Output:

```fsharp
type User =
  { Id: System.Guid
    Name: string }
val inline getId :
  record: ^T -> System.Guid when  ^T : (member get_Id :  ^T -> System.Guid)
val luke : User = { Id = 802f4714-7ef7-474b-af6b-f3fa05a661f9
                    Name = "Luke Skywalker" }
val id : Guid = 802f4714-7ef7-474b-af6b-f3fa05a661f9
```

At first glance, this may seem like nothing special is going on here.  But, note that I am able to apply the `getId` function to any type that has an `Id` of type `Guid`.  This cuts down the impulse to create type inheritance to *share* simple data structures and methods among types. 

I'm able to apply *Duck Typing* to types and have the compiler tell me as I'm developing if the type ***is a duck!***



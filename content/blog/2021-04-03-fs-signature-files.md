---
title: F# Signature Files
date: "2021-04-03"
description: "Put your library code behind signature files!"
---

[F# signature files](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/signature-files) are a great tool for providing:

1. [Encapsulation](#encapsulation)
2. [Type Inference Performance](#type-inference-performance)

## Encapsulation

> Interface and Implementation Separation

When I first started coding in F#, I found the type syntax to be a bit foreign to me.

```fsharp
// Define a simple function.
let f x = x + 3
// Output (Type Syntax):  val f : x:int -> int
```

This type syntax, `val f : x:int -> int`, seems strange but conveys a lot of meaning.  This function signature—the function name, its parameters, and its output—should be all that's needed to understand the purpose of the function.

For example, `val getUser: username:string -> User`, should tell you that you can get a `User` from a `username`.  Usually, you shouldn't care about *how* to get the user.  The implementation code can just be considered details.

The primary reason to use the F# signature files in large codebases and multiple assemblies is to enforce this **encapsulation**.  

It is surprisingly easy to inadvertently create a breaking change in a lower assembly.  Putting your code behind an interface can allow more freedom in changing the implementation without concern of breaking consumers.  Also, the consumers of your library have less information to sift through to get to the critical functionality.

> With private functions, I tend to rely on type inference to figure out the full function signatures (as I did above), but when a function is exposed publicly, I prefer to explicitly declare this type signature.

## Type Inference Performance

I first came across the idea that [signature files can improve performance](https://devblogs.microsoft.com/dotnet/f-and-f-tools-update-for-visual-studio-16-9/#big-performance-gains-for-codebases-with-f-signature-files) a few weeks ago.  This idea make sense to me.  If you explicitly tell the compiler what the types are, it doesn't have to figure it out for you.  

I've especially seen huge improvements with assemblies that rely on type providers since the type space is computed remotely.  

## BONUS POINTS

> Signature Generation

The F# compiler comes with a nice signature generation tool.  You can run it with the following command.

```
dotnet build -p:OtherFlags=--sig:My.Assembly.fsi
```

You can use it to *start off* your signature file, but I'd recommend statically defining your signature files in your assemblies.

Cheers!
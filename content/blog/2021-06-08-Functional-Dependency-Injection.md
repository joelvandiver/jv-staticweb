---
title: Functional *Dependency Injection* 
date: 2021-06-08
description: Put the FP back into DI!
---

If you've been developing for at least a year or two with an object oriented language, then you probably have come across the idea of *Dependency Injection*.  *Dependency Injection* is useful to isolate dependencies and provide loose coupling of components.  

Typically, in C# you define interfaces that provide the required dependencies and constructors (and properties) that require those dependencies.  Then, you would implement an IoC container that does the work to construct the actual dependencies at runtime.  

*Dependency Injection* is great for handling external connections since testing code that calls external connections without injection requires integration.

I remember struggling with the idea of *Dependency Injection* for a few years.  I understood the mechanics of what I needed to do to get the system to work, but I didn't really understand *why* I needed to do them.  

I then began to break apart the phrase.  What qualifies as a "dependency"?  What does it mean to inject them?  What's the smallest possible interface of a dependency?  

Later, through my study of F#, I came across the idea that a function is the *smallest* possible interface.  It defines a name, some input, and some output.  That's it.  

Aren't all parameters dependencies?  In one sense, yes, but in context to *Dependency Injection*, no.  The focus on *Dependency Injection* is on separating the construction of objects from the behavior.  Functions that require parameters may not be created by the client directly.  

But, to setup functional *Dependency Injection* with F#, you can put your dependencies behind a computed function.

Let's take a real world example.

```fsharp
type HttpResult = ... // Type to capture the results of an HttpResponseMessage

/// Encapsulates the HttpClient SendAsync 
let request : HttpRequestMessage -> Async<HttpResult> = 
    fun (req: HttpRequestMessage) -> 
        async {
            // Handle the lifetime of the client.
            use client = new HttpClient()
            // Await the response.
            let! response = client.SendAsync(req) |> Async.AwaitTask
            // Map the repsonse to a result 
            // since the client will soon be disposed.
            return response |> mapToHttpResult
        }

module Lib =
    /// Fetch the user by name with the `request` function.
    let fetchUserByName (request: HttpRequestMessage -> Async<HttpResult>) (name: string) : User option = 
        // Use `request` to call web service.
        ...

/// Get the user by name.
let getUser : string -> User option = Lib.fetchUserByName request
```

The critical dependency of `Lib.fetchUserByName` is required *first*:  `request: HttpRequestMessage -> Async<HttpResult>`.  This function will likely make an actual web request and has a pretty hefty, disposable object, `HttpClient`, backing it.  

The function, `Lib.fetchUserByName`, doesn't care about *how* the http request is made or how the connections and objects are disposed. The `getUser` also shouldn't care about how to make an http request or even how.  We can now use `getUser` like this:

```fsharp
let johnOption : User option = getUser "John"
```

But, probably the coolest part of all this is that you can swap out dependencies in computing the `getUser` function as long as the final computed function resolves to `string -> User option`.  You could, for example, get the user from a SQL database or a NoSQL data store, or whatever!

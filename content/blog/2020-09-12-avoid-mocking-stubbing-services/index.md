---
title: Avoid Mocking/Stubbing Services
date: "2020-09-12"
description: "Mocking versus stubbing test interfaces"
---

Let's say that we have a function that calls an external service:

```fsharp
let sayHelloToCurrentUser () = 
    // Call External Service
    AccountService.getUsername()
    |> sprintf "Hello %s!"

// Integration Test
let [<Fact>] ``should say Hello to the name``() =
    sayHelloToCurrentUser()
    |> equal "Hello Joel!"
```

That may look simple enough, except that this function is technically an integration function since it makes an external call.  It is difficult to guarantee the service availability and connections in a CI (continuous integration) environment.  

It is best to isolate the external calls in UNIT tests so that the run time only has to have the code itself to run the tests.

There are two primary options you can use to isolate the external call.

1.  Put the service call behind an interface and provide fake implementations with mocking or stubbing.
2.  Isolate the "testable" functions from the service connections.

For the first approach, we can provide a callback function to `getUsername`.

```fsharp
let sayHelloToCurrentUser (getUsername: unit -> string) = 
    getUsername()
    |> sprintf "Hello %s!"

// UNIT Test
let [<Fact>] ``should say Hello to the name``() =
    // Function Stub
    fun _ -> "Joel"
    |> sayHelloToCurrentUser
    |> equal "Hello Joel!"
```

The complexity of stubbing a service can grow rather quickly, though.  You can probably imagine that the AccountService has lots of methods exposed.  We would end up putting the service behind a .NET interface so that we can provide a fake implementation in our UNIT tests.  

But, there's a simpler way.  

```fsharp
// Lib
let sayHello name = sprintf "Hello %s!" name

// Composition
let sayHelloToCurrentUser = AccountService.getUsername >> sayHello

// UNIT Test
let [<Fact>] ``should say Hello to the name``() =
    "Joel"
    |> sayHello
    |> equal "Hello Joel!"
```

Now, we are simply isolating the integration point to a module devoted to composition.  

Now, we just have a simple function in our `Lib`.  It doesn't require any stubbing or mocking.  Very nice indeed!
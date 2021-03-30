# Functional Dependency Injection 

If you've been developing for at least a year or two with an object oriented language, then you probably have come across the idea of *Dependency Injection*.  *Dependency Injection* is useful to isolate dependencies and provide loose coupling of components.  

Typically, in C# you define interfaces that provide the required dependencies and constructors (and properties) that require those dependencies.  Then, you would implement an IoC container that does the work to construct the actual dependencies at runtime.  

I remember struggling with this idea for a few years.  I understood the mechanics of what I needed to do to get the system to work with dependency injection, but I didn't really understand *why* needed to do them.  

I then began to break apart the phrase.  What qualifies as a "dependency"?  What does it mean to inject them?  What's the smallest possible interface and dependency?  

Later, through my study of F#, I came across the idea that a function is the *smallest* possible interface.  It defines a name, some input, and some output.  That's it.  

Aren't all parameters dependencies?  In one sense, yes, but in context to dependency injection, no.  The focus on dependency injection is on separating the construction of objects from the behavior.  Functions that require parameters may not created by the client directly.  

But, to leverage dependency injection with F#, you can setup your functions with partial application such as:

```fsharp
let fetchUserByName (request: HttpRequestMessage -> Task<HttpResponseMessage>) (name: string) : User option = 
    ...
```

At first glance, this may appear like an ordinary function with two expected params.  But, on closer inspection, you can see that a critical dependency is required *first*:  `request: HttpRequest -> HttpResponse`.  This function will likely make an actual web request and has a pretty hefty, disposable object, `HttpClient`, backing it.  

The function, `fetchUserByName`, doesn't care about *how* the http request is made. The client of `fetchUserByName` also should care about how to make an http request.  The client should have a call such as:

```fsharp
let userOption = fetchUserByName "John"
```

## Composition

In functional programming, you will typically need a "composition root" near the entry point to your application.  This composition root serves to intantiate your services and functions with all the required dependencies.  

A simple implementation of instantiating `fetchUserByName` would look like this:

```fsharp
let request = 
    fun req -> 
        let client = new HttpClient()
        client.SendAsync(req)

let fetchUserByName` = fetchUserByName request
```



## Object Life Cycle Management

> Properly inject disposable dependencies



Like SqlConnection and HttpClient

## Phase IV - Stub It Don't Mock It
**Stubing** is the process of providing a legitimate implemenation instead of a *false*, mocked implementation.  If the function you are testing needs to talk to the outside world, then inject the dependency through a callback function:
```fsharp
// System under test
let thingToTest (dependency: input -> output) ... 
// Test code
let [<Fact>] ``thingToTest should do this.``() =
```


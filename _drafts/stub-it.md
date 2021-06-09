


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


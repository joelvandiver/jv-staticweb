open System

(* TODO: Future state with Computation Expression(s)
    smoke {
        let! results = BIOS.``Unsupported production setting``.get(1)
        let! results2 = BIOS.``Unsupported production setting 2``.get(1)
        expect (results.Length > 0)
        return "some/url"
    }        
*)


module Smoke = 


    type SmokeResult = 
        {   Errors : string list
            Url    : string
            }


    type SmokeBuilder() =
        let messages : string ResizeArray = new ResizeArray<string>()
        let addMessage p = 
            printfn "Message Added: %s" p
            messages.Add(p)


        member this.Bind(m, f) = 
            addMessage m
            {   Errors = messages |> Seq.toList
                Url    = ""
                }


        member this.Yield(x) = 
            printfn "Yield an unwrapped %A as a list" x
            {   Errors = messages |> Seq.toList
                Url    = ""
                }
                
        member this.For(m,f) =
            printfn "For %A" m
            this.Bind(m,f)
        
        member this.Return(url) =
            printfn "Returning Url:  %s" url
            {   Errors = messages |> Seq.toList
                Url    = url
                }


        [<CustomOperation("expect")>]
        member this.Expect(a, message) = 
            printfn "Expect: %A" message
            message


        [<CustomOperation("url")>]
        member this.Url(a, message) = 
            printfn "Expect: %A" message
            message


    let smoke = new SmokeBuilder()


    let test = 
        smoke {
            let x = 
                // arrange
                ()
            expect "Some Expectation 1"
            expect "Some Expectation 2"
            url "api/some/path"
        }



module Attempt = 


    type Attempt<'T> = (unit -> 'T option)


    let succeed x = (fun () -> Some x) : Attempt<'T>
    let fail = (fun () -> None) : Attempt<'T>
    let runAttempt (a: Attempt<'T>) = a()
    let bind p rest = match runAttempt p with None -> fail | Some r -> (rest r)
    let delay f = (fun () -> runAttempt (f()))
    let combine p1 p2 = (fun () -> match p1() with None -> p2() | res -> res)


    type AttemptBuilder() =
        /// Used to de-sugar uses of 'let!' inside ocmputation expressions
        member b.Bind (p, rest) = bind p rest


        /// Delays the construction of an attempt until just before it is executed
        member b.Delay(f) = delay f


        /// Used to de-sugar uses of 'return' inside computation expressions.
        member b.Return(x) = succeed x
    
        /// Used to de-sugar uses of 'return!' inside computation expressions.
        member b.ReturnFrom(x : Attempt<'T>) = x


        /// Used to de-sugar uses of 'c1: c2' inside computation expressions.
        member b.Combine(p1 : Attempt<'T>, p2 : Attempt<'T>) = combine p1 p2


        /// Used to de-sugar uses of 'if .. then ..' inside computation expressions when 
        /// the 'else' branch is empty
        member b.Zero() = fail


    let attempt = new AttemptBuilder()


    let failIfBig n = attempt { if n > 1000 then return! fail else return n }
    let result = attempt {  let! n1 = failIfBig 25
                            let! n2 = failIfBig 2000
                            let sum = n1 + n2
                            return sum }


module Expecto = 
    
    /// Test case computation expression builder
    type TestCaseBuilder() =
      member __.TryFinally(f, compensation) =
        try
          f()
        finally
          compensation()
      member __.TryWith(f, catchHandler) =
        try
          f()
        with e -> catchHandler e
      member __.Using(disposable: #IDisposable, f) = using disposable f
      member __.For(sequence, f) =
        for i in sequence do f i
      member __.While(gd, prog) =
        while gd() do prog()
      member __.Combine(f1, f2) = f2(); f1
      member __.Zero() = ()
      member __.Delay f = f
      member __.Run f = f()


    let test = TestCaseBuilder()


    let example = test {    ()
                            }


module Scenario = 


    type Scenario<'T> =
    /// *Hard*-Fail scenarios can be used for *critical* data such as PRODUCTION platforms.
    | HardFail of 'T option
    /// *Soft*-Fail:  The test will pass but a log will be reported stating that it could not find the scenario.
    | SoftFail of 'T option
    | Success of 'T


    type ScenarioRun<'T> = (unit -> Scenario<'T>)
    let succeed x = (fun () -> Success x) : ScenarioRun<'T>
    let fail = (fun () -> HardFail None) : ScenarioRun<'T>
    let runScenario (a: ScenarioRun<'T>) = a()
    let bind p rest = 
        match runScenario p with 
        | HardFail _ -> fail 
        | SoftFail _ -> fail 
        | Success r -> (rest r)
    let delay f = (fun () -> runScenario (f()))
    let combine p1 p2 = 
        (fun () -> 
            match p1() with 
            | HardFail _ -> p2() 
            | SoftFail _ -> p2() 
            | res -> res)


    type ScenarioBuilder() =
        /// Used to de-sugar uses of 'let!' inside ocmputation expressions
        member b.Bind (p, rest) = bind p rest


        /// Delays the construction of an attempt until just before it is executed
        member b.Delay(f) = delay f


        /// Used to de-sugar uses of 'return' inside computation expressions.
        member b.Return(x) = succeed x
    
        /// Used to de-sugar uses of 'return!' inside computation expressions.
        member b.ReturnFrom(x) = x


        /// Used to de-sugar uses of 'c1: c2' inside computation expressions.
        member b.Combine(p1 : ScenarioRun<'T>, p2 : ScenarioRun<'T>) = combine p1 p2


        /// Used to de-sugar uses of 'if .. then ..' inside computation expressions when 
        /// the 'else' branch is empty
        member b.Zero() = fail


    let scenario = ScenarioBuilder()


    let example = scenario {    let! x = fun _ -> Success [Guid.Empty]
                                let! y = fun _ -> Success [Guid.Empty, Guid.Empty]
                                let result = (fun () -> Success x) : ScenarioRun<Guid list>
                                return! result
                                }



module Expect = 
    type Event = string
    type Expectation = (unit -> Event option)


    let succeed x = (fun () -> Some x) : Expectation
    let fail = (fun () -> None) : Expectation
    let runExpectation (a: Expectation) = a()
    let bind p rest = match runExpectation p with None -> fail | Some r -> (rest r)
    let delay f = (fun () -> runExpectation (f()))
    let combine p1 p2 = (fun () -> match p1() with None -> p2() | res -> res)


    type ExpectationBuilder() =
        /// Used to de-sugar uses of 'let!' inside ocmputation expressions
        member b.Bind (p, rest) = bind p rest


        /// Delays the construction of an attempt until just before it is executed
        member b.Delay(f) = delay f


        /// Used to de-sugar uses of 'return' inside computation expressions.
        member b.Return(x) = succeed x
    
        /// Used to de-sugar uses of 'return!' inside computation expressions.
        member b.ReturnFrom(x : Expectation) = x


        /// Used to de-sugar uses of 'c1: c2' inside computation expressions.
        member b.Combine(p1 : Expectation, p2 : Expectation) = combine p1 p2


        /// Used to de-sugar uses of 'if .. then ..' inside computation expressions when 
        /// the 'else' branch is empty
        member b.Zero() = fail


    let smoke = ExpectationBuilder()


    // let result = smoke {    let! n1 = fun _ -> Some "Expectation 1"
    //                         let! n2 = fun _ -> Some "Expectation 2"
    //                         return [n1; n2] }


---
title: Handling Error State in F# #
date: "2020-02-19"
description: "How I've handled error state in F#"
---

The more I've developed software systems, the less I have come across truly *exceptional* situations.  Early in my career most situations were new to me and hence seemed exceptional.  Raising *exceptions* in code is equivalent to throwing up your hands and saying "There's nothing I can do in this situation."  

To improve the quality of code, we have to get away from the mentality of raising exceptions when we don't know what to do.  Figure out what you should do and identify the truly exceptional situations.  Design your functions so that they are transparent and return the complete error state.  Let the caller deal with the problems.  I have heard others call this "Railway Oriented Programming".

Let's take a simple scenario involving validing a `User` record.  

```fsharp
open System

type User = 
    { firstname : string
      lastname  : string
      username  : string }

let isEmpty = String.IsNullOrWhiteSpace

// Fail out the first failure.
// ***Note: The error state is not transparent to the caller.  
let validateUserWithExceptions (user: User) : unit = 
    if isEmpty user.firstname then failwith "The first name is required."
    if isEmpty user.lastname then failwith "The last name is required."
    if isEmpty user.username then failwith "The username is required."

// Return an error result on first failure.
// ***Note: The error state is returned to the caller, but only one at a time.
let validateUserResult (user: User) : Result<User, string> =
    if isEmpty user.firstname then Error "The first name is required."
    elif isEmpty user.lastname then Error "The last name is required."
    elif isEmpty user.username then Error "The username is required."
    else Ok user

// Validate all rules in one step and return all errors.
// ***Note: The error state is aggregated and returned to the caller.
let validateUserResultList (user: User) : Result<User, string list> =
    [
        // Return optional errors per rule.
        if isEmpty user.firstname then Some "The first name is required." else None
        if isEmpty user.lastname then Some "The last name is required." else None
        if isEmpty user.username then Some "The username is required." else None
    ]
    // Filter out the `None` error cases.
    |> List.filter Option.isSome
    // Extract the value of the remaining options.
    |> List.map Option.get
    |> function 
        // Return Ok for no errors.
        | [] -> Ok user
        // Return Error result with the list of errors.
        | errors -> Error errors
```
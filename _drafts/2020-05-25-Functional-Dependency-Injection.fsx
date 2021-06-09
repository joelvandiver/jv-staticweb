open System.Net
open System.Net.Http

let x = 
    fun req -> 
        let client = new HttpClient()
        client.SendAsync(req)
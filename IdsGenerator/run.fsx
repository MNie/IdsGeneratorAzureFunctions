open System
open System.IO
open System.Net
open System.Net.Http.Headers
open System.Collections.Generic
open Newtonsoft.Json

let generateIds input =
    let dictionary = new Dictionary<int, int>()
    JsonConvert.DeserializeObject<List<int>> input
    |> Seq.iter (fun x ->
        let rec hasKey key =
            if(dictionary.ContainsKey(key)) then
                hasKey(key+1)
            else dictionary.[key] <- x
        hasKey((new System.Random(x)).Next())
    )
    dictionary
	
let createResponse data =
    let response = new HttpResponseMessage()
    response.Content <- new StringContent(data)
    response.StatusCode <- HttpStatusCode.OK
    response.Content.Headers.ContentType <- MediaTypeHeaderValue("application/json")
    response
	
let Run (req: HttpRequestMessage) =
    req.GetQueryNameValuePairs()
    |> Seq.find(fun x -> x.Key.ToLowerInvariant() = "data")
    |> fun x -> x.Value
    |> generateIds
    |> JsonConvert.SerializeObject
    |> createResponse

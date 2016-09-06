#r "Newtonsoft.Json.dll"
open System
open System.IO
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
	
let Run (input: string, log: TraceWriter) =  
    let data = generateIds input
    let serializedData = JsonConvert.SerializeObject data
    log.Info(sprintf "F# Queue trigger function processed: '%s'" serializedData)
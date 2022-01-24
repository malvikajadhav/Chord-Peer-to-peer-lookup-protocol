// Fetching libraries from nuget
#r "nuget: Akka, 1.4.25"
#r "nuget: Akka.FSharp, 1.4.25"


// Inittializing Libraries
open Akka
open Akka.Actor
open Akka.FSharp
open System
open System.Diagnostics
open System.Security.Cryptography
open System.Text


// Creating needed custom variables with dicriminated union
type Node = string * IActorRef
type Info = 
| Arguments of (int * int)
| ChordRing of (string * Node)
| Get of (string)
| FindSuccessor of (string * Node)
| Update of (string * Node)
| ClosestPrecedingNode of (string*Node)
| Stabilize
| FixFingers
| Notify of (Node)
| VerifySuccessor of (string * Node)
| FindFinger of (int*string * Node)
| ChangeFinger of (string*int*Node)
| AddKey of (string)
| AppendKey of (string)
| InitiateKeys
| InitiateLookup
| Lookup
| FindKey of (int*string* Node)
| UpdateHops of (int)
| CheckKey of (int*string*Node)
| PrintFunc of (Node)
| LastPrint

/ Actor Configuration
let system = System.create "chord" <| Configuration.load()

// Stores the number of max finger the finger table can have (also signifies the max hops for lookup)
let mutable M = 0

// Initialize the largest possible hash by SHA1 as string
let largestPossibleHash = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"
let lowestPossiblehash = "0000000000000000000000000000000000000000"

// Intilizing nodes joined to the ring currently
let mutable totalChordNodes = 0
let mutable numNodes = 0
let mutable numRequests = 0
let mutable totalHops = 0
let mutable z = 0 // To keep printing once

// create a random function to find the SHA1 Hash Hex
let sha1Hash (id : string) : string =
    let hashAlgo = new SHA1Managed()
    id |> Encoding.ASCII.GetBytes |> hashAlgo.ComputeHash |> System.Convert.ToHexString


// Function to calculate the next hash for finger table
let GetNextHash (nodeHash : string) (k : float) : string= 
    let mutable id = nodeHash
    if nodeHash = largestPossibleHash then
        id <- lowestPossiblehash
    let d = int (Math.Floor(k / 4.0))
    let m = Convert.ToInt64("FFFFFFFF", 16)
    if d < 8 then
        let increment = int64 (2.0 ** k)
        let last = Convert.ToInt64(id.[32..39], 16)
        if (last + increment) > m then
            id.[0..31] + "FFFFFFFF"
        else
            id.[0..31] + (last+ increment).ToString("X")
    else if d >= 8 && d < 16 then
        let increment = int64 (2.0 ** (k - 32.0))
        let last = Convert.ToInt64(id.[24..31], 16)
        if (last + increment) > m then
            id.[0..23] + "FFFFFFFF" + id.[32..40]
        else
            id.[0..23] + (last+ increment).ToString("X") + id.[32..40]
    else if d >= 16 && d < 24 then
        let increment = int64 (2.0 ** (k - 64.0))
        let last = Convert.ToInt64(id.[16..23], 16)
        if (last + increment) > m then
            id.[0..15] + "FFFFFFFF" + id.[24..40]
        else
            id.[0..15] + (last+ increment).ToString("X") + id.[24..40]
    else if d >= 24 && d < 32 then
        let increment = int64 (2.0 ** (k - 96.0))
        let last = Convert.ToInt64(id.[8..15], 16)
        if (last + increment) > m then
            id.[0..7] + "FFFFFFFF" + id.[16..40]
        else
            id.[0..7] + (last+ increment).ToString("X") + id.[16..40]
    else if d >= 32 && d < 160 then
        let increment = int64 (2.0 ** (k - 128.0))
        let last = Convert.ToInt64(id.[0..7], 16)
        if (last + increment) > m then
            largestPossibleHash.[0..31] + ((m - 1L).ToString("X"))
        else
            (last+ increment).ToString("X") + id.[8..40]
    else
        largestPossibleHash.[0..31] + ((m - 1L).ToString("X"))


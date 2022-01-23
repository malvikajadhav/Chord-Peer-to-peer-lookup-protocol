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

// Actor Configuration
let system = System.create "chord" <| Configuration.load()

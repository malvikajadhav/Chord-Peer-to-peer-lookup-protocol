# Chord: Peer-to-peer lookup protocol

<b>Course</b>: COP5615 - Distributed Operating System Principles <br>
<b>Institute</b>: Unviersity of Florida <br>
<b>Semester</b>: Fall 2021 <br>
<b>Instructor</b>: Dr. Alin Dobra <br>
<b>Team</b>: 
* Prateek Kumar Goel ([Github](https://github.com/pkgprateek))
* Malvika Ranjitsinh Jadhav ([Github](https://github.com/malvikajadhav))

________________________________________________________________________________________________________________________
### Problem Definition: 
The goal of this project is to implement the Chord protocol using actor model and a simple object access service to prove its usefulness. This implementation is derived from the Chord protocol details proposed in the paperChord: A Scalable Peer-to-peer Lookup Service for Internet Applicationsby  Ion  Stoica,  Robert  Morris,  David  Karger,  M.  Frans  Kaashoek,  Hari  Balakrishnan. (https://pdos.csail.mit.edu/papers/ton:chord/paper-ton.pdf).<br>
________________________________________________________________________________________________________________________
<b>Steps for execution: </b><br>
1. Extract the zip file - project3.zip
2. In system terminal run: <br>
```
  dotnet fsi chord.fsx <numNodes> <numRequests>
```

**Parameters:**<br>
<u>numNodes : </u> enter the number of participanting peers.<br>
<u>numRequests : </u> enter the number of lookups each peer is to perform.<br>

### What is working: 
We have successfully implemented the network join and routing as described in the Chord paper (Section 4) using actor model. Every peer in the network is represented by an actor and every actor is associated with a string identifier calculated using **sha1Hash** function of peer's name. <br>
The program is able to create a chord ring joining one node at a time starting with zero nodes and is able to incorporate the number of nodes specified using <i>numNodes</i> argument. The program performs <i>numRequests</i> lookup requests for each node and keeps track of hops for each peer. The final output is the average number of hops that have to be traversed to complete a request.<br>

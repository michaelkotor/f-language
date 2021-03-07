// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.Diagnostics
                    
type test = {
    ProcessId: string;
    ProcessName:string } override this.ToString() = sprintf "%s;%s\n" this.ProcessId this.ProcessName
let exportProcessesToCsv =
    let array = Process.GetProcesses()
    let table = [for x in 0 ..(array.Length - 1) ->
        if(x = 0) then
        {ProcessId = "ProcessId"; ProcessName = "ProcessName"} else
            {ProcessId = sprintf "%i" array.[x].Id; ProcessName = array.[x].ProcessName}]
    
    let wr = new System.IO.StreamWriter("Csv.csv")
    table |> List.map(string) |> String.concat("") |> wr.Write
    wr.Close()
let printTopProcesses number =
    let array = Process.GetProcesses()
    let arrayClean = [for i in 0..(array.Length - 1) ->
        try
            array.[i].TotalProcessorTime.Seconds
        with
            | _ -> 0]
    
    let sortedList = List.sort arrayClean
    let reverseLists = sortedList |> List.rev

    let result = [for i in 0..number -> (i, reverseLists.[i])]
    for (i, item) in result do
        printf "%i, %i\n" i item

let stopProcessByName name = 
    let list = Array.toList(Process.GetProcessesByName(name))
    if(list.Length = 0) then
        printf "No processes with this name"
        exit 0
    elif(list.Length > 1) then
        printf "Too many processes"
        exit 0
     
    printf "Would you like to kill the process? y/n\n"
    
    let input = Console.ReadLine()
    printf "%s" input
    if(input = "y") then
        list.[0].Kill()
    
    0
[<EntryPoint>]
let main argv =
    stopProcessByName "Notepad.exe" |> ignore
    0 
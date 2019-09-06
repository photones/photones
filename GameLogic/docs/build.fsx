open System.Text.RegularExpressions
open System.IO

// Given a typical setup (with 'FSharp.Formatting' referenced using NuGet),
// the following will include binaries and load the literate script
#load "../../packages/FSharp.Formatting/FSharp.Formatting.fsx"
open FSharp.Literate
open FSharp.Formatting.Razor

// ----------------------------------------------------------------------------
// SETUP
// ----------------------------------------------------------------------------

/// Return path relative to the current file location
let source = __SOURCE_DIRECTORY__
let outputdir = Path.Combine(source, "..", "..", "docs")
let stylesdir = Path.Combine(source, "styles")
let contentdir = Path.Combine(outputdir, "content")
printfn "Output dir: %s" outputdir

// Create output directories & copy content files there
// (We have two sets of samples in "output" and "output-all" directories,
//  for simplicitly, this just creates them & copies content there)
if not (Directory.Exists(outputdir)) then
  Directory.CreateDirectory outputdir |> ignore
if not (Directory.Exists(contentdir)) then
  Directory.CreateDirectory contentdir |> ignore

for fileInfo in DirectoryInfo(stylesdir).EnumerateFiles() do
    printfn "copying %s" fileInfo.FullName
    fileInfo.CopyTo(Path.Combine(contentdir, fileInfo.Name), true) |> ignore

let read (file:string) =
    seq { use reader = new StreamReader(file)
          while not reader.EndOfStream do yield reader.ReadLine() }

let (|Regex|_|) pattern input =
    let m = Regex.Match(input, pattern)
    if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ])
    else None

let sourceFiles =
    seq {
        for line in (read (Path.Combine(source, "..", "GameLogic.fsproj"))) do
            match line with
            | Regex @"<Compile Include=""(?!AssemblyInfo)((.*)([.]fs))""" [ filename; prefix; suffix ] -> yield filename, prefix, suffix
            | _ -> ()
    }

// ----------------------------------------------------------------------------
// GENERATE DOCUMENTS
// ----------------------------------------------------------------------------

let template = Path.Combine(source, "templates", "template-project.html")

let navHtml = sourceFiles |> Seq.map (fun (_, p, _) -> sprintf @"<li><a href=""%s"">%s</a></li>" (p + ".html") p) |> String.concat ""

let projectInfo =
  [ "page-description", "F# Game Programming"
    "page-author", "Chiel ten Brinke"
    "github-link", "https://github.com/Mattias1/photones"
    "project-name", "Photones"
    "nav-items", navHtml]

let generateIndexHtml () =
    let md = Path.Combine(source, "index.md")
    let outputfile = Path.Combine(outputdir, "index.html")
    RazorLiterate.ProcessMarkdown(md, template, outputfile, replacements = projectInfo)

let generateSourceFileDocs () =
    for (f, p, _) in sourceFiles do
        let path = Path.Combine(source, "..", f)
        let outputfile = Path.Combine(outputdir, sprintf "%s.html" p)
        RazorLiterate.ProcessScriptFile(path, template, outputfile, replacements = projectInfo)

generateIndexHtml ()
generateSourceFileDocs ()

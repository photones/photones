// Given a typical setup (with 'FSharp.Formatting' referenced using NuGet),
// the following will include binaries and load the literate script
#load "../../packages/FSharp.Formatting/FSharp.Formatting.fsx"
open System.IO
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
printfn "%s" source
printfn "%s" outputdir
printfn "%s" contentdir

// Create output directories & copy content files there
// (We have two sets of samples in "output" and "output-all" directories,
//  for simplicitly, this just creates them & copies content there)
if not (Directory.Exists(outputdir)) then
  Directory.CreateDirectory outputdir |> ignore
if not (Directory.Exists(contentdir)) then
  Directory.CreateDirectory contentdir |> ignore

let indexFile = Path.Combine(source, "index.html")
File.Copy(indexFile, Path.Combine(outputdir, "index.html"), true)
for fileInfo in DirectoryInfo(stylesdir).EnumerateFiles() do
    printfn "copying %s" fileInfo.FullName
    fileInfo.CopyTo(Path.Combine(contentdir, fileInfo.Name), true) |> ignore

// ----------------------------------------------------------------------------
// GENERATE DOCUMENTS
// ----------------------------------------------------------------------------

let template = Path.Combine(source, "templates", "template-project.html")

let projectInfo =
  [ "page-description", "F# Game Programming"
    "page-author", "Chiel ten Brinke"
    "github-link", "https://github.com/Mattias1/photones"
    "project-name", "F# Game Programming" ]

let generateIndexHtml () =
    let md = Path.Combine(source, "index.md")
    let outputfile = Path.Combine(outputdir, "index.html")
    RazorLiterate.ProcessMarkdown(md, template, outputfile, replacements = projectInfo)

let generateGameObjectsDocs () =
    let script = Path.Combine(source, "..", "GameObjects.fs")
    let outputfile = Path.Combine(outputdir, "GameObjects.html")
    RazorLiterate.ProcessScriptFile(script, template, outputfile, replacements = projectInfo)

generateIndexHtml ()
generateGameObjectsDocs ()

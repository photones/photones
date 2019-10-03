open System.Text.RegularExpressions
open System.IO

// Given a typical setup (with 'FSharp.Formatting' referenced using Paket),
// the following will include binaries and load the literate script
#load "../packages/FSharp.Formatting/FSharp.Formatting.fsx"
open FSharp.Literate
open FSharp.Formatting.Razor
#load "Utils.fs"
open GameLogic.Utils

// ----------------------------------------------------------------------------
// SETUP
// ----------------------------------------------------------------------------

/// Return path relative to the current file location
let source = __SOURCE_DIRECTORY__
let solutiondir = Path.Combine(source, "..")
let outputdir = Path.Combine(solutiondir, "docs")
let stylesdir = Path.Combine(source, "docs", "styles")
let contentdir = Path.Combine(outputdir, "content")
printfn "Output dir: %s" outputdir

// Create output directories & copy content files there
// (We have two sets of samples in "output" and "output-all" directories,
//  for simplicitly, this just creates them & copies content there)
let deleteDir dir =
    if Directory.Exists(dir) then
        for f in Directory.EnumerateFiles(dir) do
            File.Delete f
        for d in Directory.EnumerateDirectories(dir) do
            Directory.Delete (d, true)

deleteDir outputdir
deleteDir contentdir
Directory.CreateDirectory outputdir |> ignore
Directory.CreateDirectory contentdir |> ignore

for fileInfo in DirectoryInfo(stylesdir).EnumerateFiles() do
    printfn "copying %s" fileInfo.FullName
    fileInfo.CopyTo(Path.Combine(contentdir, fileInfo.Name), true) |> ignore

let read (file:string) =
    seq { use reader = new StreamReader(file)
          while not reader.EndOfStream do yield reader.ReadLine() }

let sourceFiles =
    seq {
        let projects = ["GameLogic"; "GameLogic.Test"]
        for project in projects do
            let projectFile = Path.Combine(solutiondir, project, project + ".fsproj");
            for line in read projectFile do
                match line with
                | Regex @"<(Compile|None) Include=""docs[\\]((.*)([.]fsx?))"""
                    [ _; filename; prefix; suffix ] ->
                    let path = Path.Combine(solutiondir, project, "docs", filename)
                    yield path, filename, prefix, suffix
                | _ -> ()
    }

// ----------------------------------------------------------------------------
// GENERATE DOCUMENTS
// ----------------------------------------------------------------------------

let template = Path.Combine(source, "docs", "templates", "template-project.html")

let documentLink (_, _, p, _) = sprintf @"<li><a href=""%s"">%s</a></li>" (p + ".html") p
let navHtml = sourceFiles |> Seq.map documentLink |> String.concat ""

let projectInfo =
  [ "page-description", "F# Game Programming"
    "page-author", "Chiel ten Brinke"
    "github-link", "https://github.com/photones/photones"
    "project-name", "Photones"
    "nav-items", navHtml]

let generateIndexHtml () =
    let md = Path.Combine(source, "docs", "index.md")
    let outputfile = Path.Combine(outputdir, "index.html")
    RazorLiterate.ProcessMarkdown(md, template, outputfile, replacements = projectInfo)

let generateSourceFileDocs () =
    for (path, _, p, _) in sourceFiles do
        let outputfile = Path.Combine(outputdir, sprintf "%s.html" p)
        RazorLiterate.ProcessScriptFile(path, template, outputfile,
            replacements = projectInfo, lineNumbers = false)

generateIndexHtml ()
generateSourceFileDocs ()

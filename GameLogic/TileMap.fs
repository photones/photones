namespace GameLogic

open Bearded.Utilities.SpaceTime
open System.Collections.Generic

type public Tile() =
    let mutable content : List<GameObject> = new List<GameObject>()

    member this.Clear() =
        content.Clear()

    member this.Add(value : GameObject) =
        content.Add(value)

    member this.Get() =
        content :> seq<GameObject>


type public TileMap(originX : Unit, originY : Unit, width : Unit, height : Unit, rows : int, columns : int) =
    let tiles : array<array<Tile>> = [| for _ in 1..rows -> [| for _ in 1..columns -> new Tile() |] |]
    let tileWidth = width / (single columns)
    let tileHeight = height / (single rows)
    let y2Row (y:Unit) = (y - originY) / height * (single rows) |> int
    let x2Column (x:Unit) = (x - originX) / width * (single columns) |> int
    let tileForPosition (pos : Position2) = tiles.[y2Row pos.Y].[x2Column pos.X]

    let isPointWithinRadius (origin:Position2) (radius:Unit) (point:Position2) = true

    member public this.Update (objects : seq<GameObject>) =
        for row in tiles do
            for tile in row do
                tile.Clear()

        for obj in objects do
            (tileForPosition obj.Position).Add(obj)

    member public this.GetNeighbors (from : GameObject) (radius : Unit) =
        let firstColumn = from.Position.X - radius |> x2Column
        let lastColumn = from.Position.X + radius |> x2Column
        let firstRow = from.Position.Y - radius |> y2Row
        let lastRow = from.Position.Y + radius |> y2Row
        seq {
        for col = firstColumn to lastColumn do
            for row = firstRow to lastRow do
                // TODO: We could add another filter here by checking on the point in the tile that is closest to `from`
                for candidate in tiles.[row].[col].Get() do 
                    if isPointWithinRadius from.Position radius candidate.Position then yield candidate else ()
        }


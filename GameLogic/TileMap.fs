namespace GameLogic

open Bearded.Utilities.SpaceTime
open System.Collections.Generic

(**
The worldspace is split up into rectangular tiles. A Tile contains zero or
more game objects. This way we can quickly look up which gameobjects are
within proximity of a certain point, as long as the tiles are not too big
and crowded.
*)

type public Tile<'GameState>() =
    let mutable content:List<GameObject<'GameState>> = List<GameObject<'GameState>>()

    member this.Clear() =
        content.Clear()

    member this.Add(value:GameObject<'GameState>) =
        content.Add(value)

    member this.Get() =
        content :> seq<GameObject<'GameState>>


type public TileMap<'GameState>
        (originX:Unit, originY:Unit, width:Unit, height:Unit, rows:int, columns:int) =
    let tiles:array<array<Tile<'GameState>>> =
        [| for _ in 1..rows -> [| for _ in 1..columns -> Tile() |] |]
    let y2Row (y:Unit) = (y - originY) / height * (single rows) |> int
    let x2Column (x:Unit) = (x - originX) / width * (single columns) |> int
    let tileForPosition (pos:Position2) =
        let row = y2Row pos.Y
        let col = x2Column pos.X
        if (0 <= row && row < rows && 0 <= col && col < columns) then
            Some tiles.[row].[col]
        else
            None

    let isPointWithinRadius (origin:Position2) (radius:Unit) (point:Position2) =
        let d = origin - point
        Squared.op_LessThan(d.LengthSquared, radius.Squared)

    let capBetween (lower:int) (upper:int) (value:int):int = min upper (max lower value)

    member public this.Update (objects: seq<GameObject<'GameState>>) =
        for row in tiles do
            for tile in row do
                tile.Clear()

        for o in objects do
            match tileForPosition o.Position with
            | Some tile -> tile.Add(o)
            | None ->
                let msg = sprintf "Object outside tilemap at %s" (o.Position.ToString())
                Utils.Tracer.Log(msg)

    member public this.IsOnTileMap (pos:Position2) =
        tileForPosition pos <> None

    member public this.GetObjects (from:Position2) (radius:Unit) =
        let columnBound = capBetween 0 (columns - 1)
        let rowBound = capBetween 0 (rows - 1)
        let firstColumn = from.X - radius |> x2Column |> columnBound
        let lastColumn = from.X + radius |> x2Column |> columnBound
        let firstRow = from.Y - radius |> y2Row |> rowBound
        let lastRow = from.Y + radius |> y2Row |> rowBound
        seq {
        // FIXME: randomize order of tiles, and interleave results
        for col = firstColumn to lastColumn do
            for row = firstRow to lastRow do
                for candidate in tiles.[row].[col].Get() do 
                    if isPointWithinRadius from radius candidate.Position then yield candidate
        }


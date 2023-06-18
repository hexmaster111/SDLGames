namespace VectorTd.Tiles;

public static class TileLoader
{
    const int MapSize = 15;

    public static (string readErr, Tile[,]?) LoadVMap(string path)
    {
        //Read the first 15 lines of the file
        var lines = File.ReadAllLines(path).Take(MapSize).ToArray();
        if (lines.Length != MapSize) return ("Map file is not the correct size", null);

        var tilesFromFile = new List<Tile>();

        var y = 0;
        var x = 0;

        foreach (var line in lines)
        {
            foreach (var ch in line)
            {
                tilesFromFile.Add(ch switch
                {
                    '#' => TileFactory.Create(TileType.Void, x, y),
                    'P' => TileFactory.Create(TileType.Path, x, y),
                    'T' => TileFactory.Create(TileType.Tower, x, y),
                    'S' => TileFactory.Create(TileType.Start, x, y),
                    'E' => TileFactory.Create(TileType.End, x, y),
                    _ => throw new ArgumentOutOfRangeException(nameof(ch), ch, null)
                });
                x++;
            }

            x = 0;
            y++;
        }

        if (tilesFromFile.Count != 225) return ($"Map size mismatch Got {tilesFromFile.Count}, expected 225", null);
        var tiles = new Tile[State.MapSize, State.MapSize];
        foreach (var tile in tilesFromFile) tiles[tile.X, tile.Y] = tile;
        return (string.Empty, tiles);
    }
}
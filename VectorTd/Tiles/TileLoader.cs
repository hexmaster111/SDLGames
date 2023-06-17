namespace VectorTd.Tiles;

public static class TileLoader
{
    public static (string readErr, Tile[,]?) LoadVMap(string path)
    {
        var lines = File.ReadAllLines(path);
        var tilesFromFile = new List<Tile>();

        int y = 0;
        int x = 0;

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
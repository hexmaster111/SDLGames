using System.Text;
using VectorTd.Creeps;

namespace VectorTd.Tiles;

public static class TileLoader
{
    const int MapSize = 15;

    public static (string? readErr, Tile[,]? tiles, MapWaveData? waves) LoadVMap(string path)
    {
        var lines = File.ReadAllLines(path).Take(MapSize).ToArray();
        if (lines.Length != MapSize) return ("Map file is not the correct size", null, null);
        var tilesFromFile = LoadTiles(lines).ToList();
        if (tilesFromFile.Count != State.MapSize * State.MapSize)
            return ($"Map size mismatch Got {tilesFromFile.Count}, expected 225", null, null);
        var tiles = new Tile[State.MapSize, State.MapSize];
        foreach (var tile in tilesFromFile) tiles[tile.X, tile.Y] = tile;

        var waveLines = File.ReadAllLines(path).Skip(MapSize).ToArray();
        var waveInstructions = LoadWaveInstructions(waveLines);
        if (waveInstructions.err != null) return (waveInstructions.err, null, null);
        return (null, tiles, waveInstructions.map);
    }

    private static (MapWaveData? map, string? err) LoadWaveInstructions(string[] waveLines)
    {
        try
        {
            return (MapWaveData.Load(waveLines), null);
        }
        catch (Exception e)
        {
            return (null, e.Message);
        }
    }

    private static IEnumerable<Tile> LoadTiles(string[] lines)
    {
        var y = 0;
        var x = 0;

        foreach (var line in lines)
        {
            foreach (var ch in line)
            {
                yield return ch switch
                {
                    '#' => TileFactory.Create(TileType.Void, x, y),
                    'P' => TileFactory.Create(TileType.Path, x, y),
                    'T' => TileFactory.Create(TileType.Tower, x, y),
                    'S' => TileFactory.Create(TileType.Start, x, y),
                    'E' => TileFactory.Create(TileType.End, x, y),
                    '>' => TileFactory.Create(TileType.Path, x, y, Direction.Right),
                    '<' => TileFactory.Create(TileType.Path, x, y, Direction.Left),
                    '^' => TileFactory.Create(TileType.Path, x, y, Direction.Up),
                    'V' => TileFactory.Create(TileType.Path, x, y, Direction.Down),
                    _ => throw new ArgumentOutOfRangeException(nameof(ch), ch, null)
                };
                x++;
            }

            x = 0;
            y++;
        }
    }
}
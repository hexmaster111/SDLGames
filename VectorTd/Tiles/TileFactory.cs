namespace VectorTd.Tiles;

public abstract class TileFactory
{
    public static Tile Create(TileType type, int x, int y) => type switch
    {
        TileType.Void => new VoidTile(x, y),
        TileType.Path => new PathTile(x, y),
        TileType.Tower => new TowerTile(x, y),
        TileType.Start => new StartTile(x, y),
        TileType.End => new EndTile(x, y),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };

    public static Tile Create(TileType path, int x, int y, Direction right)
    {
        var tile = Create(path, x, y);
        tile.Direction = right;
        return tile;
    }
}
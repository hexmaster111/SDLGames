using SDLApplication;

namespace VectorTd.Tiles;

public class PathTile : Tile
{
    public PathTile(int x, int y) : base(x, y, SdlColors.DarkGray, SdlColors.Black, TileType.Path)
    {
    }

    public override bool IsWalkable { get; } = true;
    public override bool IsBuildable { get; } = false;
    public override bool IsStart { get; } = false;
    public override bool IsEnd { get; } = false;
}
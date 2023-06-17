using SDLApplication;

namespace VectorTd.Tiles;

public class EndTile : Tile
{
    public EndTile(int x, int y) : base(x, y, SdlColors.DarkRed, SdlColors.Salmon, TileType.End)
    {
    }

    public override bool IsWalkable { get; } = false;
    public override bool IsBuildable { get; } = false;
    public override bool IsStart { get; } = false;
    public override bool IsEnd { get; } = true;
}
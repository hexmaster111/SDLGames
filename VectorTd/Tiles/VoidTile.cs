using SDLApplication;

namespace VectorTd.Tiles;

public class VoidTile : Tile
{
    public VoidTile(int x, int y) : base(x, y, SdlColors.Black, SdlColors.Black, TileType.Void)
    {
    }

    public override bool IsWalkable { get; } = false;
    public override bool IsBuildable { get; } = false;
    public override bool IsStart { get; } = false;
    public override bool IsEnd { get; } = false;
}
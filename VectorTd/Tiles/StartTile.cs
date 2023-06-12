using SDLApplication;

namespace VectorTd.Tiles;

public class StartTile : Tile
{
    public StartTile(int x, int y) : base(x, y, SdlColors.DarkGreen, SdlColors.Salmon, TileType.Start)
    {
    }

    public override bool IsWalkable { get; } = true;
    public override bool IsBuildable { get; } = false;
    public override bool IsStart { get; } = true;
    public override bool IsEnd { get; } = false;
}
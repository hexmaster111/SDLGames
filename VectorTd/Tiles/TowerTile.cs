using SDLApplication;

namespace VectorTd.Tiles;

public class TowerTile : Tile
{
    public TowerTile(int x, int y) : base(x, y, SdlColors.Black, SdlColors.DarkGray, TileType.Tower)
    {
    }

    public override bool IsWalkable { get; } = false;
    public override bool IsStart { get; } = false;
    public override bool IsEnd { get; } = false;

    //TODO: members for the tower
    public override bool IsBuildable { get; } = true;
}
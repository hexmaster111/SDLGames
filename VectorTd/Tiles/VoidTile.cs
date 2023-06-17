using SDLApplication;

namespace VectorTd.Tiles;

public class VoidTile : Tile
{
    public VoidTile(int x, int y) : base(x, y, SdlColors.Black, SdlColors.Black, TileType.Void)
    {
    }


}
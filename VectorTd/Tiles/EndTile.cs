using SDLApplication;

namespace VectorTd.Tiles;

public class EndTile : Tile
{
    public EndTile(int x, int y) : base(x, y, SdlColors.DarkRed, SdlColors.Salmon, TileType.End)
    {
    }


}
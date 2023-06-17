using SDLApplication;

namespace VectorTd.Tiles;

public class StartTile : Tile
{
    public StartTile(int x, int y) : base(x, y, SdlColors.DarkGreen, SdlColors.Salmon, TileType.Start)
    {
    }

}
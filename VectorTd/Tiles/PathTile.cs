using SDLApplication;

namespace VectorTd.Tiles;

public class PathTile : Tile
{
    public PathTile(int x, int y) : base(x, y, SdlColors.DarkGray, SdlColors.Black, TileType.Path)
    {
    }


}
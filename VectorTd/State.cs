using SDL2;
using SDLApplication;
using VectorTd.Tiles;

namespace VectorTd;

public class State
{
    public const int MapSize = 15;
    public  Tile[,] Map = new Tile[MapSize, MapSize];


    public State()
    {
        for (var x = 0; x < MapSize; x++)
        for (var y = 0; y < MapSize; y++)
            Map[x, y] = new VoidTile(x, y);
    }


    public void Render(RenderArgs args, ref SDL.SDL_Rect viewPort)
    {
        foreach (var tile in Map) tile.Render(args, ref viewPort);
    }

}
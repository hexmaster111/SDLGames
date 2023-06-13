using SDL2;
using SDLApplication;
using VectorTd.Tiles;

namespace VectorTd.Towers;

public static class TowerFactory
{
    public enum TowerType
    {
        Basic,
        Simple
    }

    public static Tower? CreateTower(int x, int y, TowerType type) => type switch
    {
        TowerType.Basic => new BasicTower(x, y),
        TowerType.Simple => new SimpleTower(x, y),
        _ => null
    };
}

public abstract class Tower
{
    public int SizePx { get; }
    public SDL.SDL_Color Color { get; set; }
    public TowerFactory.TowerType Type { get; set; }

    public int X { get; internal set; }

    public int Y { get; internal set; }

    public Tower(int x, int y, SDL.SDL_Color color, int sizePx, TowerFactory.TowerType type)
    {
        Type = type;
        X = x;
        Y = y;
        Color = color;
        SizePx = sizePx;
    }

    public void Render(RenderArgs args, ref SDL.SDL_Rect viewPort)
    {
        //render a rectangle
        SDL.SDL_RenderSetViewport(args.RendererPtr, ref viewPort);
        args.SetDrawColor(Color);
        var rect = new SDL.SDL_Rect
        {
            x = X * Tile.SizePx + Tile.SizePx / 2 - SizePx / 2,
            y = Y * Tile.SizePx + Tile.SizePx / 2 - SizePx / 2,
            w = SizePx,
            h = SizePx
        };
        args.FillRect(rect);
    }

    public virtual void Update(TimeSpan deltaTime, State state)
    {
    }

    public static Tower[] Towers { get; } =
    {
        new BasicTower(0, 0),
        new SimpleTower(0, 0)
    };

    public abstract string Name { get; }
    public abstract int Cost { get; }
}
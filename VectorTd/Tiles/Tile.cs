using SDLApplication;
using static SDL2.SDL;

namespace VectorTd.Tiles;

public abstract class Tile
{
    public abstract bool IsWalkable { get; }
    public abstract bool IsBuildable { get; }
    public abstract bool IsStart { get; }
    public abstract bool IsEnd { get; }

    internal const int SizePx = 32;
    protected int ScreenXpx;
    protected int ScreenYpx;
    internal TileType Type { get; set; }

    private int _x;
    private int _y;

    public int X
    {
        get => _x;
        set
        {
            _x = value;
            ScreenXpx = _x * SizePx;
        }
    }

    public int Y
    {
        get => _y;
        set
        {
            _y = value;
            ScreenYpx = _y * SizePx;
        }
    }

    public SDL_Color Color { get; set; }
    public SDL_Color BackGround { get; set; }

    public Tile(int x, int y, SDL_Color color, SDL_Color backGround, TileType type)
    {
        X = x;
        Y = y;
        Color = color;
        BackGround = backGround;
        Type = type;
    }

    public virtual void Render(RenderArgs args, ref SDL_Rect viewport)
    {
        SDL_RenderSetViewport(args.RendererPtr, ref viewport);
        args.SetDrawColor(BackGround);
        var rect = new SDL_Rect
        {
            x = ScreenXpx,
            y = ScreenYpx,
            w = SizePx,
            h = SizePx
        };
        args.FillRect(rect);
        args.SetDrawColor(Color);
        args.DrawRect(rect);
    }

    public virtual void Update(TimeSpan deltaTime, State state)
    {
    }
}
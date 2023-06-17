using SDLApplication;
using VectorTd.Towers;
using static SDL2.SDL;

namespace VectorTd.Tiles;

public class TowerTile : Tile
{
    public TowerTile(int x, int y) : base(x, y, SdlColors.Black, SdlColors.DarkGray, TileType.Tower)
    {
    }



    private Tower? _tower;

    public Tower? Tower
    {
        get => _tower;
        set
        {
            _tower = value;
            if (_tower != null)
            {
                _tower.X = X;
                _tower.Y = Y;
            }
        }
    }

    public override void Render(RenderArgs args, ref SDL_Rect viewport)
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
        if (Tower != null) Tower.Render(args, ref viewport);
    }

    public override void Update(TimeSpan deltaTime, State state)
    {
        Tower?.Update(deltaTime, state);
        base.Update(deltaTime, state);
    }
}
using SDL2;
using SDLApplication;
using VectorTd.Tiles;
using VectorTd.Ui;

namespace VectorTd;

public class Game
{
    public const int GridScreenWidth = Tile.SizePx * State.MapSize + 1;
    public const int GridScreenHeight = Tile.SizePx * State.MapSize + 1;

    public const int ScreenWidth = GridScreenWidth + InGameUi.Width;
    public const int ScreenHeight = GridScreenHeight;

    private State _state = new State();
    private InGameUi _inGameUi = new InGameUi();

    public Game()
    {
        var map = TileLoader.LoadVMap(@"C:\Users\Hexma\Desktop\Maps\Void.vmap");
        if (!string.IsNullOrEmpty(map.readErr)) throw new Exception(map.readErr);
        if (map.Item2 == null) throw new Exception("Map is null");
        _state.Map = map.Item2;
        new SdlApp(EventHandler, RenderHandler, ScreenWidth, ScreenHeight).Run();
    }


    private void EventHandler(SDL.SDL_Event e)
    {
    }

    private void RenderHandler(RenderArgs args)
    {
        //Create two viewports, one for the grid and one for the sidebar 
        var gridViewport = new SDL.SDL_Rect
        {
            x = InGameUi.Width,
            y = 0,
            w = GridScreenWidth,
            h = GridScreenHeight
        };

        var sidebarViewport = new SDL.SDL_Rect
        {
            x = 0,
            y = 0,
            w = InGameUi.Width,
            h = InGameUi.Height
        };

        _state.Render(args, ref gridViewport);
        _inGameUi.Render(args, ref _state, ref sidebarViewport);
    }
}
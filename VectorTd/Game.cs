using SDLApplication;
using VectorTd.Creeps;
using VectorTd.Tiles;
using VectorTd.Ui;
using static SDL2.SDL;


namespace VectorTd;

public class Game
{
    public const int GridScreenWidth = Tile.SizePx * State.MapSize + 1;
    public const int GridScreenHeight = Tile.SizePx * State.MapSize + 1;

    public const int ScreenWidth = GridScreenWidth + InGameUi.Width;
    public const int ScreenHeight = GridScreenHeight;

    public GlobalState GlobalState = new();
    private State _state = new State(GridViewport);
    private readonly InGameUi _inGameUi = new(SidebarViewport);

    //Create two viewports, one for the grid and one for the sidebar 
    public static SDL_Rect GridViewport { get; } = new()
    {
        x = InGameUi.Width,
        y = 0,
        w = GridScreenWidth,
        h = GridScreenHeight
    };

    public static SDL_Rect SidebarViewport { get; } = new()
    {
        x = 0,
        y = 0,
        w = InGameUi.Width,
        h = InGameUi.Height
    };

    public Game()
    {
        var map = TileLoader.LoadVMap(@"C:\Users\Hexma\Desktop\Maps\Void.vmap");
        // var map = TileLoader.LoadVMap(@"/home/hailey/code/SDLGames/VectorTd/Maps/Basic.vmap");
        if (!string.IsNullOrEmpty(map.readErr)) throw new Exception(map.readErr);
        if (map.Item2 == null) throw new Exception("Map is null");
        _state.Map = map.Item2;
        new SdlApp(EventHandler, RenderHandler, UpdateHandler, ScreenWidth, ScreenHeight).Run();
    }

    private void UpdateHandler(TimeSpan deltaTime)
    {
        _state.Update(deltaTime);
    }


    private void EventHandler(SDL_Event e)
    {
        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch (e.type)
        {
            case SDL_EventType.SDL_MOUSEMOTION:
                GlobalState.MouseX = e.motion.x;
                GlobalState.MouseY = e.motion.y;
                break;
            case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                var x = e.button.x;
                var y = e.button.y;
                if (x < InGameUi.Width) _inGameUi.Click(x, y, ref _state);
                else _state.Click(x, y);

                break;
            case SDL_EventType.SDL_MOUSEBUTTONUP:
                break;
            case SDL_EventType.SDL_MOUSEWHEEL:
                break;

            case SDL_EventType.SDL_KEYDOWN:
                var key = e.key.keysym.sym;
                switch (key)
                {
                    case SDL_Keycode.SDLK_SPACE:
                        _state.AddCreep(new BasicCreep());
                        break;
                }

                break;
        }
    }

    private void RenderHandler(RenderArgs args)
    {
        _state.Render(args);
        _inGameUi.Render(args, ref _state);
        GlobalState.Render(args);
    }
}
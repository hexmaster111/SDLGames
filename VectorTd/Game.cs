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
    public readonly SdlApp App;

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

    public Game(string mapPath)
    {
        var map = TileLoader.LoadVMap(mapPath);
        if (!string.IsNullOrEmpty(map.readErr)) throw new Exception(map.readErr);
        if (map.tiles == null) throw new Exception("Map is null");
        _state.Map = map.tiles;
        _state.SetWave(map.waves);
        App = new SdlApp(EventHandler, RenderHandler, UpdateHandler, ScreenWidth, ScreenHeight);
    }

    private void UpdateHandler(TimeSpan deltaTime, long _)
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
                        _state.AddCreep(new SmallCreep());
                        break;
                    case SDL_Keycode.SDLK_w:
                        _state.WaveController.StartWave();
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
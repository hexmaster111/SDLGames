using System.Numerics;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SDLApplication;
using static SDL2.SDL;

namespace MapGenTest;

internal class Game
{
    public float GameScaleWidth = 1.0f;
    public float GameScaleHeight = 1.0f;

    public const int WorldHeight = 25;
    public const int WorldWidth = 50;
    public List<TileObject> TileObjects = new();
    private readonly TileObject _player;
    private readonly GameAssetFactory _assetFactoryFactory;

    public State State = new()
    {
        KeyboardInputFocus = State.KeyboardInputLocation.Game
    };


    public Game()
    {
        var idealHeight = WorldHeight * Sprite.GridSpriteImageSize;
        var idealWidth = WorldWidth * Sprite.GridSpriteImageSize;

        GameScaleHeight = 1.0f / ((float)idealHeight / Program.App.ScreenHeight);
        GameScaleWidth = 1.0f / ((float)idealWidth / Program.App.ScreenWidth);


        _assetFactoryFactory = new GameAssetFactory(this);
        _assetFactoryFactory.LoadTextures(Program.App.RendererPtr);

        _player = new TileObject
        {
            Sprite = _assetFactoryFactory.NewSprite(GameAssetType.Player),
        };
        TileObjects.Add(_player);
    }


    public void Render(RenderArgs args)
    {
        foreach (var tileObject in TileObjects)
        {
            var screenXy = ToScreenPoint(tileObject.Point);
            tileObject.Sprite.Render(screenXy, args);
        }
    }

    private static T Map<T>(T x, T inMin, T inMax, T outMin, T outMax) where T : INumber<T> =>
        (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;

    private static SDL_Point ToScreenPoint(SDL_Point gridPos) => new()
    {
        x = Map(gridPos.x, 0, WorldWidth, 0, Program.App.ScreenWidth),
        y = Map(gridPos.y, 0, WorldHeight, 0, Program.App.ScreenHeight)
    };

    public void Event(SDL_Event e)
    {
        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault -- we dont care about a lot of them
        switch (e.type)
        {
            case SDL_EventType.SDL_KEYDOWN:
            case SDL_EventType.SDL_KEYUP:
            case SDL_EventType.SDL_MOUSEMOTION:
            case SDL_EventType.SDL_MOUSEBUTTONDOWN:
            case SDL_EventType.SDL_MOUSEBUTTONUP:
            case SDL_EventType.SDL_MOUSEWHEEL:
                InputEvent(e);
                break;
        }
    }

    private void InputEvent(SDL_Event e)
    {
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (e.type)
        {
            case SDL_EventType.SDL_KEYDOWN:
            case SDL_EventType.SDL_KEYUP:
                KeyEvent(e);
                break;


            case SDL_EventType.SDL_MOUSEMOTION:
                break;
            case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                break;
            case SDL_EventType.SDL_MOUSEBUTTONUP:
                break;
            case SDL_EventType.SDL_MOUSEWHEEL:
                break;

            default: throw new ArgumentOutOfRangeException();
        }
    }

    private void KeyEvent(SDL_Event e)
    {
        if (e.type != SDL_EventType.SDL_KEYUP && e.type != SDL_EventType.SDL_KEYDOWN)
        {
            throw new ArgumentOutOfRangeException();
        }

        switch (State.KeyboardInputFocus)
        {
            case State.KeyboardInputLocation.Game:
                GameKeyEvent(e);
                break;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    private void GameKeyEvent(SDL_Event e)
    {
        var key = e.key.keysym;
        if (e.key.state == 1)
        {
            if (key.sym == SDL_Keycode.SDLK_LEFT) _player.Move(Direction.E, 1);
            if (key.sym == SDL_Keycode.SDLK_RIGHT) _player.Move(Direction.W, 1);
            if (key.sym == SDL_Keycode.SDLK_UP) _player.Move(Direction.N, 1);
            if (key.sym == SDL_Keycode.SDLK_DOWN) _player.Move(Direction.S, 1);
        }
    }
}
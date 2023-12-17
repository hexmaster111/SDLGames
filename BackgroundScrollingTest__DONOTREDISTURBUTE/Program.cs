using SDLApplication;
using static SDL2.SDL;

namespace BackgroundScrollingTest__DONOTREDISTURBUTE;

internal static class Textures
{
    internal static readonly TextureWrapper DotTexture;
    internal static readonly TextureWrapper PotTexture;

    static Textures()
    {
        DotTexture = new("dot.bmp");
        PotTexture = new TextureWrapper("CONTAINOR_POT.png");
    }
}

internal static class Program
{
    internal static SdlApp App;

    internal const int TargetFps = 20;

    internal const int ScreenWidthPx = 800;
    internal const int ScreenHeightPx = 600;

    internal const int TileSizePx = 32;

    internal const int LevelWidth = 50;
    internal const int LevelHeight = 50;

    internal const int LevelWidthPx = TileSizePx * LevelWidth;
    internal const int LevelHeightPx = TileSizePx * LevelHeight;

    private static SDL_Rect _camera;

    private static readonly List<GameGridSprite> _sprites = new();

    private static GameGridSprite _focusedSprite;

    public static void Main(string[] args)
    {
        App = new SdlApp(EventHandler, RenderHandler, UpdateHandler,
            targetFps: 20,
            width: ScreenWidthPx,
            height: ScreenHeightPx,
            targetUpdatesPerSec: TargetFps);


        _camera = new SDL_Rect { x = 0, y = 0, w = ScreenWidthPx, h = ScreenHeightPx };
        Dot _dot = new(Textures.DotTexture);
        Pot _pot = new(Textures.PotTexture);
        _pot.PosXPx = 30;
        _pot.PosYPx = 30;

        _focusedSprite = _dot;
        
        _sprites.Add(_dot);
        _sprites.Add(_pot);
        
        App.Run();
    }

    private static void UpdateHandler(TimeSpan _, long now)
    {
    }

    private static void RenderHandler(RenderArgs args)
    {

        _camera.x = _focusedSprite.PosXPx + Dot.WidthPx / 2 - ScreenWidthPx / 2;
        _camera.y = _focusedSprite.PosYPx + Dot.HeightPx / 2 - ScreenHeightPx / 2;


        //Keep the camera in bounds
        if (_camera.x < 0) _camera.x = 0;
        if (_camera.y < 0) _camera.y = 0;
        if (_camera.x > LevelWidthPx - _camera.w) _camera.x = LevelWidthPx - _camera.w;
        if (_camera.y > LevelHeightPx - _camera.h) _camera.y = LevelHeightPx - _camera.h;


        foreach (var sprite in _sprites)
        {
            sprite.Render(_camera.x, _camera.y);
        }
    }

    private static void EventHandler(SDL_Event e)
    {
        Console.WriteLine($"X: {_camera.x:0000}, y: {_camera.y:0000}");
    }
}
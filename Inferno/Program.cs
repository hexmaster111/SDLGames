// See https://aka.ms/new-console-template for more information

using Inferno.GameSprites;
using SDLApplication;
using static SDL2.SDL;

namespace Inferno;

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

    private static readonly List<IGameSprite> _sprites = new();

    private static IGameSprite _focusedSprite;


    public static void Main(string[] args)
    {
        App = new SdlApp(EventHandler, RenderHandler, UpdateHandler,
            targetFps: 20,
            width: ScreenWidthPx,
            height: ScreenHeightPx,
            targetUpdatesPerSec: TargetFps);


        _camera = new SDL_Rect { x = 0, y = 0, w = ScreenWidthPx, h = ScreenHeightPx };
        Player player = new("d505");
        Pot pot = new();
        Torch torch = new();

        player.GridPosX = 10;
        player.GridPosY = 10;

        pot.GridPosX = 15;
        pot.GridPosY = 15;

        torch.GridPosX = 5;
        torch.GridPosY = 5;
        

        _focusedSprite = player;

        _sprites.Add(player);
        _sprites.Add(pot);
        _sprites.Add(torch);

        App.Run();
    }

    private static void UpdateHandler(TimeSpan _, long now)
    {
        foreach (var sprite in _sprites)
        {
            sprite.Update(now);
        }
    }

    private static void RenderHandler(RenderArgs args)
    {
        _camera.x = _focusedSprite.PosXPx + TileSizePx / 2 - ScreenWidthPx / 2;
        _camera.y = _focusedSprite.PosYPx + TileSizePx / 2 - ScreenHeightPx / 2;


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
        if (e.type == SDL_EventType.SDL_KEYDOWN)
        {
            switch (e.key.keysym.sym)
            {
                case SDL_Keycode.SDLK_UP:
                    _focusedSprite.GridPosY -= 1;
                    break;
                case SDL_Keycode.SDLK_DOWN:
                    _focusedSprite.GridPosY += 1;
                    break;
                case SDL_Keycode.SDLK_LEFT:
                    _focusedSprite.GridPosX -= 1;
                    break;
                case SDL_Keycode.SDLK_RIGHT:
                    _focusedSprite.GridPosX += 1;
                    break;
            }
        }
    }
}
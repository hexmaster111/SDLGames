// See https://aka.ms/new-console-template for more information

using Inferno.GameSprites;
using SDLApplication;
using TinyGui;
using TinyGui.UiElements;
using static SDL2.SDL;

namespace Inferno;

internal static class Program
{
    internal static SdlApp App;

    internal const int TargetFps = 20;

    internal const int ScreenWidthPx = 1000;
    internal const int ScreenHeightPx = 800;

    internal const int TileSizePx = 32;

    internal const int LevelWidth = 50;
    internal const int LevelHeight = 50;

    internal const int LevelWidthPx = TileSizePx * LevelWidth;
    internal const int LevelHeightPx = TileSizePx * LevelHeight;

    private static SDL_Rect _camera;

    private static GameObjectCollection _sprites = new();

    private static IGameObject _focusedObject;


    public static void Main(string[] args)
    {
        App = new SdlApp(EventHandler, RenderHandler, UpdateHandler,
            targetFps: TargetFps,
            width: ScreenWidthPx,
            height: ScreenHeightPx,
            targetUpdatesPerSec: TargetFps);


        _camera = new SDL_Rect { x = 0, y = 0, w = ScreenWidthPx, h = ScreenHeightPx };

        Player player = new("D505")
        {
            GridPosX = 10,
            GridPosY = 10
        };

        AddDemoItems();

        _sprites.Add(player);
        _focusedObject = player;

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
        _camera.x = _focusedObject.PosXPx + TileSizePx / 2 - ScreenWidthPx / 2;
        _camera.y = _focusedObject.PosYPx + TileSizePx / 2 - ScreenHeightPx / 2;


        //Keep the camera in bounds
        if (_camera.x < 0) _camera.x = 0;
        if (_camera.y < 0) _camera.y = 0;
        if (_camera.x > LevelWidthPx - _camera.w) _camera.x = LevelWidthPx - _camera.w;
        if (_camera.y > LevelHeightPx - _camera.h) _camera.y = LevelHeightPx - _camera.h;


        foreach (var sprite in _sprites)
        {
            sprite.Render(_camera.x, _camera.y);
        }


        var sp = new StackPanel()
        {
            Children =
            {
                new TextElement($"@ x: {_focusedObject.GridPosX} y:{_focusedObject.GridPosY}"),
                new TextElement($"CAM x: {_camera.x} y:{_camera.y} w:{_camera.w} h:{_camera.h}")
            }
        };

        sp.Measure();
        sp.X = 10;
        sp.Y = ScreenHeightPx - sp.Height - 10;

        sp.Render();
    }

    private static void EventHandler(SDL_Event e)
    {
        if (e.type == SDL_EventType.SDL_KEYDOWN)
        {
            switch (e.key.keysym.sym)
            {
                case SDL_Keycode.SDLK_UP:
                    _focusedObject.GridPosY -= 1;
                    break;
                case SDL_Keycode.SDLK_DOWN:
                    _focusedObject.GridPosY += 1;
                    break;
                case SDL_Keycode.SDLK_LEFT:
                    _focusedObject.GridPosX -= 1;
                    break;
                case SDL_Keycode.SDLK_RIGHT:
                    _focusedObject.GridPosX += 1;
                    break;
            }
        }
    }


    public static void AddDemoItems()
    {
        Pot pot = new()
        {
            GridPosX = 15,
            GridPosY = 15
        };

        Torch torch = new()
        {
            GridPosX = 5,
            GridPosY = 5
        };

        Slime slime = new()
        {
            GridPosX = 3,
            GridPosY = 3
        };

        Zombie zombie = new()
        {
            GridPosX = 4,
            GridPosY = 3
        };

        ContainerChestWood chest = new()
        {
            GridPosX = 5,
            GridPosY = 6
        };


        _sprites.Add(pot);
        _sprites.Add(torch);
        _sprites.Add(slime);
        _sprites.Add(zombie);
        _sprites.Add(chest);

        for (int i = 0; i < 5; i++)
        {
            _sprites.Add(new WallStone()
            {
                GridPosX = 20 + i,
                GridPosY = 20
            });
        }


        _sprites.Add(new WallStoneDoor()
        {
            GridPosX = 20,
            GridPosY = 21
        });
    }
}
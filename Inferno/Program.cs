// See https://aka.ms/new-console-template for more information

using Inferno.GameSprites;
using Inferno.GameSprites.Items;
using SDLApplication;
using TinyGui;
using TinyGui.UiElements;
using static SDL2.SDL;

namespace Inferno;

internal static class State
{
    public static UiFocusE ActiveFocus = UiFocusE.Game;

    public enum UiFocusE
    {
        Game,
        Inventory,
        Grab,
        LookBox,
        OpenMenu,
        CloseMenu
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

    private static GameObjectCollection _sprites = new();

    public static IGameObject FocusedObject { get; set; }
    internal static Player Player;
    private static InventoryHandler _invHandler;
    private static ItemPickupHandler _itemPickupHandler;
    private static LookBoxHandler _lookBoxHandler;
    private static ItemOpenCloseMenuHandler _itemOpenCloseMenuHandler;
    private static ItemOpenCloseMenuHandler _itemCloseMenuHandler;

    public static void AddWorldSprite(IGameObject sprite, int mapX, int mapY)
    {
        sprite.GridPosX = mapX;
        sprite.GridPosY = mapY;
        _sprites.Add(sprite);
    }

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
        FocusedObject = player;
        Player = player;
        AddDemoItems();
        _invHandler = new InventoryHandler(player)
        {
            Player = player
        };

        _itemPickupHandler = new ItemPickupHandler();
        var lb = new LookBox()
        {
            Hide = true
        };
        _lookBoxHandler = new LookBoxHandler(lb);
        _itemOpenCloseMenuHandler = new ItemOpenCloseMenuHandler();
        _itemCloseMenuHandler = new ItemOpenCloseMenuHandler();

        _sprites.Add(player);
        _sprites.Add(lb);

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
        switch (State.ActiveFocus)
        {
            case State.UiFocusE.Game:
                RenderGame(args);
                break;
            case State.UiFocusE.Inventory:
                _invHandler.Render(args);
                break;
            case State.UiFocusE.Grab:
                RenderGame(args);
                _itemPickupHandler.Render(args);
                break;
            case State.UiFocusE.LookBox:
                RenderGame(args);
                _lookBoxHandler.Render(args);
                break;
            case State.UiFocusE.OpenMenu:
                RenderGame(args);
                _itemOpenCloseMenuHandler.Render(args);
                break;
            case State.UiFocusE.CloseMenu:
                RenderGame(args);
                _itemCloseMenuHandler.Render(args);
                break;


            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static void EventHandler(SDL_Event e)
    {
        switch (State.ActiveFocus)
        {
            case State.UiFocusE.Game:
                GameEventHandler(e);
                break;
            case State.UiFocusE.Inventory:
                _invHandler.HandleEvent(e);
                break;
            case State.UiFocusE.Grab:
                _itemPickupHandler.HandleEvent(e);
                break;
            case State.UiFocusE.LookBox:
                _lookBoxHandler.HandleEvent(e);
                break;
            case State.UiFocusE.OpenMenu:
                _itemOpenCloseMenuHandler.HandleEvent(e);
                break;
            case State.UiFocusE.CloseMenu:
                _itemCloseMenuHandler.HandleEvent(e);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static void RenderGame(RenderArgs args)
    {
        _camera.x = FocusedObject.PosXPx + TileSizePx / 2 - ScreenWidthPx / 2;
        _camera.y = FocusedObject.PosYPx + TileSizePx / 2 - ScreenHeightPx / 2;


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
                new TextElement($"@ x: {FocusedObject.GridPosX} y:{FocusedObject.GridPosY}"),
                new TextElement($"CAM x: {_camera.x} y:{_camera.y} w:{_camera.w} h:{_camera.h}"),
                new TextElement($"FPS: {args.Fps}" + $" UPS: {args.Ups}"),
                new TextElement($"SPRITES: {_sprites.Count()}"),
                new TextElement($"FOCUS: {State.ActiveFocus}"),
            }
        };

        sp.Measure();
        sp.X = 10;
        sp.Y = ScreenHeightPx - sp.Height - 10;

        sp.Render();
    }


    private static void MovePlayer(SDL_Event e)
    {
        if (e.type == SDL_EventType.SDL_KEYDOWN)
        {
            int nextX = -1, nextY = -1;
            switch (e.key.keysym.sym)
            {
                case SDL_Keycode.SDLK_UP:
                    nextX = Player.GridPosX;
                    nextY = Player.GridPosY - 1;
                    break;
                case SDL_Keycode.SDLK_DOWN:
                    nextX = Player.GridPosX;
                    nextY = Player.GridPosY + 1;
                    break;
                case SDL_Keycode.SDLK_LEFT:
                    nextX = Player.GridPosX - 1;
                    nextY = Player.GridPosY;
                    break;
                case SDL_Keycode.SDLK_RIGHT:
                    nextX = Player.GridPosX + 1;
                    nextY = Player.GridPosY;
                    break;
            }

            var n = _sprites.GetObjectsAt(nextX, nextY);
            var nextTiles = n as Item[] ?? n.ToArray();

            if (nextTiles.Any(x => x.Solidity == Solidity.Solid))
            {
                Console.WriteLine("You clumbsly bump into something");
                return;
            }

            Player.GridPosX = nextX;
            Player.GridPosY = nextY;
            // var nextTilesNames = string.Join(", ", nextTiles.Select(x => x.GetType().Name));
            // Console.WriteLine($"Player just went to tile {nextX} {nextY} and found {nextTilesNames}");
        }
    }

    private static void GameEventHandler(SDL_Event e)
    {
        if (e.type == SDL_EventType.SDL_KEYDOWN)
        {
            switch (e.key.keysym.sym)
            {
                case SDL_Keycode.SDLK_UP:
                case SDL_Keycode.SDLK_DOWN:
                case SDL_Keycode.SDLK_LEFT:
                case SDL_Keycode.SDLK_RIGHT:
                    MovePlayer(e);
                    break;


                case SDL_Keycode.SDLK_i:
                    State.ActiveFocus = State.UiFocusE.Inventory;
                    break;

                case SDL_Keycode.SDLK_g:
                    State.ActiveFocus = State.UiFocusE.Grab;
                    _itemPickupHandler.Pickup(_sprites.GetItemsAt(Player.GridPosX, Player.GridPosY));
                    break;

                case SDL_Keycode.SDLK_l:
                    State.ActiveFocus = State.UiFocusE.LookBox;
                    _lookBoxHandler.Focus(FocusedObject);
                    FocusedObject = _lookBoxHandler.Sprite;
                    break;

                case SDL_Keycode.SDLK_o:
                    State.ActiveFocus = State.UiFocusE.OpenMenu;
                    _itemOpenCloseMenuHandler.OpenMenu(_sprites.GetObjectsAround(Player.GridPosX, Player.GridPosY),
                        true);
                    break;

                case SDL_Keycode.SDLK_c:
                    State.ActiveFocus = State.UiFocusE.CloseMenu;
                    _itemCloseMenuHandler.OpenMenu(_sprites.GetObjectsAround(Player.GridPosX, Player.GridPosY), false);
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

        Stick stick = new()
        {
            GridPosX = 6,
            GridPosY = 6
        };

        Player.AddItemToInventory(new LesserHealingPotion());
        Player.AddItemToInventory(new LesserManaPotion());
        Player.AddItemToInventory(new Stick());
        Player.AddItemToInventory(new Dagger());
        Player.AddItemToInventory(new Ranch());


        _sprites.Add(pot);
        _sprites.Add(torch);
        _sprites.Add(slime);
        _sprites.Add(zombie);
        _sprites.Add(chest);
        _sprites.Add(stick);


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

        _sprites.Add(new WallStoneDoor()
        {
            GridPosX = 20,
            GridPosY = 22
        });
        _sprites.Add(new WallStoneDoor()
        {
            GridPosX = 18,
            GridPosY = 22
        });
    }

    public static void RemoveWordSprite(Item item)
    {
        _sprites.Remove(item);
    }
}

internal class ItemOpenCloseMenuHandler
{
    private IGameObject[] _itemsAround;
    private StackPanel<IGameObject> _sp;

    bool _open = false;

    public void OpenMenu(IEnumerable<IGameObject> itemsAround, bool open)
    {
        _open = open;
        var arr = itemsAround.Where(x => open ? x.CanOpen : x.CanClose).ToArray();
        if (arr.Length == 0)
        {
            State.ActiveFocus = State.UiFocusE.Game;
            return;
        }

        if (arr.Length == 1)
        {
            if (_open) arr[0].Open();
            else arr[0].Close();
            State.ActiveFocus = State.UiFocusE.Game;
            return;
        }


        _itemsAround = arr;

        _sp = new StackPanel<IGameObject>(() => arr,
            o => new TextElement(o.ObjName + " " + GetDirectionArrow(Program.Player, o)));
        _sp.X = (int)(1 / 3f * Program.ScreenWidthPx);
        _sp.Y = (int)(1 / 3f * Program.ScreenHeightPx);
        _sp.EnableSelection = true;
        _sp.UpdateChildren();
        _sp.Measure();
    }


    private string GetDirectionArrow(IGameObject player, IGameObject item) => player.GridPosX == item.GridPosX ? 
            player.GridPosY > item.GridPosY ? "↑" : "↓" :
            player.GridPosX > item.GridPosX ? "←" : "→";

    public void HandleEvent(SDL_Event e)
    {
        if (e.type == SDL_EventType.SDL_KEYDOWN)
        {
            switch (e.key.keysym.sym)
            {
                case SDL_Keycode.SDLK_ESCAPE:
                    State.ActiveFocus = State.UiFocusE.Game;
                    break;

                case SDL_Keycode.SDLK_UP:
                    _sp.SelectedIndex--;
                    break;
                case SDL_Keycode.SDLK_DOWN:
                    _sp.SelectedIndex++;
                    break;

                case SDL_Keycode.SDLK_RETURN:
                    if (_open) _itemsAround[_sp.SelectedIndex].Open();
                    else _itemsAround[_sp.SelectedIndex].Close();
                    State.ActiveFocus = State.UiFocusE.Game;
                    break;
            }
        }
    }


    public void Render(RenderArgs args)
    {
        _sp.Render();
    }
}
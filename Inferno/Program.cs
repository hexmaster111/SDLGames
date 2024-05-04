// See https://aka.ms/new-console-template for more information

using Inferno.GameSprites;
using Inferno.GameSprites.Items;
using SDLApplication;
using TinyGui;
using TinyGui.UiElements;
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

    private static GameObjectCollection _sprites = new();

    public static IGameObject FocusedObject { get; set; }
    internal static Player Player;
    private static InventoryHandler _invHandler;
    private static ItemPickupHandler _itemPickupHandler;
    private static LookBoxHandler _lookBoxHandler;
    private static ItemOpenCloseMenuHandler _itemOpenCloseMenuHandler;
    private static ItemOpenCloseMenuHandler _itemCloseMenuHandler;

    static Program()
    {
     
        App = new SdlApp(EventHandler, RenderHandler, UpdateHandler,
            targetFps: TargetFps,
            width: ScreenWidthPx,
            height: ScreenHeightPx,
            targetUpdatesPerSec: 20);


        _camera = new SDL_Rect { x = 0, y = 0, w = ScreenWidthPx, h = ScreenHeightPx };

        Player player = new("D505")
        {
            X = 10,
            Y = 10
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

    }
    public static void AddWorldSprite(IGameObject sprite, int mapX, int mapY)
    {
        sprite.X = mapX;
        sprite.Y = mapY;
        _sprites.Add(sprite);
    }

    public static void Main(string[] args) => App.Run();

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

    /// <summary>
    ///     Main Application Event Handler
    /// </summary>
    private static void EventHandler(SDL_Event e)
    {
        {
            //dev stuff
            //reload textures hotkey
            if (e.type == SDL_EventType.SDL_KEYDOWN && e.key.keysym.sym == SDL_Keycode.SDLK_KP_MINUS)
                Textures.ReloadFromDisk();
        }


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
                new TextElement($"@ x: {Player.X} y:{Player.Y}"),
                new TextElement($"GRID x: {FocusedObject.X} y:{FocusedObject.Y}"),
                new TextElement($"CAM x: {_camera.x} y:{_camera.y} w:{_camera.w} h:{_camera.h}"),
                new TextElement($"FPS: {args.Fps}" + $" UPS: {args.Ups}" + $" TB: {args.Tb}ms"),
                new TextElement($"SPRITES: {_sprites.Count()}"),
                new TextElement($"FOCUS: {State.ActiveFocus}"),
                new TextElement(
                    $"Focused item: {_sprites.GetObjectsAt(FocusedObject.X, FocusedObject.Y).FirstOrDefault()?.ObjName}"),
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
                    nextX = Player.X;
                    nextY = Player.Y - 1;
                    break;
                case SDL_Keycode.SDLK_DOWN:
                    nextX = Player.X;
                    nextY = Player.Y + 1;
                    break;
                case SDL_Keycode.SDLK_LEFT:
                    nextX = Player.X - 1;
                    nextY = Player.Y;
                    break;
                case SDL_Keycode.SDLK_RIGHT:
                    nextX = Player.X + 1;
                    nextY = Player.Y;
                    break;
            }

            var n = _sprites.GetObjectsAt(nextX, nextY);
            var nextTiles = n as Item[] ?? n.ToArray();

            if (nextTiles.Any(x => x.Solidity == Solidity.Solid))
            {
                Console.WriteLine("You clumbsly bump into something");
                return;
            }

            Player.X = nextX;
            Player.Y = nextY;
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
                    _itemPickupHandler.Pickup(_sprites.GetItemsAt(Player.X, Player.Y));
                    break;

                case SDL_Keycode.SDLK_l:
                    State.ActiveFocus = State.UiFocusE.LookBox;
                    _lookBoxHandler.Focus(FocusedObject);
                    FocusedObject = _lookBoxHandler.Sprite;
                    break;

                case SDL_Keycode.SDLK_o:
                    State.ActiveFocus = State.UiFocusE.OpenMenu;
                    _itemOpenCloseMenuHandler.OpenMenu(_sprites.GetObjectsAround(Player.X, Player.Y),
                        true);
                    break;

                case SDL_Keycode.SDLK_c:
                    State.ActiveFocus = State.UiFocusE.CloseMenu;
                    _itemCloseMenuHandler.OpenMenu(_sprites.GetObjectsAround(Player.X, Player.Y), false);
                    break;
            }
        }
    }


    public static void AddDemoItems()
    {
        Pot pot = new() { X = 15, Y = 15 };
        Torch torch = new() { X = 5, Y = 5 };
        Slime slime = new() { X = 3, Y = 3 };
        Zombie zombie = new() { X = 4, Y = 3 };
        ContainerChestWood chest = new()
        {
            X = 5, Y = 6,
            Items = new List<Item>()
            {
                new Dagger(), new Ranch(), new Stick(), new ShortSward()
            }
        };
        ContainerChestWood chest2 = new()
        {
            X = 7, Y = 6,
            Items = new List<Item>()
            {
                new LesserManaPotion(), new LesserHealingPotion()
            }
        };

        Stick stick = new() { X = 6, Y = 6 };
        PathGravel pg = new() { X = 7, Y = 3, };
        WallWoodenFence wallWoodenFence = new() { X = 8, Y = 4, };
        TrapDoor trapDoor = new() {X = 10, Y = 3};

        _sprites.Add(pot);
        _sprites.Add(torch);
        _sprites.Add(slime);
        _sprites.Add(zombie);
        _sprites.Add(chest);
        _sprites.Add(chest2);
        _sprites.Add(stick);
        _sprites.Add(pg);
        _sprites.Add(wallWoodenFence);
        _sprites.Add(trapDoor);
        Player.AddItemToInventory(new LesserHealingPotion());
        Player.AddItemToInventory(new LesserManaPotion());
        Player.AddItemToInventory(new Stick());
        Player.AddItemToInventory(new Dagger());
        Player.AddItemToInventory(new Ranch());

        for (int i = 0; i < 5; i++)
        {
            _sprites.Add(new WallStone()
            {
                X = 20 + i,
                Y = 20
            });
        }


        _sprites.Add(new WallStoneDoor() { X = 20, Y = 21 });
        _sprites.Add(new WallStoneDoor() { X = 20, Y = 22 });
        _sprites.Add(new WallStoneDoor() { X = 18, Y = 22 });
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
            if (_open) arr[0].Open(Program.Player);
            else arr[0].Close(Program.Player);
            State.ActiveFocus = State.UiFocusE.Game;
            return;
        }


        _itemsAround = arr;

        _sp = new StackPanel<IGameObject>(() => arr,
            o => new TextElement(o.ObjName + " " + GetDirectionArrow(Program.Player, o)))
        {
            X = (int)(1 / 3f * Program.ScreenWidthPx),
            Y = (int)(1 / 3f * Program.ScreenHeightPx),
            EnableSelection = true
        };
        _sp.UpdateChildren();
        _sp.Measure();
    }


    private string GetDirectionArrow(IGameObject player, IGameObject item) => player.X == item.X
        ? player.Y > item.Y ? "↑" : "↓"
        : player.X > item.X
            ? "←"
            : "→";

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
                    if (_open) _itemsAround[_sp.SelectedIndex].Open(Program.Player);
                    else _itemsAround[_sp.SelectedIndex].Close(Program.Player);
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
using System.Numerics;
using Inferno.GuiElements;
using SDLApplication;
using static SDL2.SDL;

namespace Inferno;

internal class TileObjectCollection : List<TileObject>
{
    public TileObject[] FindItemsAt(SDL_Point mapPos) =>
        this.Where(x => x.Point.x == mapPos.x && x.Point.y == mapPos.y)
            .ToArray();
}

internal class Game
{
    public static Game Inst { get; private set; }
    public static float GameScaleWidth = 1.0f;
    public static float GameScaleHeight = 1.0f;
    public static GameAssetFactory Assets { get; private set; }

    public const int WorldHeight = 15;
    public const int WorldWidth = 25;
    public TileObjectCollection TileObjects = new();

    private readonly TileObject _testDoor;
    private readonly PlayerInventoryHandler _invhdlr;

    public State State = new()
    {
        KeyboardInputFocus = State.KeyboardInputLocation.Game
    };

    internal readonly TileObject _player;


    public Game()
    {
        var idealHeight = WorldHeight * Sprite.GridSpriteImageSize;
        var idealWidth = WorldWidth * Sprite.GridSpriteImageSize;
        Inst = this;

        GameScaleHeight = 1.0f / ((float)idealHeight / Program.App.ScreenHeight);
        GameScaleWidth = 1.0f / ((float)idealWidth / Program.App.ScreenWidth);
        var assetFactory = Assets = new GameAssetFactory();
        assetFactory.LoadTextures(Program.App.RendererPtr);
        _invhdlr = new PlayerInventoryHandler();
        _player = assetFactory.NewTile(GameObjectType.Player);
        _testDoor = assetFactory.NewTile(GameObjectType.WallStoneDoor);
        _testDoor.Point = new SDL_Point() { x = 5, y = 4 };
        var fire = assetFactory.NewTile(GameObjectType.Torch);
        fire.Point = new SDL_Point() { x = 4, y = 4 };
        var slime = assetFactory.NewTile(GameObjectType.EntitySlimeLv0);
        slime.Point = new SDL_Point() { x = 3, y = 4 };
        var zombie = assetFactory.NewTile(GameObjectType.EntityZombieLv0);
        zombie.Point = new SDL_Point() { x = 3, y = 5 };
        var pot = assetFactory.NewTile(GameObjectType.ContainerPot);
        pot.Point = new SDL_Point() { x = 2, y = 4 };
        var chest = assetFactory.NewTile(GameObjectType.ContainerChestWood);
        chest.Point = new SDL_Point() { x = 2, y = 5 };
        for (int i = 0; i < 10; i++)
        {
            if (i == 2) continue;
            var wall = assetFactory.NewTile(GameObjectType.WallStone);
            wall.Point = new SDL_Point() { x = 9, y = i };
            TileObjects.Add(wall);
        }

        TileObjects.Add(fire);
        TileObjects.Add(_testDoor);
        TileObjects.Add(pot);
        TileObjects.Add(chest);
        TileObjects.Add(slime);
        TileObjects.Add(zombie);
        TileObjects.Add(_player);
        State.Player.PlayerInventory.Items.Add(new Item()
        {
            Type = GameObjectType.Stick,
            EquitablePositions = ItemEquitablePositions.Hands,
            ItemType = ItemType.Weapon
        });

        State.Player.PlayerInventory.Items.Add(new Item()
        {
            Type = GameObjectType.Dagger,
            EquitablePositions = ItemEquitablePositions.Hands,
            ItemType = ItemType.Weapon
        });

        State.Player.PlayerInventory.Items.Add(new Item()
        {
            Type = GameObjectType.ShortSward,
            EquitablePositions = ItemEquitablePositions.Hands,
            ItemType = ItemType.Weapon
        });

        State.Player.PlayerInventory.Items.Add(new Item()
        {
            Type = GameObjectType.Ranch,
            EquitablePositions = ItemEquitablePositions.NoWhere,
            ItemType = ItemType.Potion
        });

        State.Player.PlayerInventory.ArmorSlots.Hands = new Item()
        {
            Type = GameObjectType.ShortSward,
            EquitablePositions = ItemEquitablePositions.Hands,
            ItemType = ItemType.Weapon
        };
    }


    public void Update(long now)
    {
        foreach (var tileObject in TileObjects) tileObject.Update(now);
    }

    public void RenderGame(RenderArgs args)
    {
        foreach (var tileObject in TileObjects)
        {
            var screenXy = ToScreenPoint(tileObject.Point);
            tileObject.Sprite.Render(screenXy, args);
        }
    }

    public void Render(RenderArgs args)
    {
        switch (State.KeyboardInputFocus)
        {
            case State.KeyboardInputLocation.Game:
                RenderGame(args);
                break;
            case State.KeyboardInputLocation.Inventory:
                RenderInventory(args);
                break;
            case State.KeyboardInputLocation.GrabDialog:
                RenderGrabDialog(args);
                break;

            default: throw new ArgumentOutOfRangeException();
        }
    }


    private ListBox _grabDialogLb = new()
    {
        Background = SdlColors.Teal,
    };

    private TextBlock _grabDialogTb = new("Grab what?", new SDL_Point())
    {
        Visibility = Visibility.Visible
    };


    private void RenderGrabDialog(RenderArgs args)
    {
        var playerPos = _player.Point;
        var items = TileObjects.FindItemsAt(playerPos).Where(x => x.Type != GameObjectType.Player).ToArray();
        var viewBox = new SDL_Rect()
        {
            h = Program.App.ScreenHeight,
            w = Program.App.ScreenWidth,
        };

        _grabDialogLb.Items = items.Select(x => $"{x.Type}").ToList();
        _grabDialogLb.Pos = new SDL_Point()
        {
            x = viewBox.w / 2 - _grabDialogLb.MeasureSize().w / 2,
            y = viewBox.h / 2 - _grabDialogLb.MeasureSize().h / 2,
        };

        _grabDialogTb.Pos = new SDL_Point()
        {
            x = viewBox.w / 2 - _grabDialogTb.MeasureSize().w / 2,
            y = viewBox.y + viewBox.h / 2 - _grabDialogTb.MeasureSize().h / 2 - _grabDialogLb.MeasureSize().h,
        };

        RenderGame(args);
        _grabDialogTb.Render(args);
        _grabDialogLb.Render(args);
    }

    private void RenderInventory(RenderArgs args)
    {
        _invhdlr.Render(args, State.Player, new SDL_Rect()
        {
            h = Program.App.ScreenHeight,
            w = Program.App.ScreenWidth,
        });
    }

    private void InventoryKeyEvent(SDL_Event sdlEvent)
    {
        _invhdlr.KeyEvent(sdlEvent, State);
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
            case SDL_EventType.SDL_MOUSEMOTION: break;
            case SDL_EventType.SDL_MOUSEBUTTONDOWN: break;
            case SDL_EventType.SDL_MOUSEBUTTONUP: break;
            case SDL_EventType.SDL_MOUSEWHEEL: break;
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
            case State.KeyboardInputLocation.Inventory:
                InventoryKeyEvent(e);
                break;

            case State.KeyboardInputLocation.GrabDialog:
                GrabDialogKeyEvent(e);
                break;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    private void GrabDialogKeyEvent(SDL_Event sdlEvent)
    {
        if (sdlEvent.key.state == 0) return;
        switch (sdlEvent.key.keysym.sym)
        {
            case SDL_Keycode.SDLK_DOWN:
                _grabDialogLb.SelectedIndex++;
                break;
            case SDL_Keycode.SDLK_UP:
                _grabDialogLb.SelectedIndex--;
                break;
            case SDL_Keycode.SDLK_RETURN:
                var playerPos = _player.Point;
                var items = TileObjects.FindItemsAt(playerPos).Where(x => x.Type != GameObjectType.Player).ToArray();
                if (_grabDialogLb.SelectedIndex < 0 || _grabDialogLb.SelectedIndex >= items.Length) break;
                var selectedItem = items[_grabDialogLb.SelectedIndex];
                var itemTile = (ItemTileObject)selectedItem;
                State.Player.PlayerInventory.Items.Add(itemTile.Item);
                TileObjects.Remove(selectedItem);
                if (items.Length == 1) State.KeyboardInputFocus = State.KeyboardInputLocation.Game;
                else _grabDialogLb.SelectedIndex = 0;
                break;

            default:
                State.KeyboardInputFocus = State.KeyboardInputLocation.Game;
                break;
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
            if (key.sym == SDL_Keycode.SDLK_d) ((StatefulSprite)_testDoor.Sprite).SetState(1);

            //inventory
            if (key.sym == SDL_Keycode.SDLK_i)
            {
                State.KeyboardInputFocus = State.KeyboardInputLocation.Inventory;
            }

            //Grab / pickup
            if (key.sym == SDL_Keycode.SDLK_g)
            {
                var playerPos = _player.Point;
                var items = TileObjects
                    .FindItemsAt(playerPos)
                    .Where(x => x.GetType() == typeof(ItemTileObject)) // only items
                    .ToArray();
                if (items.Length > 0) State.KeyboardInputFocus = State.KeyboardInputLocation.GrabDialog;
            }
        }
    }
}
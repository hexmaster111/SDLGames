using MapGenTest.GuiElements;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SDLApplication;
using static SDL2.SDL;

namespace MapGenTest;

internal enum InventoryHandlerPart
{
    ItemsList, ItemContextMenu
}

internal class PlayerInventoryHandler
{
    private InventoryHandlerPart _activePart = InventoryHandlerPart.ItemsList;

    internal void KeyEvent(SDL_Event sdlEvent, State state)
    {
        switch (_activePart)
        {
            case InventoryHandlerPart.ItemsList:
                ItemsListKeyEvent(sdlEvent, state);
                break;
            case InventoryHandlerPart.ItemContextMenu:
                ItemContextMenuKeyEvent(sdlEvent);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ItemContextMenuKeyEvent(SDL_Event sdlEvent)
    {
        switch (sdlEvent.key.keysym.sym)
        {
            case SDL_Keycode.SDLK_ESCAPE when sdlEvent.key.state == 1:
                _activePart = InventoryHandlerPart.ItemsList;
                _itemContextMenu.Visibility = Visibility.Hidden;
                break;
            case SDL_Keycode.SDLK_DOWN when sdlEvent.key.state == 1:
                _itemContextMenu.SelectedIndex++;
                break;
            case SDL_Keycode.SDLK_UP when sdlEvent.key.state == 1:
                _itemContextMenu.SelectedIndex--;
                break;
        }
    }


    private void ItemsListKeyEvent(SDL_Event sdlEvent, State state)
    {
        switch (sdlEvent.key.keysym.sym)
        {
            case SDL_Keycode.SDLK_ESCAPE when sdlEvent.key.state == 1:
                state.KeyboardInputFocus = State.KeyboardInputLocation.Game;
                break;
            case SDL_Keycode.SDLK_DOWN when sdlEvent.key.state == 1:
                _itemLb.SelectedIndex++;
                break;
            case SDL_Keycode.SDLK_UP when sdlEvent.key.state == 1:
                _itemLb.SelectedIndex--;
                break;
            case SDL_Keycode.SDLK_RETURN when sdlEvent.key.state == 1:
                //todo: Load the context menu with valid things the player can do with the item
                // Drop, Equip, Quaff, 
                _itemContextMenu.Items.Clear();
                _itemContextMenu.Items.AddRange(new[] { "Drop", "Examine" });
                _itemContextMenu.Visibility = Visibility.Visible;
                _activePart = InventoryHandlerPart.ItemContextMenu;
                break;
        }

        Console.WriteLine(sdlEvent.key.keysym.sym);
    }

    private readonly TextBlock _invTitleTb = new("", new SDL_Point());
    private readonly TextBlock _equippedTitleTb = new("", new SDL_Point());
    private readonly ListBox _itemLb = new();
    private readonly EquippedItemBox _selectedItemBox = new();

    private readonly ContextMenu _itemContextMenu = new()
    {
        Visibility = Visibility.Hidden,
    };


    internal void Render(RenderArgs ra, Player player, SDL_Rect viewPort)
    {
        var inv = player.Inventory;
        var invTitle = $"Inventory ({inv.Items.Count}/{inv.Capacity})";
        _invTitleTb.Text = invTitle;
        _invTitleTb.Pos = new SDL_Point
        {
            x = viewPort.w / 2 - invTitle.Length * _invTitleTb.FontSizeDest / 2,
            y = viewPort.y + viewPort.h / 2
        };
        _invTitleTb.Render(ra);

        var invItemPos = new SDL_Point
        {
            x = viewPort.x,
            y = viewPort.y + _invTitleTb.Pos.y + _invTitleTb.FontSizeDest
        };

        _itemLb.Items = player.Inventory.Items.Select(x => $"{x.Modifier} {x.Type}").ToList();
        _itemLb.Pos = invItemPos;
        _equippedTitleTb.Text = "Equipped";
        _itemLb.Render(ra);
        _selectedItemBox.Item = player.Inventory.Items[_itemLb.SelectedIndex];

        _selectedItemBox.Pos = new SDL_Point
        {
            x = viewPort.x,
            y = (int)(viewPort.y + (1 / 8f * (viewPort.h / 2f)))
        };

        _itemContextMenu.Pos = invItemPos with
        {
            x = viewPort.x + (int)(5 / 8f * (viewPort.w / 2f))
        };

        _selectedItemBox.Render(ra);
        _itemContextMenu.Render(ra);
    }
}
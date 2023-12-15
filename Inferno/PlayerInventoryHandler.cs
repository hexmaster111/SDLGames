using MapGenTest.GuiElements;
using SDLApplication;
using static SDL2.SDL;

namespace MapGenTest;

internal enum InventoryHandlerPart
{
    ItemsList,
    ItemContextMenu
}

internal class GrabItemsDialogHandler
{
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
                ItemContextMenuKeyEvent(sdlEvent, state);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ItemContextMenuKeyEvent(SDL_Event sdlEvent, State state)
    {
        if (sdlEvent.key.state == 0) return;

        switch (sdlEvent.key.keysym.sym)
        {
            case SDL_Keycode.SDLK_ESCAPE:
                _activePart = InventoryHandlerPart.ItemsList;
                _selectedItemContextMenu.Visibility = Visibility.Hidden;
                break;
            case SDL_Keycode.SDLK_DOWN:
                _selectedItemContextMenu.SelectedIndex++;
                break;
            case SDL_Keycode.SDLK_UP:
                _selectedItemContextMenu.SelectedIndex--;
                break;
            case SDL_Keycode.SDLK_RETURN:
                HandleItemContextMenuSelected(_selectedItemContextMenu.SelectedItem, state);
                break;
        }
    }

    private void HandleItemContextMenuSelected(ItemContextMenuAction action, State state)
    {
        switch (action)
        {
            case ItemContextMenuAction.Drop:
                // state.Player.PlayerInventory.Items[_itemLb.SelectedIndex];
                state.Player.PlayerInventory.DropItem(_itemLb.SelectedIndex, state);
                _selectedItemContextMenu.Visibility = Visibility.Hidden;
                _selectedItemContextMenu.SelectedIndex = _itemLb.SelectedIndex - 1;
                _activePart = InventoryHandlerPart.ItemsList;
                break;
            case ItemContextMenuAction.Equip:

                break;
            case ItemContextMenuAction.Quaff:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(action), action, null);
        }
    }

    private void ItemsListKeyEvent(SDL_Event sdlEvent, State state)
    {
        if (sdlEvent.key.state == 0) return;
        switch (sdlEvent.key.keysym.sym)
        {
            case SDL_Keycode.SDLK_ESCAPE:
                state.KeyboardInputFocus = State.KeyboardInputLocation.Game;
                _itemLb.SelectedIndex = 0;
                break;
            case SDL_Keycode.SDLK_DOWN:
                _itemLb.SelectedIndex++;
                break;
            case SDL_Keycode.SDLK_UP:
                _itemLb.SelectedIndex--;
                break;
            case SDL_Keycode.SDLK_RETURN:
                //todo: Load the context menu with valid things the player can do with the item
                // Drop, Equip, Quaff, 
                _selectedItemContextMenu.Items.Clear();
                if (_itemLb.SelectedIndex >= 0 && _itemLb.SelectedIndex < state.Player.PlayerInventory.Items.Count)
                {
                    var item = state.Player.PlayerInventory.Items[_itemLb.SelectedIndex];

                    switch (item.ItemType)
                    {
                        case ItemType.Potion:
                            _selectedItemContextMenu.AddOption(ItemContextMenuAction.Quaff);
                            break;
                        case ItemType.Armor or ItemType.Weapon:
                            _selectedItemContextMenu.AddOption(ItemContextMenuAction.Equip);
                            break;
                    }

                    _selectedItemContextMenu.AddOption(ItemContextMenuAction.Drop);
                    _selectedItemContextMenu.Visibility = Visibility.Visible;
                    _activePart = InventoryHandlerPart.ItemContextMenu;
                }

                break;
        }

        Console.WriteLine(sdlEvent.key.keysym.sym);
    }

    private readonly TextBlock _invTitleTb = new("", new SDL_Point());
    private readonly TextBlock _equippedTitleTb = new("", new SDL_Point());

    private readonly ListBox _itemLb = new();

    private readonly EquippedItemBox _selectedItemBox = new();

    private readonly ContextMenu<ItemContextMenuAction> _selectedItemContextMenu = new()
    {
        Visibility = Visibility.Hidden,
        Background = SdlColors.Teal,
    };

    private enum ItemContextMenuAction
    {
        Drop,
        Equip,
        Quaff,
    }

    internal void Render(RenderArgs ra, Player player, SDL_Rect viewPort)
    {
        var inv = player.PlayerInventory;
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

        _itemLb.Items = player.PlayerInventory.Items.Select(x => $"{x.Modifier} {x.Type}").ToList();
        _itemLb.Pos = invItemPos;
        _equippedTitleTb.Text = "Equipped";
        _itemLb.Render(ra);

        if (_itemLb.SelectedIndex >= 0 && _itemLb.SelectedIndex < player.PlayerInventory.Items.Count)
        {
            _selectedItemBox.Item = player.PlayerInventory.Items[_itemLb.SelectedIndex];

            _selectedItemBox.Pos = new SDL_Point
            {
                x = viewPort.x,
                y = (int)(viewPort.y + (1 / 8f * (viewPort.h / 2f)))
            };
            _selectedItemBox.Render(ra);
        }


        _selectedItemContextMenu.Pos = invItemPos with
        {
            x = viewPort.x + (int)(5 / 8f * (viewPort.w / 2f))
        };

        _selectedItemContextMenu.Render(ra);
    }
}
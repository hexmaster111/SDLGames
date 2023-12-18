using Inferno.GameSprites;
using Inferno.GameSprites.Items;
using SDLApplication;
using TinyGui.UiElements;
using static SDL2.SDL;

namespace Inferno;

internal class InventoryHandler
{
    public Player Player { get; set; }
    private readonly StackPanel<Item> _itemListSp;
    private readonly StackPanel<InvItemMenuOptionsE> _invItemMenuSp;

    private enum InvItemMenuOptionsE
    {
        Quaff,
        Drop,
        Equip,
        Throw,
        Use,
        Cancel
    }

    public InventoryHandler(Player player)
    {
        Player = player;
        _itemListSp = new StackPanel<Item>(
            () => Player.Inventory,
            item => new Button(item.Name)
        )
        {
            EnableSelection = true,
            SelectedColor = SdlColors.Teal,
            X = 10,
            Y = (Program.ScreenHeightPx / 4) * 3,
        };
        _itemListSp.UpdateChildren();
        _itemListSp.Measure();

        _invItemMenuSp = new StackPanel<InvItemMenuOptionsE>(
            GetActionsAvailableForSelectedItem,
            option => new Button(option.ToString())
        )
        {
            EnableSelection = true,
            SelectedColor = SdlColors.DarkYellow,
            X = Program.ScreenWidthPx / 2,
            Y = (Program.ScreenHeightPx / 4) * 3,
        };

        _invItemMenuSp.UpdateChildren();
        _invItemMenuSp.Measure();
    }

    private IEnumerable<InvItemMenuOptionsE> GetActionsAvailableForSelectedItem()
    {
        var item = _itemListSp.SelectedValue;
        if (item == null) yield break;
        if (item.IsDrinkable) yield return InvItemMenuOptionsE.Quaff;
        if (item.IsEquip) yield return InvItemMenuOptionsE.Equip;
        if (item.IsThrowable) yield return InvItemMenuOptionsE.Throw;
        if (item.IsUsable) yield return InvItemMenuOptionsE.Use;

        yield return InvItemMenuOptionsE.Drop;
        yield return InvItemMenuOptionsE.Cancel;
    }


    public void Render(RenderArgs args)
    {
        _itemListSp.Render();


        if (_activeFocus == UiFocusE.ItemSelectedMenu)
        {
            _invItemMenuSp.Render();
        }
    }

    public void HandleEvent(SDL_Event e)
    {
        _itemListSp.UpdateChildren();
        _itemListSp.Measure();
        _invItemMenuSp.UpdateChildren();
        _invItemMenuSp.Measure();
        switch (_activeFocus)
        {
            case UiFocusE.ItemsList:
                HandleItemsList(e);
                break;
            case UiFocusE.ItemSelectedMenu:
                HandleItemSelectedMenu(e);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void HandleItemSelectedMenuSelected(Item item, InvItemMenuOptionsE action)
    {
        switch (action)
        {
            case InvItemMenuOptionsE.Quaff:
                item.Drink(Player);
                break;
            case InvItemMenuOptionsE.Drop:
                item.Drop(Player);
                break;
            case InvItemMenuOptionsE.Equip:
                item.Equip(Player);
                break;
            case InvItemMenuOptionsE.Cancel:
                _activeFocus = UiFocusE.ItemsList;
                break;
            case InvItemMenuOptionsE.Throw:
                item.Throw(Player);
                break;
            case InvItemMenuOptionsE.Use:
                item.Use(Player);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _activeFocus = UiFocusE.ItemsList;
    }

    private void HandleItemSelectedMenu(SDL_Event e)
    {
        switch (e.type)
        {
            case SDL_EventType.SDL_KEYDOWN:
                switch (e.key.keysym.sym)
                {
                    case SDL_Keycode.SDLK_UP:
                        _invItemMenuSp.SelectedIndex--;
                        break;
                    case SDL_Keycode.SDLK_DOWN:
                        _invItemMenuSp.SelectedIndex++;
                        break;
                    case SDL_Keycode.SDLK_ESCAPE:
                        _activeFocus = UiFocusE.ItemsList;
                        break;

                    case SDL_Keycode.SDLK_RETURN:
                        var item = Player.Inventory[_itemListSp.SelectedIndex];
                        var action = _invItemMenuSp.SelectedValue;
                        HandleItemSelectedMenuSelected(item, action);
                        break;
                }

                break;
        }
    }

    private void HandleItemsList(SDL_Event e)
    {
        switch (e.type)
        {
            case SDL_EventType.SDL_KEYDOWN:
                switch (e.key.keysym.sym)
                {
                    case SDL_Keycode.SDLK_UP:
                        _itemListSp.SelectedIndex--;
                        break;
                    case SDL_Keycode.SDLK_DOWN:
                        _itemListSp.SelectedIndex++;
                        break;
                    case SDL_Keycode.SDLK_RETURN:
                        _activeFocus = UiFocusE.ItemSelectedMenu;
                        SetItemSelectedMenuItem();
                        break;
                    case SDL_Keycode.SDLK_ESCAPE:
                        State.ActiveFocus = State.UiFocusE.Game;
                        break;
                }

                break;
        }
    }

    private void SetItemSelectedMenuItem()
    {
        var item = Player.Inventory[_itemListSp.SelectedIndex];
        _invItemMenuSp.X = _itemListSp.X + _itemListSp.Width + 10;
        _invItemMenuSp.Y = _itemListSp.Y + (_itemListSp.SelectedIndex * 16);
    }

    private UiFocusE _activeFocus = UiFocusE.ItemsList;


    private enum UiFocusE
    {
        ItemsList,
        ItemSelectedMenu
    }
}
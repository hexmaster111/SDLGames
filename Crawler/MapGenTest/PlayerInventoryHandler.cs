using MapGenTest.GuiElements;
using SDLApplication;
using static SDL2.SDL;

namespace MapGenTest;

internal class PlayerInventoryHandler
{
    internal void KeyEvent(SDL_Event sdlEvent, State state)
    {
        if (sdlEvent.key.keysym.sym == SDL_Keycode.SDLK_ESCAPE && sdlEvent.key.state == 1)
        {
            state.KeyboardInputFocus = State.KeyboardInputLocation.Game;
        }

        if (sdlEvent.key.keysym.sym == SDL_Keycode.SDLK_DOWN && sdlEvent.key.state == 1)
        {
            _lb.SelectedIndex++;
        }

        if (sdlEvent.key.keysym.sym == SDL_Keycode.SDLK_UP && sdlEvent.key.state == 1)
        {
            _lb.SelectedIndex--;
        }
    }

    private TextBlock _invTitleTb = new("", new SDL_Point());
    private TextBlock _equippedTitleTb = new("", new SDL_Point());
    private ListBox _lb = new();
    private EquippedItemBox _selectedItemBox = new();


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

        _lb.Items = player.Inventory.Items.Select(x => $"{x.Modifier} {x.Type}").ToList();
        _lb.Pos = invItemPos;
        _equippedTitleTb.Text = "Equipped";
        _lb.Render(ra);
        _selectedItemBox.Item = player.Inventory.Items[_lb.SelectedIndex];


        _selectedItemBox.Pos = new SDL_Point
        {
            x = viewPort.x,
            y = (int)(viewPort.y + (1 / 8f * (viewPort.h / 2f)))
        };


        _selectedItemBox.Render(ra);
    }
}
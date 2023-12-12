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
            lb.SelectedIndex++;
        }

        if (sdlEvent.key.keysym.sym == SDL_Keycode.SDLK_UP && sdlEvent.key.state == 1)
        {
            lb.SelectedIndex--;
        }
    }

    private TextBlock InvTitleTb = new("", new SDL_Point());
    private ListBox lb = new();


    internal void Render(RenderArgs ra, Player player, SDL_Rect viewPort)
    {
        var inv = player.Inventory;
        var invTitle = $"Inventory ({inv.Items.Count}/{inv.Capacity})";
        InvTitleTb.Text = invTitle;
        InvTitleTb.Pos = new SDL_Point()
            { x = viewPort.w / 2 - invTitle.Length * InvTitleTb.FontSizeDest / 2, y = viewPort.y };
        InvTitleTb.Render(ra);

        var invItemPos = new SDL_Point()
            { x = viewPort.x, y = viewPort.y + InvTitleTb.Pos.y + InvTitleTb.FontSizeDest };

        lb.Items = player.Inventory.Items.Select(x => $"{x.Modifier} {x.Type}").ToList();
        lb.Pos = invItemPos;

        lb.Render(ra);
    }
}
using Inferno.GameSprites;
using SDL2;
using SDLApplication;
using TinyGui.UiElements;

namespace Inferno;

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

    public void HandleEvent(SDL.SDL_Event e)
    {
        if (e.type == SDL.SDL_EventType.SDL_KEYDOWN)
        {
            switch (e.key.keysym.sym)
            {
                case SDL.SDL_Keycode.SDLK_ESCAPE:
                    State.ActiveFocus = State.UiFocusE.Game;
                    break;

                case SDL.SDL_Keycode.SDLK_UP:
                    _sp.SelectedIndex--;
                    break;
                case SDL.SDL_Keycode.SDLK_DOWN:
                    _sp.SelectedIndex++;
                    break;

                case SDL.SDL_Keycode.SDLK_RETURN:
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
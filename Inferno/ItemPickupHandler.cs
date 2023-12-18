using Inferno.GameSprites.Items;
using SDL2;
using SDLApplication;
using TinyGui.UiElements;

namespace Inferno;

internal class ItemPickupHandler
{
    private StackPanel<Item> _grabItemsSp;


    public void Render(RenderArgs args)
    {
        _grabItemsSp.UpdateChildren();
        _grabItemsSp.Measure();
        _grabItemsSp.Render();
    }

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
                    _grabItemsSp.SelectedIndex--;
                    break;
                case SDL.SDL_Keycode.SDLK_DOWN:
                    _grabItemsSp.SelectedIndex++;
                    break;

                case SDL.SDL_Keycode.SDLK_RETURN:
                    var item = _grabItemsSp.SelectedValue;
                    item.IsInInventory = true;
                    Program.Player.AddItemToInventory(item);
                    Program.RemoveWordSprite(item);
                    State.ActiveFocus = State.UiFocusE.Game;
                    break;
            }
        }
    }

    public void Pickup(IEnumerable<Item> objectsAtTile)
    {
        var atTile = objectsAtTile as Item[] ?? objectsAtTile.ToArray();
        if (!atTile.Any())
        {
            Console.WriteLine("Hmm, nothing to pickup here");
            State.ActiveFocus = State.UiFocusE.Game;
            return;
        }

        var player = Program.Player;


        _grabItemsSp = new StackPanel<Item>(() => atTile, item => new TextElement(item.Name))
        {
            EnableSelection = true,
            FillColor = SdlColors.Black
        };

        _grabItemsSp.Measure();

        _grabItemsSp.Y = Program.ScreenHeightPx / 2 - _grabItemsSp.Height / 2;
        _grabItemsSp.X = Program.ScreenWidthPx / 2 - _grabItemsSp.Width / 2;
    }
}
using SDL2;
using SDLApplication;

namespace Inferno.GuiElements;

public class EquippedItemBox : IGuiElement
{
    private Item _item;

    internal Item Item
    {
        get => _item;
        set
        {
            if (_item.Equals(value)) return;
            _item = value;
            _sprite = null;
            if (_item.Type == GameObjectType.Nothing) return;
            _sprite = Game.Assets.NewSprite(_item.Type);

            var size = MeasureSize();

            var scale = 1.0f / (128.0f / Program.App.ScreenWidth);

            _sprite._rect.w = size.w * (int)scale;
            _sprite._rect.h = size.h * (int)scale;
        }
    }

    private Sprite? _sprite;


    public SDL.SDL_Point Pos { get; set; }

    public Visibility Visibility { get; set; }

    public SDL.SDL_Rect MeasureSize()
    {
        if (_sprite == null) return new SDL.SDL_Rect();
        return _sprite._rect;
    }

    public void Render(RenderArgs ra)
    {
        if (Visibility != Visibility.Visible) return;
        
        //There is a little bit of an important order, the first time we render,
        // calling _sprite.Render sets _sprit._rect.x && .y, causing the little
        // shift to occur.
        _sprite?.Render(Pos, ra);
        var size = _sprite?._rect ?? default; // _sprite is a ptr, _rect is a value type, default is 0,0,0,0
        SDL.SDL_SetRenderDrawColor(ra.RendererPtr, 0, 0, 0, 255);
        SDL.SDL_RenderFillRect(ra.RendererPtr, ref size);
        _sprite?.Render(Pos, ra);

    }
}
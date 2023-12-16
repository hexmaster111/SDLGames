using SDL2;
using SDLApplication;

namespace Inferno.GuiElements;

public interface IGuiElement
{
    public Visibility Visibility { get; set; }
    public SDL.SDL_Rect MeasureSize();
    public void Render(RenderArgs ra);
}
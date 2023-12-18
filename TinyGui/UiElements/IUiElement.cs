using SDL2;

namespace TinyGui.UiElements;

public abstract class UiElement
{
 

    public int X { get; set; }
    public int Y { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public bool IsVisible { get; set; } = true;
    public abstract void Render();
    public abstract void Measure();
}
namespace TinyGui.UiElements;

public abstract class UiElement
{
    public int X { get; set; }
    public int Y { get; set; }

    private int _width = int.MaxValue;
    private int _height = int.MaxValue;
    private bool _measured = false;

    public int Width
    {
        get
        {
            if (!_measured) Measure();
            _measured = true;
            return _width;
        }
        protected set => _width = value;
    }

    public int Height
    {
        get
        {
            if (!_measured) Measure();
            _measured = true;
            return _height;
        }
        protected set => _height = value;
    }

    public bool IsVisible { get; set; } = true;
    public abstract void Render();
    public abstract void Measure();
}
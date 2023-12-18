namespace TinyGui.UiElements;

public class StackPanel : UiElement
{
    public List<UiElement> Children { get; } = new();

    public override void Render()
    {
        if (!IsVisible)
        {
            return;
        }

        int x = X;
        int y = Y;

        foreach (var child in Children)
        {
            child.X = x;
            child.Y = y;
            child.Render();
            y += child.Height;
        }
    }

    public override void Measure()
    {
      
        int width = 0;
        int height = 0;
        foreach (var child in Children)
        {
            child.Measure();
            width = Math.Max(width, child.Width);
            height += child.Height;
        }
        Width = width;
        Height = height;
        
    }
}
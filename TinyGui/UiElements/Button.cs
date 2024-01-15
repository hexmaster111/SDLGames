namespace TinyGui.UiElements;

public class Button : UiElement
{
    public Button(string label)
    {
        Label = label;
    }


    public string Label { get; set; }
    private TextElement _labelElement = new("");

    public override void Render()
    {
        if (!IsVisible)
        {
            return;
        }

        _labelElement.X = X;
        _labelElement.Y = Y;
        _labelElement.IsVisible = IsVisible;
        _labelElement.Render();
    }

    public override void Measure()
    {
        _labelElement.Text = Label;
        _labelElement.Measure();
        Width = _labelElement.Width;
        Height = _labelElement.Height;
    }
}
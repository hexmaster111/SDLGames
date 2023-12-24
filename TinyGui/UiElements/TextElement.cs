using SDL2;

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

public class TextElement : UiElement
{
    public string Text { get; set; }


    private readonly FontTextureWrapper _texture = TinyGuiShared.TextTexture;
    private readonly int _fontSize = TinyGuiShared.TextTexture.FontWidth;

    public TextElement(string text)
    {
        Text = text;
    }


    public override void Render()
    {
        if (!IsVisible)
        {
            return;
        }

        int x = X;
        int y = Y;
        int xStart = x;

        foreach (var ch in Text)
        {
            int state = ch;
            if (ch == '\n')
            {
                newLine();
                continue;
            }

            //Ascii upArrow
            if (ch == '↑')
            {
                state = 24;
            }
            
            if (ch == '↓')
            {
                state = 25;
            }
            
            if (ch == '→')
            {
                state = 26;
            }
            
            if (ch == '←')
            {
                state = 27;
            }

            _texture.SetState((char)state);
            _texture.Render(x, y);
            x += _fontSize;
            continue;

            void newLine()
            {
                y += _fontSize;
                x = xStart;
            }
        }

        x = xStart;
    }

    public override void Measure()
    {
        var width = 0;
        var height = _fontSize;
        int maxWidth = int.MinValue;

        foreach (var ch in Text)
        {
            if (ch == '\n') newLine();

            width += _fontSize;
            if (width > maxWidth) maxWidth = width;
            continue;

            void newLine()
            {
                if (width > maxWidth) maxWidth = width;
                Height += _fontSize;
                width = 0;
            }
        }

        width = maxWidth;

        Width = width;
        Height = height;
    }
}
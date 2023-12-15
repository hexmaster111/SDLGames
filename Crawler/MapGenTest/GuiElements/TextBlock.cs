using MapGenTest.GuiElements;
using SDL2;
using SDLApplication;

namespace MapGenTest;

public class TextBlock : IGuiElement
{
    public int FontSizeSrc = 8;

    public int FontSizeDest = Style.FontSize;

    //Not FontSize as we want to zoom things in a bit
    private readonly StatefulSprite _sprite;
    private readonly string _text;

    public TextBlock(string text, SDL.SDL_Point pos)
    {
        _sprite = new(GameAssetFactory.FontWrapper.TexturePtr, FontSizeDest, FontSizeDest, 128, 1)
        {
            CurrentSpriteRect = new SDL.SDL_Rect() { w = FontSizeSrc, h = FontSizeSrc, },
            NextFrameIncAmount = FontSizeSrc
        };

        _text = text;
        Text = text;
        Pos = pos;
    }

    public string Text { get; set; }
    public SDL.SDL_Point Pos { get; set; }


    public SDL.SDL_Rect MeasureSize()
    {
        var h = Text.MeasureFontHeight(FontSizeDest);
        var w = Text.MeasureFontWidth(FontSizeDest);
        return new SDL.SDL_Rect() { w = w, h = h };
    }

    public void Render(RenderArgs ra)
    {
        if (string.IsNullOrWhiteSpace(Text)) return;
        var sr = new StringReader(Text);
        int r;
        var pos = Pos;
        while ((r = sr.Read()) != -1)
        {
            _sprite.SetState(r);
            _sprite.Render(pos, ra);
            switch (r)
            {
                case '\n':
                    pos.y += FontSizeDest;
                    continue;
                case '\r':
                    pos.x = Pos.x;
                    continue;
                //we can do other cases to change colors etc
            }

            pos.x += FontSizeDest;
        }
    }
}
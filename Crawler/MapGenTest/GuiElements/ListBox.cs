using SDL2;
using SDLApplication;

namespace MapGenTest.GuiElements;

public class ListBox : IGuiElement
{
    private int _selectedIndex = 0;
    public SDL.SDL_Point Pos { get; set; }

    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            if (value < 0) value = Items.Count - 1;
            if (value >= Items.Count) value = 0;
            _selectedIndex = value;
        }
    }

    public List<string> Items { get; set; } = new();
    public int FontSizeDest { get; set; } = Style.FontSize;

    public Visibility Visibility { get; set; } = Visibility.Visible;

    public SDL.SDL_Rect MeasureSize()
    {
        var h = Items.Count * FontSizeDest;
        var w = Items.Max(x => x.MeasureFontWidth(FontSizeDest));
        return new SDL.SDL_Rect { w = w, h = h };
    }

    public virtual void Render(RenderArgs ra)
    {
        if (Visibility != Visibility.Visible) return;
        var pos = Pos;
        int i = 0;
        foreach (var item in Items)
        {
            var tb = new TextBlock(item, pos);
            if (i == SelectedIndex)
            {
                ra.SetDrawColor(SdlColors.DarkYellow);
                ra.FillRect(new SDL.SDL_Rect() { x = Pos.x, y = pos.y, w = tb.MeasureSize().w, h = FontSizeDest });
            }


            tb.Render(ra);
            pos.y += FontSizeDest;
            i++;
        }
    }
}

public class ContextMenu<T> : ListBox where T : struct, Enum
{
    private readonly List<T> _options = new();
    public T SelectedItem => _options[SelectedIndex];

    public void AddOption(T opt)
    {
        _options.Add(opt);
        Items.Add(opt.ToString());
    }

    public override void Render(RenderArgs ra)
    {
        if (Visibility == Visibility.Hidden) return;
        //Render the Background
        var size = base.MeasureSize();
        size = size with
        {
            x = Pos.x,
            y = Pos.y
        };

        ra.SetDrawColor(SdlColors.Teal);
        ra.FillRect(size);
        base.Render(ra);
    }
}
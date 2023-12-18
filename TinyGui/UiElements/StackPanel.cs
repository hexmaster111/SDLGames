using static SDL2.SDL;

namespace TinyGui.UiElements;

public class StackPanel<TValue> : StackPanel
{
    private GetMethod<IEnumerable<TValue>> GetListOfValue;
    private Func<TValue, UiElement> ElementFactory;


    public StackPanel(GetMethod<IEnumerable<TValue>> getListOfValue, Func<TValue, UiElement> elementFactory)
    {
        GetListOfValue = getListOfValue;
        ElementFactory = elementFactory;
    }

    public TValue SelectedValue
    {
        get
        {
            var list = GetListOfValue().ToList();
            if (list.Count == 0) return default;
            if (SelectedIndex >= list.Count) SelectedIndex = list.Count - 1;
            return list.ElementAt(SelectedIndex);
        }
    }


    public void UpdateChildren()
    {
        Children.Clear();
        foreach (var item in GetListOfValue())
        {
            Children.Add(ElementFactory(item));
        }
    }

    public override void Render()
    {
        if (!IsVisible) return;
        base.Render();
    }
}

public delegate T GetMethod<T>();

public class StackPanel : UiElement
{
    public virtual List<UiElement> Children { get; } = new();


    private int _selectedIndex = 0;

    //wrap around down and up
    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            if (value < 0) _selectedIndex = Children.Count - 1;
            else if (value >= Children.Count) _selectedIndex = 0;
            else _selectedIndex = value;
        }
    }

    public bool EnableSelection { get; set; } = false;

    public SDL_Color SelectedColor { get; set; } = new SDL_Color()
    {
        r = 255,
        g = 0,
        b = 0,
        a = 255,
    };

    public override void Render()
    {
        if (!IsVisible)
        {
            return;
        }

        int x = X;
        int y = Y;

        int currIndex = 0;

        foreach (var child in Children)
        {
            if (EnableSelection && currIndex == SelectedIndex)
            {
                var bgRect = new SDL_Rect()
                {
                    x = x,
                    y = y,
                    w = child.Width,
                    h = child.Height,
                };

                SDL_SetRenderDrawColor(TinyGuiShared.RendererPtr,
                    SelectedColor.r, SelectedColor.g,
                    SelectedColor.b, SelectedColor.a);

                SDL_RenderFillRect(TinyGuiShared.RendererPtr, ref bgRect);
            }

            child.X = x;
            child.Y = y;
            child.Render();
            y += child.Height;
            currIndex++;
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
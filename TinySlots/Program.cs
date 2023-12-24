using SDLApplication;
using static SDL2.SDL;

namespace TinySlots;

public static class Program
{
    internal static SdlApp App;
    private const int ScreenHeightPx = 600;
    private const int ScreenWidthPx = 800;

    private static SlotWheel _slotWheel1;
    private static SlotWheel _slotWheel2;
    private static SlotWheel _slotWheel3;


    public static void Main()
    {
        App = new SdlApp(EventHandler, RenderHandler, UpdateHandler,
            targetFps: 60,
            width: ScreenWidthPx,
            height: ScreenHeightPx);

        int slotWidth = 32 * 4;

        _slotWheel1 = new SlotWheel(0, 0, slotWidth, slotWidth);
        _slotWheel2 = new SlotWheel(slotWidth, 0, slotWidth, slotWidth);
        _slotWheel3 = new SlotWheel(slotWidth * 2, 0, slotWidth, slotWidth);

        App.Run();
    }

    private static void UpdateHandler(TimeSpan deltaTime, long now)
    {
        _slotWheel1.Update(now);
        _slotWheel2.Update(now);
        _slotWheel3.Update(now);
    }

    private static void RenderHandler(RenderArgs args)
    {
        _slotWheel1.Render(args);
        _slotWheel2.Render(args);
        _slotWheel3.Render(args);
    }

    private static void EventHandler(SDL_Event e)
    {
    }
}

internal class SlotWheel
{
    public SlotWheel(int x, int y, int width, int height)
    {
        SlotWindowRect = new SDL_Rect { x = x, y = y, w = width, h = height };
        var textures = new[]
        {
            Textures.Seven,
            Textures.Bar,
            Textures.Cherry,
            Textures.Bell,
            Textures.BarBar,
            Textures.Grape,
            Textures.Orange,
            Textures.BarBarBar,
        };
        
        
        
        IsSpinning = true;
    }


    public SDL_Rect SlotWindowRect;
    public bool IsSpinning;

    public void Render(RenderArgs args)
    {
    }

    public void Update(long now)
    {
    }
}
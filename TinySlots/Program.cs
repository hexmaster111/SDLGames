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
            height: ScreenHeightPx,
            targetUpdatesPerSec: 30
        );

        _slotWheel1 = new SlotWheel(20, 20, WheelSymbolList.Wheel1);
        _slotWheel2 = new SlotWheel(_slotWheel1.Position.x + _slotWheel1.Measure().w, _slotWheel1.Position.y,
            WheelSymbolList.Wheel2);
        _slotWheel3 = new SlotWheel(_slotWheel2.Position.x + _slotWheel2.Measure().w, _slotWheel1.Position.y,
            WheelSymbolList.Wheel3);

        App.Run();
    }

    private static void UpdateHandler(TimeSpan deltaTime, long now)
    {
        _slotWheel1?.Update(now);
        _slotWheel2?.Update(now);
        _slotWheel3?.Update(now);
    }

    private static void RenderHandler(RenderArgs args)
    {
        _slotWheel1?.Render(args);
        _slotWheel2?.Render(args);
        _slotWheel3?.Render(args);
    }

    private static void EventHandler(SDL_Event e)
    {
    }
}

internal class SlotWheel
{
    private readonly Symbol[] _symbols;
    public float SpinSpeed = .1f;
    public SDL_Point Position;
    public float State;
    SymbolTexture _symbolTexture;

    public SlotWheel(int x, int y, Symbol[] symbols)
    {
        Position = new SDL_Point { x = x, y = y };
        _symbols = symbols;
        //Take all the symbols, and turn it into a single texture
        //Then, we can just render the texture, and scroll it

        _symbolTexture = new SymbolTexture(_symbols)
        {
            DestRect = new SDL_Rect { x = x, y = y, w = 64, h = 64 }
        };
    }


    public void Render(RenderArgs args)
    {
        _symbolTexture.Render(Position.x, Position.y, State);
    }

    public void Update(long now)
    {
        State += SpinSpeed;
        if (State > _symbols.Length)
        {
            State = 0;
        }
    }

    public SDL_Rect Measure() => _symbolTexture.DestRect;
}

internal class SymbolTexture
{
    public SDL_Rect SourceRect;
    public SDL_Rect DestRect { get; set; } = new SDL_Rect { w = 32, h = 32 };
    public readonly IntPtr TexturePtr;


    public void Render(int x, int y, float state)
    {
        DestRect = new SDL_Rect { x = x, y = y, w = DestRect.w, h = DestRect.h };
        SourceRect.y = (int)(state * 32);
        var dr = DestRect;
        SDL_RenderCopy(Program.App.RendererPtr, TexturePtr, ref SourceRect, ref dr);
    }

    public SymbolTexture(Symbol[] symbols)
    {
        TexturePtr = SDL_CreateTexture(Program.App.RendererPtr, SDL_PIXELFORMAT_RGB24,
            (int)SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET,
            32, symbols.Length * 32);

        SourceRect.x = 0;
        SourceRect.w = 32;
        SourceRect.h = 32;


        SDL_SetRenderTarget(Program.App.RendererPtr, TexturePtr);
        SDL_SetRenderDrawColor(Program.App.RendererPtr, 0, 0, 0, 0);
        SDL_RenderClear(Program.App.RendererPtr);
        SDL_SetRenderDrawColor(Program.App.RendererPtr, 255, 255, 255, 255);
        for (var i = 0; i < symbols.Length; i++)
        {
            var symbolRect = new SDL_Rect { x = 0, y = i * 32, w = 32, h = 32 };
            SDL_RenderDrawRect(Program.App.RendererPtr, ref symbolRect);
        }

        SDL_SetRenderTarget(Program.App.RendererPtr, IntPtr.Zero);
        //Debug render the texture to a file name from first 3 symbols
        SDL_SaveBMP(TexturePtr, $"test.bmp");
    }
}
using System.Diagnostics;
using SDL2;

namespace SDLApplication;

public class SdlApp
{
    private IntPtr WindowPtr = IntPtr.Zero;
    private IntPtr RendererPtr = IntPtr.Zero;
    private IntPtr FontPtr = IntPtr.Zero;

    private int ScreenWidth = 320;
    private int ScreenHeight = 240;

    private bool Running = true;
    private int Fps = 0;

    private Action<SDL.SDL_Event> _eventHandler;
    private Action<RenderArgs> _renderHandler;



    public SdlApp
    (
        Action<SDL.SDL_Event> eventHandler,
        Action<RenderArgs> renderHandler,
        int width = 320, int height = 240
    )
    {
        ScreenWidth = width;
        ScreenHeight = height;
        _eventHandler = eventHandler;
        _renderHandler = renderHandler;
        if (!SetupSdl()) throw new Exception("Failed to setup SDL");

    }

    public SdlApp Run(int targetFps = 60)
    {
        long lastTime = SDL.SDL_GetTicks();
        long currentTime = SDL.SDL_GetTicks();
        long deltaTime;

        while (Running)
        {
            currentTime = SDL.SDL_GetTicks();
            deltaTime = currentTime - lastTime;
            if (targetFps > 0)
            {

                if (deltaTime < 1000 / targetFps)
                {

                    var timeToSleep = (1000 / targetFps) - deltaTime;
                    Thread.Sleep((int)timeToSleep);
                    continue;
                }

                lastTime = currentTime;
                Fps = (int)(1000 / deltaTime);
            }

            if (deltaTime > 1000)
            {
                // If we hit a breakpoint, the delta time will be huge.
                continue;
            }

            HandleEvents();
            Render(deltaTime);
        }

        return this;
    }

    internal void Dispose()
    {
        SDL.SDL_DestroyRenderer(RendererPtr);
        SDL.SDL_DestroyWindow(WindowPtr);
        SDL_ttf.TTF_CloseFont(FontPtr);
        SDL_ttf.TTF_Quit();
        SDL.SDL_Quit();
    }

    private void HandleEvents()
    {
        while (SDL.SDL_PollEvent(out var e) != 0)
        {
            switch (e.type)
            {
                case SDL.SDL_EventType.SDL_QUIT:
                    Running = false;
                    break;
                default:
                    _eventHandler?.Invoke(e);
                    break;
            }
        }
    }

    private void Render(double deltaTime)
    {
        SDL.SDL_SetRenderDrawColor(RendererPtr, 0x10, 0x10, 0x00, 0xFF);
        SDL.SDL_RenderClear(RendererPtr);
        _renderHandler?.Invoke(new RenderArgs(WindowPtr, RendererPtr, FontPtr, Fps, deltaTime, ScreenWidth, ScreenHeight));
        RenderFps();
        SDL.SDL_RenderPresent(RendererPtr);
    }

    private int[] _fpsHistory = new int[60];
    private int _fpsHistoryIndex = 0;
    private void RenderFps()
    {
        _fpsHistory[_fpsHistoryIndex] = Fps;
        _fpsHistoryIndex++;
        if (_fpsHistoryIndex >= _fpsHistory.Length)
        {
            _fpsHistoryIndex = 0;
        }

        var avgFps = _fpsHistory.Average();

        var fpsText = $"FPS: {avgFps:00}";
        var FPSSurface = SDL2.SDL_ttf.TTF_RenderText_Solid(FontPtr, fpsText,
            new SDL.SDL_Color() { r = 0xFF, g = 0xFF, b = 0xFF, a = 0xFF });
        var fpsTexture = SDL.SDL_CreateTextureFromSurface(RendererPtr, FPSSurface);
        SDL.SDL_Rect fpsRect = new SDL.SDL_Rect()
        {
            x = 0,
            y = 0,
            w = 100 / 2,
            h = 24 / 2
        };
        SDL.SDL_RenderCopy(RendererPtr, fpsTexture, IntPtr.Zero, ref fpsRect);
        SDL.SDL_DestroyTexture(fpsTexture);
        SDL.SDL_FreeSurface(FPSSurface);
    }

    private bool SetupSdl()
    {
        if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
        {
            Console.WriteLine($"SDL could not initialize! SDL_Error: {SDL.SDL_GetError()}");
            return false;
        }

        SDL.SDL_GetVersion(out var ver);


        Console.WriteLine($"SDL V{ver.major}.{ver.minor}.{ver.patch} initialized");

        var res = SDL.SDL_CreateWindowAndRenderer(ScreenWidth, ScreenHeight,
            SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN,
            out WindowPtr,
            out RendererPtr);

        if (res < 0)
        {
            Console.WriteLine($"SDL could not create window and renderer! SDL_Error: {SDL.SDL_GetError()}");
            return false;
        }

        Debug.Assert(WindowPtr != IntPtr.Zero);
        Debug.Assert(RendererPtr != IntPtr.Zero);

        if (SDL2.SDL_ttf.TTF_Init() < 0)
        {
            Console.WriteLine($"SDL_ttf could not initialize! SDL_ttf Error:" +
                              $"{SDL2.SDL_ttf.TTF_GetError()}");
            return false;
        }

        FontPtr = SDL2.SDL_ttf.TTF_OpenFont("Assets/TerminusTTF.ttf", 12);
        if (FontPtr == IntPtr.Zero)
        {
            Console.WriteLine($"Failed to load font! SDL_ttf Error: {SDL2.SDL_ttf.TTF_GetError()}");
            Console.WriteLine($"Current directory: {Environment.CurrentDirectory}");
            return false;
        }


        return true;
    }
}
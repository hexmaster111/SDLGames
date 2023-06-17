using System.Diagnostics;
using System.Timers;
using SDL2;

namespace SDLApplication;

public class SdlApp
{
    private IntPtr WindowPtr = IntPtr.Zero;
    private IntPtr RendererPtr = IntPtr.Zero;
    private IntPtr FontPtr = IntPtr.Zero;

    private int ScreenWidth = 320;
    private int ScreenHeight = 240;
    private int _targetFps;

    private bool Running = true;
    private int Fps = 0;

    private TimeOnly _lastTime = TimeOnly.MinValue;

    private readonly EventHandler _eventHandler;
    private readonly RenderHandler _renderHandler;
    private readonly UpdateHandler _updateHandler;

    public delegate void RenderHandler(RenderArgs args);

    public delegate void EventHandler(SDL.SDL_Event e);

    public delegate void UpdateHandler(TimeSpan deltaTime);


    public SdlApp
    (
        EventHandler eventHandler,
        RenderHandler renderHandler,
        UpdateHandler updateHandler,
        int width = 320, int height = 240, int targetFps = 60
    )
    {
        ScreenWidth = width;
        ScreenHeight = height;
        _eventHandler = eventHandler;
        _renderHandler = renderHandler;
        _targetFps = targetFps;
        _updateHandler = updateHandler;
        if (!SetupSdl()) throw new Exception("Failed to setup SDL");
    }

    public SdlApp Run()
    {
        const int UPDATE_HZ = 100;
        long lastRender = SDL.SDL_GetTicks();
        long lastUpdate = SDL.SDL_GetTicks();

        while (Running)
        {
            long currentTime = SDL.SDL_GetTicks();
            var deltaTime = currentTime - lastRender;
            if (_targetFps > 0)
            {
                if (deltaTime >= 1000 / _targetFps)
                {
                    // If we hit a breakpoint, the delta time will be huge.
                    if (deltaTime <= 1000)
                    {
                        lastRender = currentTime;
                        Fps = (int)(1000 / deltaTime);
                        HandleEvents();
                        Render(deltaTime);
                    }
                    else lastRender = currentTime;
                }

                //Timer for updating the game
                if (currentTime - lastUpdate >= 1000 / UPDATE_HZ)
                {
                    lastUpdate = currentTime;
                    _updateHandler?.Invoke(TimeSpan.FromMilliseconds(1000 / UPDATE_HZ));
                }


                //The time to sleep is made up of time last update and the last render, so they can fire on time
                var timeToSleep = (1000 / _targetFps) - (SDL.SDL_GetTicks() - lastRender);
                //Now for the update
                timeToSleep = Math.Min(timeToSleep, (1000 / UPDATE_HZ) - (SDL.SDL_GetTicks() - lastUpdate));
                if (timeToSleep > 0) SDL.SDL_Delay((uint)timeToSleep);
            }
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
        _renderHandler?.Invoke(new RenderArgs(WindowPtr, RendererPtr, FontPtr, Fps, deltaTime, ScreenWidth,
            ScreenHeight));
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

    private void UpdateEventTimerCb()
    {
        var now = TimeOnly.FromDateTime(DateTime.UtcNow);
        var deltaTime = now - _lastTime;


        _updateHandler?.Invoke(deltaTime);
        _lastTime = now;
    }
}
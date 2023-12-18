using System.Diagnostics;
using System.Timers;
using SDL2;

namespace SDLApplication;

public class SdlApp
{
    private IntPtr WindowPtr = IntPtr.Zero;
    public IntPtr RendererPtr { get; private set; } = IntPtr.Zero;
    private IntPtr FontPtr = IntPtr.Zero;

    public int ScreenWidth = 320;
    public int ScreenHeight = 240;
    private int _targetFps;
    private int _targetUpdatesPerSec;

    private bool Running = true;
    private int frameCount = 0;
    private int updateCount = 0;

    private readonly EventHandler _eventHandler;
    private readonly RenderHandler _renderHandler;
    private readonly UpdateHandler _updateHandler;

    public delegate void RenderHandler(RenderArgs args);

    public delegate void EventHandler(SDL.SDL_Event e);

    public delegate void UpdateHandler(TimeSpan deltaTime, long now);


    public SdlApp
    (
        EventHandler eventHandler,
        RenderHandler renderHandler,
        UpdateHandler updateHandler,
        int width = 320, int height = 240, int targetFps = 60, int targetUpdatesPerSec = 100
    )
    {
        ScreenWidth = width;
        ScreenHeight = height;
        _eventHandler = eventHandler;
        _renderHandler = renderHandler;
        _targetFps = targetFps;
        _updateHandler = updateHandler;
        _targetUpdatesPerSec = targetUpdatesPerSec;
        if (!SetupSdl()) throw new Exception("Failed to setup SDL");
        TinyGui.TinyGuiShared.Init(RendererPtr);
    }


    public SdlApp Run()
    {
        long lastRender = SDL.SDL_GetTicks();

        SimpleTimer renderTimer = new(1000 / _targetFps);
        SimpleTimer updateTimer = new(1000 / _targetUpdatesPerSec);
        SimpleTimer oneSecTmr = new(1000);

        while (Running)
        {
            HandleEvents();
            long currentTime = SDL.SDL_GetTicks();

            if (updateTimer.Evaluate(currentTime))
            {
                _updateHandler.Invoke(TimeSpan.FromMilliseconds(1000 / _targetUpdatesPerSec), currentTime);
                updateCount++;
            }

            if (renderTimer.Evaluate(currentTime))
            {
                var deltaTime = currentTime - lastRender;
                RenderFps();
                frameCount++;
                Render(deltaTime);
                lastRender = currentTime;
            }

            if (oneSecTmr.Evaluate(currentTime))
            {
                FpsSample();
                UpsSample();
                frameCount = 0;
                updateCount = 0;
            }

            var minTime = Math.Min(updateTimer.SleepTimeMs, renderTimer.SleepTimeMs);
            if (0 > minTime) continue;
            SDL.SDL_Delay((uint)minTime);
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
        _renderHandler?.Invoke(new RenderArgs(WindowPtr, RendererPtr, FontPtr, Fps,Ups, deltaTime, ScreenWidth,
            ScreenHeight));
        RenderFps();
        SDL.SDL_RenderPresent(RendererPtr);
    }

    public int Fps { get;private set; }
    public int Ups { get;private set; }

    private int[] _fpsHistory = new int[10]; //Seconds of average
    private int _fpsHistoryIndex = 0;

    private int[] _upsHistory = new int[10]; //Seconds of average
    private int _upsHistoryIndex = 0;

    private void FpsSample()
    {
        _fpsHistory[_fpsHistoryIndex] = frameCount;
        _fpsHistoryIndex++;
        if (_fpsHistoryIndex >= _fpsHistory.Length)
        {
            _fpsHistoryIndex = 0;
        }
    }

    private void UpsSample()
    {
        _upsHistory[_upsHistoryIndex] = updateCount;
        _upsHistoryIndex++;
        if (_upsHistoryIndex >= _upsHistory.Length)
        {
            _upsHistoryIndex = 0;
        }
    }

    private void RenderFps()
    {
        var avgFps = _fpsHistory.Average();
        var avgUps = _upsHistory.Average();
        Fps = (int)avgFps;
        Ups = (int)avgUps;

        var fpsText = $"FPS: {avgFps:00} UPS: {avgUps:00}";
        var FPSSurface = SDL2.SDL_ttf.TTF_RenderText_Solid(FontPtr, fpsText,
            new SDL.SDL_Color() { r = 0xFF, g = 0xFF, b = 0xFF, a = 0xFF });
        var fpsTexture = SDL.SDL_CreateTextureFromSurface(RendererPtr, FPSSurface);
        SDL.SDL_Rect fpsRect = new SDL.SDL_Rect()
        {
            x = 0,
            y = 0,
            w = 100,
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

        var fullScreenArgPresent = Environment.GetCommandLineArgs().Any(arg => arg == "--fullscreen");

        var fullScreenFlag = fullScreenArgPresent
            ? SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP
            : SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN;


        var res = SDL.SDL_CreateWindowAndRenderer(ScreenWidth, ScreenHeight,
            fullScreenFlag,
            out WindowPtr,
            out var tmpRenderPtrOut);

        RendererPtr = tmpRenderPtrOut;

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
            Console.WriteLine($"current dir {Environment.CurrentDirectory}");
            Console.WriteLine($"Current directory: {Environment.CurrentDirectory}");
            return false;
        }

        return true;
    }
}
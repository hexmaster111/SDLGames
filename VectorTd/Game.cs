using SDL2;
using SDLApplication;

namespace VectorTd;

public class Game
{
    private const int ScreenWidth = 640;
    private const int ScreenHeight = 480;

    public Game() => new SdlApp(EventHandler, RenderHandler, ScreenWidth, ScreenHeight).Run();

    private void EventHandler(SDL.SDL_Event e)
    {
    }

    private void RenderHandler(RenderArgs args)
    {
    }
}
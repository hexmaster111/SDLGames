using SDL2;
using static SDL2.SDL;

namespace SDLApplication;

public struct RenderArgs
{
    public IntPtr WindowPtr;
    public IntPtr RendererPtr;
    public int Fps;
    public int Ups;
    public double DeltaTime;
    public int ScreenWidth_Px;
    public int ScreenHeight_px;

    public RenderArgs(IntPtr windowPtr, nint rendererPtr, int fps, int ups, double deltaTime, int width_px,
        int height_px)
    {
        RendererPtr = rendererPtr;
        Fps = fps;
        DeltaTime = deltaTime;
        WindowPtr = windowPtr;
        ScreenWidth_Px = width_px;
        ScreenHeight_px = height_px;
        Ups = ups;
    }


    public void DrawRect(SDL_Rect sdlRect) => SDL_RenderDrawRect(RendererPtr, ref sdlRect);

    public void FillRect(SDL_Rect fillRect) => SDL_RenderFillRect(RendererPtr, ref fillRect);

    [Obsolete("Use ui elements now plz")]
    public void RenderText(string text, int x, int y, SDL_Color? color = null, SDL_Color? background = null)
    {
    }

    public void DrawLine(int x, int y, double x2, double y2, int thickness, SDL_Color color)
    {
        SDL_SetRenderDrawColor(RendererPtr, color.r, color.g, color.b, color.a);

        for (int i = 0; i < thickness; i++)
        {
            SDL_RenderDrawLine(RendererPtr, x, y + i, (int)x2, (int)y2 + i);
        }
    }
}

public static class Renderer
{
    public static void SetDrawColor(this RenderArgs args, byte r, byte g, byte b, byte a) =>
        SDL_SetRenderDrawColor(args.RendererPtr, r, g, b, a);

    public static void SetDrawColor(this RenderArgs args, SDL_Color c) =>
        SDL_SetRenderDrawColor(args.RendererPtr, c.r, c.g, c.b, c.a);
}
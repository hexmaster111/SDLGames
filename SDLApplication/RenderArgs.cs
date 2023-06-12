using static SDL2.SDL;

namespace SDLApplication;

public struct RenderArgs
{
    public IntPtr WindowPtr;
    public IntPtr RendererPtr;
    public IntPtr FontPtr;
    public int Fps;
    public double DeltaTime;
    public int ScreenWidth_Px;
    public int ScreenHeight_px;

    public RenderArgs(IntPtr windowPtr, nint rendererPtr, nint fontPtr, int fps, double deltaTime, int width_px, int height_px)
    {
        RendererPtr = rendererPtr;
        FontPtr = fontPtr;
        Fps = fps;
        DeltaTime = deltaTime;
        WindowPtr = windowPtr;
        ScreenWidth_Px = width_px;
        ScreenHeight_px = height_px;
    }

    public void DrawRect(SDL_Rect sdlRect) => SDL_RenderDrawRect(RendererPtr, ref sdlRect);

    public void FillRect(SDL_Rect fillRect) => SDL_RenderFillRect(RendererPtr, ref fillRect);
}

public static class Renderer
{
    public static void SetDrawColor(this RenderArgs args, byte r, byte g, byte b, byte a) => SDL_SetRenderDrawColor(args.RendererPtr, r, g, b, a);
    public static void SetDrawColor(this RenderArgs args, SDL_Color c) => SDL_SetRenderDrawColor(args.RendererPtr, c.r, c.g, c.b, c.a);
}
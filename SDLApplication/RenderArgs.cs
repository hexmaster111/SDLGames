using SDL2;
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

    public void RenderText(string text, int x, int y, SDL_Color? color = null, SDL_Color? background = null)
    {
        var colorToUse = color ?? new SDL_Color { r = 255, g = 255, b = 255, a = 255 };
        //Use SDL_TTF to render our text

        var textSurface = SDL_ttf.TTF_RenderText_Solid(FontPtr, text, colorToUse);
        var textTexture = SDL_CreateTextureFromSurface(RendererPtr, textSurface);
        //marshal the text texture's dimensions
        SDL_QueryTexture(textTexture, out _, out _, out var textWidth, out var textHeight);
        //set the text's position and size
        SDL_Rect renderQuad = new SDL_Rect()
        {
            x = x,
            y = y,
            w = textWidth,
            h = textHeight
        };
        //render background
        if (background.HasValue)
        {
            SDL_SetRenderDrawColor(RendererPtr, background.Value.r, background.Value.g, background.Value.b, background.Value.a);
            SDL_RenderFillRect(RendererPtr, ref renderQuad);
        }

        //render to screen
        SDL_RenderCopy(RendererPtr, textTexture, IntPtr.Zero, ref renderQuad);
        //clean up unmanaged resources
        SDL_DestroyTexture(textTexture);
        SDL_FreeSurface(textSurface);
    }
}

public static class Renderer
{
    public static void SetDrawColor(this RenderArgs args, byte r, byte g, byte b, byte a) => SDL_SetRenderDrawColor(args.RendererPtr, r, g, b, a);
    public static void SetDrawColor(this RenderArgs args, SDL_Color c) => SDL_SetRenderDrawColor(args.RendererPtr, c.r, c.g, c.b, c.a);
}
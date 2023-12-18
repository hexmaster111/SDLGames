using System.Runtime.InteropServices;
using SDL2;
using static SDL2.SDL;

namespace TinyGui;

internal class TextureWrapper
{
    public int FontWidth { get; private set; }
    public int FontHeight { get; private set; }
    protected IntPtr MTexture = IntPtr.Zero;


    // For anamated textures
    public TextureWrapper(string path, int fontWidth, int fontHeight)
    {
        LoadFromPath(path);
        FontWidth = fontWidth;
        FontHeight = fontHeight;
    }

    public void LoadFromPath(string path)
    {
        Free();
        var surface = SDL_image.IMG_Load(path);
        if (surface == IntPtr.Zero) throw new Exception($"Failed to load texture {path}! {SDL_image.IMG_GetError()}");
        var surfaceStruct = Marshal.PtrToStructure<SDL_Surface>(surface);
        SDL_SetColorKey(surface, 1, SDL_MapRGB(surfaceStruct.format, 0xff, 0x66, 0xff));
        var newTexture = SDL_CreateTextureFromSurface(TinyGuiShared.RendererPtr, surface);
        if (newTexture == IntPtr.Zero) throw new Exception($"Failed to create texture {path} Error:{SDL_GetError()}");
        surfaceStruct = Marshal.PtrToStructure<SDL_Surface>(surface);
        FontWidth = surfaceStruct.w;
        FontHeight = surfaceStruct.h;
        SDL_FreeSurface(surface);
        MTexture = newTexture;
    }

    public void SetColor(SDL_Color color) => SDL_SetTextureColorMod(MTexture, color.r, color.g, color.b);

    public void SetBlendMode(SDL_BlendMode blendMode) => SDL_SetTextureBlendMode(MTexture, blendMode);

    public void SetAlpha(byte alpha) => SDL_SetTextureAlphaMod(MTexture, alpha);

    public virtual void Render(int x, int y)
    {
        var rq = new SDL_Rect { x = x, y = y, w = FontWidth, h = FontHeight };
        var srcRq = new SDL_Rect { x = 0, y = 0, w = FontWidth, h = FontHeight };
        SDL_RenderCopy(TinyGuiShared.RendererPtr, MTexture, ref srcRq, ref rq);
    }


    ~TextureWrapper() => Free();

    private void Free()
    {
        if (MTexture != IntPtr.Zero)
        {
            SDL_DestroyTexture(MTexture);
        }
    }
}
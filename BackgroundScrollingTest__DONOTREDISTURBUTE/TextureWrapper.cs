using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SDL2;
using static SDL2.SDL;

namespace BackgroundScrollingTest__DONOTREDISTURBUTE;

internal class TextureWrapper
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    private IntPtr _mTexture = IntPtr.Zero;

    public TextureWrapper(string path)
    {
        LoadFromPath(path);
    }

    public void LoadFromPath(string path)
    {
        Free();
        var newTexture = IntPtr.Zero;
        var surface = SDL_image.IMG_Load(path);
        if (surface == IntPtr.Zero) throw new Exception($"Failed to load texture {path}! {SDL_image.IMG_GetError()}");
        var surfaceStruct = Marshal.PtrToStructure<SDL_Surface>(surface);
        SDL_SetColorKey(surface, 1, SDL_MapRGB(surfaceStruct.format, 0xff, 0x66, 0xff));
        newTexture = SDL_CreateTextureFromSurface(Program.App.RendererPtr, surface);
        if (newTexture == IntPtr.Zero) throw new Exception($"Failed to create texture {path} Error:{SDL_GetError()}");
        surfaceStruct = Marshal.PtrToStructure<SDL_Surface>(surface);
        Width = surfaceStruct.w;
        Height = surfaceStruct.h;
        SDL_FreeSurface(surface);
        _mTexture = newTexture;
    }

    public void SetColor(SDL_Color color) => SDL_SetTextureColorMod(_mTexture, color.r, color.g, color.b);

    public void SetBlendMode(SDL_BlendMode blendMode) => SDL_SetTextureBlendMode(_mTexture, blendMode);

    public void SetAlpha(byte alpha) => SDL_SetTextureAlphaMod(_mTexture, alpha);

    public void Render(int x, int y, ref SDL_Rect clip)
    {
        var rq = new SDL_Rect { x = x, y = y, w = Width, h = Height };
        SDL_RenderCopy(Program.App.RendererPtr, _mTexture, ref clip, ref rq);
    }

    public void Render(int x, int y)
    {
        var rq = new SDL_Rect { x = x, y = y, w = Width, h = Height };
        var srcRq = new SDL_Rect { x = 0, y = 0, w = Width, h = Height };
        SDL_RenderCopy(Program.App.RendererPtr, _mTexture, ref srcRq, ref rq);
    }


    ~TextureWrapper() => Free();

    private void Free()
    {
        if (_mTexture != IntPtr.Zero)
        {
            SDL_DestroyTexture(_mTexture);
        }
    }
}
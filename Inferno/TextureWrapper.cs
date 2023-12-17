﻿using System.Runtime.InteropServices;
using SDL2;
using static SDL2.SDL;

namespace Inferno;

public class TextureWrapper
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    protected IntPtr _mTexture = IntPtr.Zero;

    public TextureWrapper(string path)
    {
        LoadFromPath(path);
    }

    // For anamated textures
    public TextureWrapper(string path, int width, int height)
    {
        LoadFromPath(path);
        Width = width;
        Height = height;
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

    public virtual void Render(int x, int y)
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

public class StatefulAnimatedTextureWrapper : TextureWrapper
{
    public StatefulAnimatedTextureWrapper(string path, int frameSizePx, int frameCount) : base(path, frameSizePx, frameSizePx)
    {
        _frameSizePx = frameSizePx;
        _srcRect.w = frameSizePx;
        _srcRect.h = frameSizePx;
        FrameCount = frameCount;
    }

    //states go from left to right on the image
    //frames go from top to bottom on the image

    private SDL_Rect _srcRect;
    private readonly int _frameSizePx;

    public int State
    {
        get => _srcRect.x / _frameSizePx;
        set => _srcRect.x = value * _frameSizePx;
    }

    public int Frame
    {
        get => _srcRect.y / _frameSizePx;
        set => _srcRect.y = value * _frameSizePx;
    }

    public int FrameCount { get; private set; }

    public void NextFrame()
    {
        Frame++;
        if (Frame >= FrameCount)
        {
            Frame = 0;
        }
    }

    public override void Render(int x, int y)
    {
        var rq = new SDL_Rect { x = x, y = y, w = Width, h = Height };
        SDL_RenderCopy(Program.App.RendererPtr, _mTexture, ref _srcRect, ref rq);
    }
}
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using SDL2;

namespace MapGenTest;

public class TextureWrapper
{
    public required string TexturePath;
    public IntPtr TexturePtr = IntPtr.Zero;
    public IntPtr Load(IntPtr renderPtr) => TexturePtr = LoadGameAssetTexture(TexturePath, renderPtr);

    private static IntPtr LoadGameAssetTexture(string path, IntPtr renderPtr)
    {
        IntPtr tmpSurfacePtr;
        if ((tmpSurfacePtr = SDL_image.IMG_Load(path)) == IntPtr.Zero) ThrowFailedToLoad(path);
        var s = Marshal.PtrToStructure<SDL.SDL_Surface>(tmpSurfacePtr);
        SDL.SDL_SetColorKey(tmpSurfacePtr, 1, SDL.SDL_MapRGB(s.format, 0xff, 0x66, 0xff));
        var retSurfacePtr = SDL.SDL_CreateTextureFromSurface(renderPtr, tmpSurfacePtr);
        SDL.SDL_FreeSurface(tmpSurfacePtr);
        return retSurfacePtr;
    }

    [DoesNotReturn]
    private static void ThrowFailedToLoad(string textureName) => throw new Exception($"Failed to load {textureName}");
}
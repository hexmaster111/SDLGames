using SDL2;

namespace TinyGui;

internal class FontTextureWrapper : TextureWrapper
{
    public FontTextureWrapper(string path, int fontWidth, int fontHeight) : base(path, fontWidth, fontHeight)
    {
        _frameSizePx = fontWidth;
        _srcRect.w = 8;
        _srcRect.h = 8;
    }
    // //states go from left to right on the image
    //frames go from top to bottom on the image

    private SDL.SDL_Rect _srcRect;
    private readonly int _frameSizePx;

    public void SetFrame(int frame)
    {
        _srcRect.y = frame * _srcRect.w;
    }

    public void SetState(int state)
    {
        _srcRect.x = state * _srcRect.h;
    }


    public override void Render(int x, int y)
    {
        var rq = new SDL.SDL_Rect { x = x, y = y, w = FontWidth, h = FontHeight };
        SDL.SDL_RenderCopy(TinyGuiShared.RendererPtr, MTexture, ref _srcRect, ref rq);
    }
}
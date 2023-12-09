using SDLApplication;
using static SDL2.SDL;

namespace MapGenTest;

internal struct Sprite
{
    public const int GridSpriteImageSize = 32;
    
    private readonly IntPtr _texturePtr;
    private SDL_Rect _rect;

    public Sprite(IntPtr texturePtr, int w, int h)
    {
        _texturePtr = texturePtr;
        _rect.w = w;
        _rect.h = h;
    }


    public void Render(SDL_Point pos, RenderArgs renderArgs)
    {
        _rect.x = pos.x;
        _rect.y = pos.y;
        SDL_RenderCopy(renderArgs.RendererPtr, _texturePtr, IntPtr.Zero, ref _rect);
    }
}
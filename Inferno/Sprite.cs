using SDLApplication;
using static SDL2.SDL;

namespace MapGenTest;

internal class Sprite
{
    public const int GridSpriteImageSize = 32;

    protected readonly IntPtr _texturePtr;
    protected internal SDL_Rect _rect;

    public Sprite(IntPtr texturePtr, int singleFrameWidth, int singleFrameHeight)
    {
        _texturePtr = texturePtr;
        _rect.w = singleFrameWidth;
        _rect.h = singleFrameHeight;
    }


    public virtual void Render(SDL_Point pos, RenderArgs renderArgs)
    {
        _rect.x = pos.x;
        _rect.y = pos.y;
        SDL_RenderCopy(renderArgs.RendererPtr, _texturePtr, IntPtr.Zero, ref _rect);
    }
}
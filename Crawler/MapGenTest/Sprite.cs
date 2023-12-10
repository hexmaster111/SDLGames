using SDLApplication;
using static SDL2.SDL;

namespace MapGenTest;

internal class AnimatedSprite(IntPtr texturePtr, int singleFrameWidth, int singleFrameHeight, int frameCount)
    : Sprite(texturePtr, singleFrameWidth, singleFrameHeight)
{
    private readonly int _frameCount = frameCount;
    private int _currentFrameIndex;

    private SDL_Rect _currentSpriteRect = new()
    {
        x = 0,
        y = 0,
        h = GridSpriteImageSize,
        w = GridSpriteImageSize
    };

    private SimpleTimer _tmr = new(250);

    private void NextFrame()
    {
        _currentSpriteRect.y += GridSpriteImageSize;

        _currentFrameIndex++;
        if (_currentFrameIndex >= _frameCount)
        {
            _currentSpriteRect.y = 0;
            _currentFrameIndex = 0;
        }
    }

    public void Update(long now)
    {
        if (_tmr.Evaluate(now))
        {
            NextFrame();
        }
    }

    public override void Render(SDL_Point pos, RenderArgs renderArgs)
    {
        _rect.x = pos.x;
        _rect.y = pos.y;
        SDL_RenderCopy(renderArgs.RendererPtr, _texturePtr, ref _currentSpriteRect, ref _rect);
    }
}

internal class Sprite
{
    public const int GridSpriteImageSize = 32;

    protected readonly IntPtr _texturePtr;
    protected SDL_Rect _rect;

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
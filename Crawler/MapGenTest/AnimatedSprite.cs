using SDL2;
using SDLApplication;

namespace MapGenTest;

internal class AnimatedSprite(
        IntPtr texturePtr,
        int singleFrameWidth,
        int singleFrameHeight,
        int animationFrameCount)
    : Sprite(
        texturePtr,
        singleFrameWidth,
        singleFrameHeight)
{
    private int _currentFrameIndex;
    public int NextFrameIncAmount = GridSpriteImageSize;

    public SDL.SDL_Rect CurrentSpriteRect = new()
    {
        x = 0,
        y = 0,
        h = GridSpriteImageSize,
        w = GridSpriteImageSize
    };

    private SimpleTimer _tmr = new(250);

    private void NextFrame()
    {
        CurrentSpriteRect.y += NextFrameIncAmount;

        _currentFrameIndex++;
        if (_currentFrameIndex >= animationFrameCount)
        {
            CurrentSpriteRect.y = 0;
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

    public override void Render(SDL.SDL_Point pos, RenderArgs renderArgs)
    {
        _rect.x = pos.x;
        _rect.y = pos.y;
        SDL.SDL_RenderCopy(renderArgs.RendererPtr, _texturePtr, ref CurrentSpriteRect, ref _rect);
    }
}
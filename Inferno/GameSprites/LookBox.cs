using SDLApplication;

namespace Inferno.GameSprites;

public class LookBox : GameObject<StatefulAnimatedTextureWrapper>
{
    public LookBox() : base(Textures.LookBox, "LookBox")
    {
        Solidity = Solidity.Passable;
    }

    private SimpleTimer _textureTimer = new(500);

    public override void Render(int camXPx, int camYPx)
    {
        if (Hide)
        {
            return;
        }

        base.Render(camXPx, camYPx);
    }

    public override void Update(long now)
    {
        if (Hide)
        {
            return;
        }

        if (_textureTimer.Evaluate(now))
        {
            _texture.NextFrame();
        }
    }

    public bool Hide { get; set; }
}
using SDLApplication;

namespace Inferno.GameSprites;

public class Slime() : GameSprite<StatefulAnimatedTextureWrapper>(Textures.EntitySlimeLv0)
{
    private SimpleTimer _textureTimer = new(250);

    public override void Update(long now)
    {
        if (_textureTimer.Evaluate(now))
        {
            _texture.NextFrame();
        }
    }
}
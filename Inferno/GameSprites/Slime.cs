using SDLApplication;

namespace Inferno.GameSprites;

public class Slime : GameObject<StatefulAnimatedTextureWrapper>
{
    private SimpleTimer _textureTimer = new(250);

    public Slime() : base(Textures.EntitySlimeLv0, "Slime")
    {
        Solidity = Solidity.Passable;
    }

    public override void Update(long now)
    {
        if (_textureTimer.Evaluate(now))
        {
            _texture.NextFrame();
        }
    }
}
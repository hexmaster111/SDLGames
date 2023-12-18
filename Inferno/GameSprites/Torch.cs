using SDLApplication;

namespace Inferno.GameSprites;

public class Torch : GameObject<StatefulAnimatedTextureWrapper>
{
    private SimpleTimer _textureTimer = new(250);

    public Torch() : base(Textures.Torch, "Torch")
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
using SDLApplication;

namespace Inferno.GameSprites;

public class Torch : GameObject<StatefulAnimatedTextureWrapper>
{
    private SimpleTimer _textureTimer = new(250);

    public override string Description => "A warm glowing fire, that entrances you, as you watch it flicker";

    public Torch() : base(Textures.Torch, nameof(Torch))
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
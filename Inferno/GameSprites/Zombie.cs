using SDLApplication;

namespace Inferno.GameSprites;

public class Zombie() : GameObject<StatefulAnimatedTextureWrapper>(Textures.EntityZombieLv0)
{
    private SimpleTimer _textureTimer = new(500);

    public override void Update(long now)
    {
        if (_textureTimer.Evaluate(now))
        {
            _texture.NextFrame();
        }
    }
}
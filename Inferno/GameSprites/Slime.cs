using SDLApplication;

namespace Inferno.GameSprites;

public class Slime : GameObject<StatefulAnimatedTextureWrapper>
{
    private readonly SimpleTimer _textureTimer = new(250);
    private readonly SimpleTimer _moveTimer = new(1000);

    private readonly FriendSlimeAi _brain = new();

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


        if (_moveTimer.Evaluate(now))
        {
            
        }
    }
}

public class FriendSlimeAi
{
    //Slime tries to stay within this 
    private const int SHEALD_LINK_RANGE_MIN = 3;
    private const int SHEALD_LINK_RANGE_MAX = 5;
}
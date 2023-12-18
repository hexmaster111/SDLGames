namespace Inferno.GameSprites;

public class Pot : GameObject<TextureWrapper>
{
    public Pot() : base(Textures.Pot)
    {
        Solidity = Solidity.Passable;
    }
};
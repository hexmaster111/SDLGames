namespace Inferno.GameSprites;

public class Pot : GameObject<TextureWrapper>
{
    public Pot() : base(Textures.Pot, "Pot")
    {
        Solidity = Solidity.Passable;
    }
};
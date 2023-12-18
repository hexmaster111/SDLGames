namespace Inferno.GameSprites;

public class WallStone : GameObject<TextureWrapper>
{
    public WallStone() : base(Textures.FloorStone)
    {
        Solidity = Solidity.Solid;
    }
}
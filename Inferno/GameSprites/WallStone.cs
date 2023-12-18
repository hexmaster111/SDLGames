namespace Inferno.GameSprites;

public class WallStone : GameObject<TextureWrapper>
{
    public WallStone() : base(Textures.FloorStone, "Wall")
    {
        Solidity = Solidity.Solid;
    }
}
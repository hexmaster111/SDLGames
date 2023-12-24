namespace Inferno.GameSprites;

public class WallWoodenFence : GameObject<TextureWrapper>
{
    public WallWoodenFence() : base(Textures.WallWoodenFence, "Wall Wooden Fence")
    {
        Solidity = Solidity.Solid;
    }
}
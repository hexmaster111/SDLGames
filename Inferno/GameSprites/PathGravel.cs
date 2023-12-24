namespace Inferno.GameSprites;

public class PathGravel : GameObject<TextureWrapper>
{
    public PathGravel() : base(Textures.PathGravel, "Path Gravel")
    {
        Solidity = Solidity.Passable;
    }
}
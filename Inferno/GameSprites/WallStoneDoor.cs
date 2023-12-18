namespace Inferno.GameSprites;

public class WallStoneDoor : GameObject<StatefulAnimatedTextureWrapper>
{
    public void Open()
    {
        _texture.State = 1;
        Solidity = Solidity.Passable;
    }

    public void Close()
    {
        _texture.State = 0;
        Solidity = Solidity.Solid;
    }

    public WallStoneDoor() : base(Textures.WallStoneDoor)
    {
        Close();
    }
}
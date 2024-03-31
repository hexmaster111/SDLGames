namespace Inferno.GameSprites;

public class WallStoneDoor : GameObject<StatefulAnimatedTextureWrapper>
{
    public override void Open(IGameObject closer)
    {
        _texture.State = 1;
        Solidity = Solidity.Passable;
        CanClose = true;
        CanOpen = false;
    }

    public sealed override void Close(IGameObject closer)
    {
        _texture.State = 0;
        Solidity = Solidity.Solid;
        CanClose = false;
        CanOpen = true;
    }

    public WallStoneDoor() : base(Textures.WallStoneDoor, "Door")
    {
        Close(null!);
    }
}
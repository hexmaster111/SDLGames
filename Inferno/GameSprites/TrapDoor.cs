namespace Inferno.GameSprites;

public sealed class TrapDoor : GameObject<StatefulAnimatedTextureWrapper>
{

    public TrapDoor() : base(Textures.TrapDoor, "Trap Door")
    {
        Solidity = Solidity.Passable;
        Close(null!);
    }

    public override void Open(IGameObject closer)
    {
        _texture.State = 0;
        CanClose = true;
        CanOpen = false;
    }

    public override void Close(IGameObject closer)
    {
        _texture.State = 1;
        CanClose = false;
        CanOpen = true;
    }
}
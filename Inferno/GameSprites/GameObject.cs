namespace Inferno.GameSprites;

public interface IGameObject
{
    int PosXPx { get; set; }
    int PosYPx { get; set; }
    int GridPosX { get; set; }
    int GridPosY { get; set; }
    void Render(int camXPx, int camYPx);
    void Update(long now);
    Solidity Solidity { get; }
}

public enum Solidity
{
    Passable,
    Solid
}

public abstract class GameObject<T> : IGameObject
    where T : TextureWrapper
{
    internal GameObject(T texture)
    {
        _texture = texture;
    }

    internal readonly T _texture;
    public int PosXPx { get; set; }
    public int PosYPx { get; set; }

    public virtual bool CanOpen => false;
    public virtual bool CanClose => false;


    public int GridPosX
    {
        get => PosXPx / Program.TileSizePx;
        set => PosXPx = value * Program.TileSizePx;
    }

    public int GridPosY
    {
        get => PosYPx / Program.TileSizePx;
        set => PosYPx = value * Program.TileSizePx;
    }


    public virtual void Render(int camXPx, int camYPx)
    {
        _texture.Render(PosXPx - camXPx, PosYPx - camYPx);
    }

    public virtual void Update(long now)
    {
    }
    public virtual void Open() => throw new Exception("Cannot open this object");
    public virtual void Close() => throw new Exception("Cannot close this object");

    public Solidity Solidity { get; set; }
}

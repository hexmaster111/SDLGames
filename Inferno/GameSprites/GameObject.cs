namespace Inferno.GameSprites;

public interface IGameObject
{
    public string ObjName { get; set; }
    int PosXPx { get; set; }
    int PosYPx { get; set; }
    int GridPosX { get; set; }
    int GridPosY { get; set; }
    void Render(int camXPx, int camYPx);
    void Update(long now);
    Solidity Solidity { get; }
    bool CanOpen { get; set; }
    bool CanClose { get; }
    void Open();
    void Close();
}

public enum Solidity
{
    Passable,
    Solid
}

public abstract class GameObject<T> : IGameObject
    where T : TextureWrapper
{
    internal GameObject(T texture, string objName)
    {
        _texture = texture;
        ObjName = objName;
    }

    internal readonly T _texture;
    public virtual string ObjName { get; set; }
    public int PosXPx { get; set; }
    public int PosYPx { get; set; }
    public  Solidity Solidity { get; protected set; }

    public virtual bool CanOpen { get; set; } = false;

    public virtual bool CanClose { get; set; } = false;


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

}
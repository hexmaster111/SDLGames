using Newtonsoft.Json;

namespace Inferno.GameSprites;

public interface IGameObject
{
    public string ObjName { get; set; }
    int PosXPx { get; set; }
    int PosYPx { get; set; }
    int X { get; set; }
    int Y { get; set; }
    void Render(int camXPx, int camYPx);
    void Update(long now);
    Solidity Solidity { get; }
    bool CanOpen { get; }
    bool CanClose { get; }
    void Open(IGameObject opener);
    void Close(IGameObject closer);

    string Description { get; }
}

public enum Solidity
{
    Passable,
    Solid
}

public abstract class GameObject<TTextureWrapper> : IGameObject
    where TTextureWrapper : TextureWrapper
{
    internal GameObject(TTextureWrapper texture, string objName)
    {
        _texture = texture;
        ObjName = objName;
    }

    internal readonly TTextureWrapper _texture;
    public virtual string ObjName { get; set; }

    public event Action Moved;

    private int _PosXPx;
    private int _PosYPx;

    public int PosXPx
    {
        get => _PosXPx;
        set
        {
            _PosXPx = value;
            OnMoved();
        }
    }

    public int PosYPx
    {
        get => _PosYPx;
        set
        {
            _PosYPx = value;
            OnMoved();
        }
    }

    public Solidity Solidity { get; protected set; }

    public virtual bool CanOpen { get; set; } = false;

    public virtual bool CanClose { get; set; } = false;


    public int X
    {
        get => PosXPx / Program.TileSizePx;
        set => PosXPx = value * Program.TileSizePx;
    }

    public int Y
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


    public virtual void Open(IGameObject closer) => throw new Exception("Cannot open this object");
    public virtual void Close(IGameObject closer) => throw new Exception("Cannot close this object");
    public virtual string Description => "??something??";

    protected virtual void OnMoved()
    {
        Moved?.Invoke();
    }
}
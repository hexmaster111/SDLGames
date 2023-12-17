namespace Inferno.GameSprites;

public interface IGameSprite
{
    int PosXPx { get; set; }
    int PosYPx { get; set; }
    int GridPosX { get; set; }
    int GridPosY { get; set; }
    void Render(int camXPx, int camYPx);
    void Update(long now);
}

public abstract class GameSprite<T> : IGameSprite
    where T : TextureWrapper
{
    internal GameSprite(T texture)
    {
        _texture = texture;
    }

    internal readonly T _texture;
    public int PosXPx { get; set; }
    public int PosYPx { get; set; }

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
}
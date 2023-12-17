namespace Inferno.GameSprites;

public abstract class GameSprite
{
    internal GameSprite(TextureWrapper texture)
    {
        _texture = texture;
    }

    private readonly TextureWrapper _texture;
    public int PosXPx { get; internal set; }
    public int PosYPx { get; internal set; }

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
}
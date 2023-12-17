namespace BackgroundScrollingTest__DONOTREDISTURBUTE;

public abstract class GameGridSprite
{
    internal GameGridSprite(TextureWrapper texture)
    {
        _texture = texture;
    }

    private readonly TextureWrapper _texture;
    public int PosXPx { get; internal set; }
    public int PosYPx { get; internal set; }

    public virtual void Render(int camXPx, int camYPx)
    {
        _texture.Render(PosXPx - camXPx, PosYPx - camYPx);
    }
}
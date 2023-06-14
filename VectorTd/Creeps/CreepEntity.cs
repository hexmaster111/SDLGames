using SDLApplication;
using VectorTd.Tiles;
using static SDL2.SDL;

namespace VectorTd.Creeps;

public abstract class CreepEntity
{
    public CreepEntity(SDL_Color color, double speed, double health, double maxHealth, int reward)
    {
        Color = color;
        Speed = speed;
        Health = health;
        MaxHealth = maxHealth;
        Reward = reward;
    }

    public const int SizePx = Tile.SizePx / 4;
    public double X { get; private set; }
    public double Y { get; private set; }
    public double Speed { get; private set; }
    public double Health { get; private set; }
    public double MaxHealth { get; private set; }
    public SDL_Color Color { get; private set; }
    public int Reward { get; private set; }

    public virtual void Render(RenderArgs args, ref SDL_Rect viewPort)
    {
        var rect = new SDL_Rect
        {
            x = (int)(X * Tile.SizePx),
            y = (int)(Y * Tile.SizePx),
            w = (int)SizePx,
            h = (int)SizePx
        };

        SDL_RenderSetViewport(args.RendererPtr, ref viewPort);
        args.SetDrawColor(Color);
        SDL_RenderFillRect(args.RendererPtr, ref rect);
    }

    private Tile _currentTile;


    //true to remove
    public bool Update(TimeSpan deltaTime, State state)
    {
        //Debug move right
        X += Speed * deltaTime.TotalSeconds;
        return false;
    }

    public void SetTile(Tile tile)
    {
        _currentTile = tile;

        X = _currentTile.X;
        Y = _currentTile.Y;
    }
}
using SDLApplication;
using VectorTd.Creeps;
using VectorTd.Tiles;
using static SDL2.SDL;

namespace VectorTd.Towers;

public abstract class Projectile
{
    private int Range { get; }
    private int X { get; }
    private int Y { get; }
    private TimeSpan PulseTime { get; }
    private Creep Target { get; }
    private SDL_Color Color { get; }
    private double Damage { get; }
    private DateTime DrawTime { get; }

    public Projectile(int x, int y, Creep target, double damage, int range, SDL_Color color, TimeSpan pulseTime)
    {
        Damage = damage;
        PulseTime = pulseTime;
        X = x;
        Y = y;
        Target = target;
        Color = color;
        Range = range;
        DrawTime = DateTime.Now;
    }

    //True to remove
    public virtual bool Update(TimeSpan __, State state)
    {


        //if the target is out of range, remove the projectile
        var distance = _.Distance(X, Y, Target.MapPosition.X, Target.MapPosition.Y);
        if (distance > Range) return true;

        //if the target is in range, deal damage
        Target.Damage(Damage);

        //if the projectil has been drawn for pulse time, remove it
        if (DateTime.Now - DrawTime > PulseTime) return true;

        return false;
    }


    public virtual void Render(RenderArgs args, ref SDL_Rect viewPort)
    {
        SDL_RenderSetViewport(args.RendererPtr, ref viewPort);
        args.SetDrawColor(Color);
        var towerPxX = X * Tile.SizePx + Tile.SizePx / 2d;
        var towerPxY = Y * Tile.SizePx + Tile.SizePx / 2d;
        var creepPxX = Target.XPx + Creep.SizePx / 2d;
        var creepPxY = Target.YPx + Creep.SizePx / 2d;
        args.DrawLine((int)towerPxX, (int)towerPxY, (int)creepPxX, (int)creepPxY, 5, Color);
    }
}
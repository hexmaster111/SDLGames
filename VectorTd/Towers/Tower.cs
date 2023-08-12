using SDLApplication;
using VectorTd.Creeps;
using VectorTd.Projectiles;
using VectorTd.Tiles;
using static SDL2.SDL;

namespace VectorTd.Towers;

public static class TowerFactory
{

    public static Tower[] RefrenceTowers { get; } =
      {
        new DevSimpleTower(0, 0),
        new DevGodTower(0, 0),
    };

    public enum TowerType
    {
        DEV_Simple,
        DEV_God,
    }

    public static Tower? CreateTower(int x, int y, TowerType type) => type switch
    {
        TowerType.DEV_Simple => new DevSimpleTower(x, y),
        TowerType.DEV_God => new DevGodTower(x, y),
        _ => null
    };
}

public abstract class Tower
{
    public abstract double Damage { get; }
    public abstract int Range { get; }
    public abstract double FireRate { get; }


    public int SizePx { get; }
    public SDL_Color Color { get; }
    public TowerFactory.TowerType Type { get; }

    public int X { get; internal set; }

    public int Y { get; internal set; }

    public Tower(int x, int y, SDL_Color color, int sizePx, TowerFactory.TowerType type)
    {
        Type = type;
        X = x;
        Y = y;
        Color = color;
        SizePx = sizePx;
    }

    public void Render(RenderArgs args, ref SDL_Rect viewPort)
    {
        //render a rectangle
        SDL_RenderSetViewport(args.RendererPtr, ref viewPort);
        args.SetDrawColor(Color);
        var rect = new SDL_Rect
        {
            x = X * Tile.SizePx + Tile.SizePx / 2 - SizePx / 2,
            y = Y * Tile.SizePx + Tile.SizePx / 2 - SizePx / 2,
            w = SizePx,
            h = SizePx
        };
        args.FillRect(rect);
    }

    //If a tower has a range of 1, it can hit all the tiles within 1 tile of it, including diagonals
    protected List<Creep> GetCreepsInRange(State state) => state.Creeps
        .Where(c => c.MapPosition.X >= X - Range && c.MapPosition.X <= X + Range &&
                    c.MapPosition.Y >= Y - Range && c.MapPosition.Y <= Y + Range)
        .ToList();

    private DateTime _lastFireTime = DateTime.MinValue;
    private Creep? _creepBeingShot;

    public virtual void Update(TimeSpan deltaTime, State state)
    {
        var creepsInRange = GetCreepsInRange(state);
        if (creepsInRange.Count == 0) return;
        var creepClosestToEnd = creepsInRange
            .OrderBy(c => c.DistanceTo(state.EndTile!))
            .First();


        if ((DateTime.Now - _lastFireTime).TotalSeconds < FireRate) return;
        _lastFireTime = DateTime.Now;
        _creepBeingShot = creepClosestToEnd;


        var bullet = new LaserProjectile(X, Y, creepClosestToEnd, Damage, Range, Color);
        state.Projectiles.Add(bullet);


        // foreach (var creep in creepsInRange) creep.Color = SdlColors.Red;
        // foreach (var creep in state.Creeps.Except(creepsInRange)) creep.Color = SdlColors.White;
    }



    public abstract string Name { get; }
    public abstract int Cost { get; }

    public void RenderPlacing(RenderArgs args, ref SDL_Rect viewport, int mouseX, int mouseY)
    {
        SDL_RenderSetViewport(args.RendererPtr, ref viewport);
        // args.SetDrawColor(Color);
        args.SetDrawColor(SdlColors.Green);
        var towerCenterX = mouseX - SizePx / 2;
        var towerCenterY = mouseY - SizePx / 2;

        var rect = new SDL_Rect
        {
            x = towerCenterX,
            y = towerCenterY,
            w = SizePx,
            h = SizePx
        };
        args.FillRect(rect);

        //Render the range of the tower
        args.SetDrawColor(Color);
        var rangePx = Range * Tile.SizePx;
        var rangeRect = new SDL_Rect
        {
            x = towerCenterX - rangePx,
            y = towerCenterY - rangePx,
            w = rangePx * 2 + Tile.SizePx / 2,
            h = rangePx * 2 + Tile.SizePx / 2
        };

        args.DrawRect(rangeRect);
    }
}
using SDLApplication;
using VectorTd.Tiles;
using VectorTd.Towers;
using static SDL2.SDL;

namespace VectorTd;

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


    public double Speed;
    public double Health;
    public double MaxHealth;
    public SDL_Color Color;
    public int Reward;

    public virtual void Render(RenderArgs args, ref SDL_Rect viewPort)
    {
    }
}

public class BasicCreep : CreepEntity
{
    public BasicCreep(SDL_Color color, double speed, double health, double maxHealth, int reward) : base(color, speed, health, maxHealth, reward)
    {
    }
}

public class State
{
    public const int MapSize = 15;
    public Tile[,] Map = new Tile[MapSize, MapSize];
    private SDL_Rect _viewPort;

    public State(SDL_Rect viewPort)
    {
        _viewPort = viewPort;
        for (var x = 0; x < MapSize; x++)
        for (var y = 0; y < MapSize; y++)
            Map[x, y] = new VoidTile(x, y);
    }

    public int Money { get; set; }


    public void Render(RenderArgs args)
    {
        foreach (var tile in Map) tile.Render(args, ref _viewPort);
    }

    public void Update(TimeSpan deltaTime)
    {
        foreach (var tile in Map) tile.Update(deltaTime, this);
    }

    public void Click(int clickX, int clickY)
    {
        //Convert x, y to the viewports coordinate system
        clickX -= _viewPort.x;
        clickY -= _viewPort.y;

        //Check if the click is in the viewport
        if (clickX < 0 || clickX > _viewPort.w || clickY < 0 || clickY > _viewPort.h) throw new Exception("Click outside of viewport");

        //convert to grid coordinates
        clickX /= Tile.SizePx;
        clickY /= Tile.SizePx;
        Console.WriteLine($"Clicked State {clickX}, {clickY}");

        if (GlobalState.IsPlacingTower)
        {
            //Check if the tower can be placed
            if (Map[clickX, clickY] is not TowerTile { IsBuildable: true, Tower: null } towerTile) return;
            if (GlobalState.PlacingTower == null) return;

            //Place the tower
            towerTile.Tower = GlobalState.PlacingTower;
            towerTile.Tower = TowerFactory.CreateTower(clickX, clickY, GlobalState.PlacingTower.Type);
            GlobalState.IsPlacingTower = false;
            GlobalState.PlacingTower = null;
        }
    }
}
using SDLApplication;
using VectorTd.Creeps;
using VectorTd.Tiles;
using VectorTd.Towers;
using static SDL2.SDL;

namespace VectorTd;

public class State
{
    public const int MapSize = 15;
    public Tile[,] Map = new Tile[MapSize, MapSize];
    private SDL_Rect _viewPort;
    private List<CreepEntity> _creeps = new List<CreepEntity>();


    public State(SDL_Rect viewPort)
    {
        _viewPort = viewPort;
        for (var x = 0; x < MapSize; x++)
        for (var y = 0; y < MapSize; y++)
            Map[x, y] = new VoidTile(x, y);
    }

    public int Money { get; set; }
    public int Lives { get; set; }


    public void Render(RenderArgs args)
    {
        foreach (var tile in Map) tile.Render(args, ref _viewPort);
        foreach (var creep in _creeps) creep.Render(args, ref _viewPort);
    }

    public void Update(TimeSpan deltaTime)
    {
        foreach (var tile in Map) tile.Update(deltaTime, this);
        var toRemove = new List<CreepEntity>();
        foreach (var creep in _creeps)
            if (creep.Update(deltaTime, this))
                toRemove.Add(creep);
        foreach (var creep in toRemove) RemoveCreep(creep);
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

    private void RemoveCreep(CreepEntity p0)
    {
        _creeps.Remove(p0);
    }

    public void AddCreep(CreepEntity creep)
    {
        var startTile = Map.OfType<StartTile>().First();
        creep.SetTile(startTile);

        _creeps.Add(creep);
    }

    public IEnumerable<Tile> GetTilesAround(Tile thisTile)
    {
        var x = thisTile.X;
        var y = thisTile.Y;
        var tiles = new Tile[4];
        if (x > 0) tiles[0] = Map[x - 1, y];
        if (x < MapSize - 1) tiles[1] = Map[x + 1, y];
        if (y > 0) tiles[2] = Map[x, y - 1];
        if (y < MapSize - 1) tiles[3] = Map[x, y + 1];
        return tiles;
    }
}
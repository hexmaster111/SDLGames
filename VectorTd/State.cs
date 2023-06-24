using System.Collections.ObjectModel;
using SDLApplication;
using VectorTd.Creeps;
using VectorTd.Tiles;
using VectorTd.Towers;
using static SDL2.SDL;

namespace VectorTd;

public record MapWaveData(MapWaveData.WaveData[] Waves)
{
    public record WaveData(int Reward, WaveData.CreepSpawnInfo[] CreepSpawnInfos)
    {
        public record CreepSpawnInfo(CreepType CreepType, TimeSpan WaitAfterSpawn);
    };
};

public class WaveController
{
    public WaveController()
    {
        _mapWaveData = new(new[]
        {
            new MapWaveData.WaveData(100, new[]
            {
                new MapWaveData.WaveData.CreepSpawnInfo(CreepType.Small, TimeSpan.Zero)
            }),
            new MapWaveData.WaveData(100, new[]
            {
                new MapWaveData.WaveData.CreepSpawnInfo(CreepType.Small, TimeSpan.FromSeconds(1)),
                new MapWaveData.WaveData.CreepSpawnInfo(CreepType.Small, TimeSpan.Zero)
            }),
        });
    }

    private MapWaveData _mapWaveData;
    
    public void Update(TimeSpan deltaTime, State state)
    {
    }

    public void StartWave()
    {
    }
}

public class State
{
    public const int MapSize = 15;
    public Tile[,] Map = new Tile[MapSize, MapSize];
    private SDL_Rect _viewPort;
    private readonly List<Creep> _creeps = new();
    private readonly object _creepsLock = new();
    public readonly List<Projectile> Projectiles = new();
    public readonly object _projectilesLock = new();
    public WaveController WaveController { get; } = new();

    public IEnumerable<Creep> Creeps
    {
        get
        {
            lock (_creepsLock)
            {
                return _creeps.AsReadOnly();
            }
        }
    }


    public Tile? StartTile => _.GetFirstItemOfType<Tile, StartTile>(Map);
    public Tile? EndTile => _.GetFirstItemOfType<Tile, EndTile>(Map);


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
        lock (_creepsLock)
        {
            foreach (var creep in _creeps)
                creep.Render(args, ref _viewPort);
        }

        lock (_projectilesLock)
        {
            foreach (var projectile in Projectiles) projectile.Render(args, ref _viewPort);
        }
    }

    public void Update(TimeSpan deltaTime)
    {
        foreach (var tile in Map) tile.Update(deltaTime, this);
        var toRemove = new List<Creep>();
        lock (_creepsLock)
        {
            foreach (var creep in _creeps)
                if (creep.Update(deltaTime, this))
                    toRemove.Add(creep);
            foreach (var creep in toRemove) _creeps.Remove(creep);
        }

        lock (_projectilesLock)
        {
            var projectialsToRemove = new List<Projectile>();
            foreach (var projectile in Projectiles)
                if (projectile.Update(deltaTime, this))
                    projectialsToRemove.Add(projectile);

            foreach (var projectile in projectialsToRemove) Projectiles.Remove(projectile);
        }

        lock (_creepsLock) WaveController.Update(deltaTime, this);
    }

    public void Click(int clickX, int clickY)
    {
        //Convert x, y to the viewports coordinate system
        clickX -= _viewPort.x;
        clickY -= _viewPort.y;

        //Check if the click is in the viewport
        if (clickX < 0 || clickX > _viewPort.w || clickY < 0 || clickY > _viewPort.h)
            throw new Exception("Click outside of viewport");

        //convert to grid coordinates
        clickX /= Tile.SizePx;
        clickY /= Tile.SizePx;
        Console.WriteLine($"Clicked State {clickX}, {clickY}");

        if (GlobalState.IsPlacingTower)
        {
            //Check if the tower can be placed
            if (Map[clickX, clickY] is not TowerTile { Type: TileType.Tower, Tower: null } towerTile) return;
            if (GlobalState.PlacingTower == null) return;

            //Place the tower
            towerTile.Tower = GlobalState.PlacingTower;
            towerTile.Tower = TowerFactory.CreateTower(clickX, clickY, GlobalState.PlacingTower.Type);
            GlobalState.IsPlacingTower = false;
            GlobalState.PlacingTower = null;
        }
    }


    public void AddCreep(Creep creep) => _.Lock(_creepsLock, () =>
    {
        creep.SetTile(StartTile!);
        _creeps.Add(creep);
    });
}
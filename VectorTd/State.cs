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

    public static MapWaveData? Load(string[] waveDataLines)
    {
        //Remove all comments, empty lines, Anything after a # including the #, and trim whitespace
        waveDataLines = waveDataLines
            .Where(line => !line.StartsWith("#") && !string.IsNullOrWhiteSpace(line))
            .Select(line => line.Split("#")[0])
            .Select(line => line.Trim())
            .ToArray();


        // From lines StartWave to EndWave is a wave def, so first we create a list of all the lines that are in a wave def

        var waveDefs = new List<string[]>();
        var waveDefLines = new List<string>();
        var inWaveDef = false;
        foreach (var line in waveDataLines)
        {
            if (line == "START_WAVE")
            {
                inWaveDef = true;
                continue;
            }

            if (line == "END_WAVE")
            {
                inWaveDef = false;
                waveDefs.Add(waveDefLines.ToArray());
                waveDefLines.Clear();
                continue;
            }

            if (inWaveDef)
            {
                waveDefLines.Add(line);
            }
        }

        // now that we have waveDefs we can parse them individually

        // START_WAVE
        // REWARD:100
        // START_WI
        // SPAWN S, 1, 5
        // SPAWN S,1
        // END_WI
        // END_WAVE

        var waves = new List<WaveData>();


        foreach (var waveLine in waveDefs)
        {
            var rewardLine = waveLine.FirstOrDefault(line => line.StartsWith("REWARD:"));
            if (rewardLine == null) throw new Exception($"Wave def missing REWARD line \n{waveLine}");
            //Split the line on the : and take the second part, then parse it as an int
            if (!int.TryParse(rewardLine.Split(":")[1], out var reward))
                throw new Exception($"Wave def REWARD line is not a valid int \n{waveLine}");

            var creepSpawnInfos = new List<WaveData.CreepSpawnInfo>();
            var creepSpawnInfoLines = new List<string>();
            var inCreepSpawnInfo = false;
            foreach (var line in waveLine)
            {
                if (line == "START_WI")
                {
                    inCreepSpawnInfo = true;
                    continue;
                }

                if (line == "END_WI")
                {
                    inCreepSpawnInfo = false;
                    creepSpawnInfos.AddRange(ParseCreepSpawnInfo(creepSpawnInfoLines.ToArray()));
                    creepSpawnInfoLines.Clear();
                    waves.Add(new WaveData(reward, creepSpawnInfos.ToArray()));
                    continue;
                }

                if (inCreepSpawnInfo)
                {
                    creepSpawnInfoLines.Add(line);
                }
            }
        }

        return new MapWaveData(waves.ToArray());
    }

    private static WaveData.CreepSpawnInfo[] ParseCreepSpawnInfo(string[] toArray)
    {
        var creepSpawnInfos = new List<WaveData.CreepSpawnInfo>();
        foreach (var line in toArray)
        {
            //split on space and camas
            var split = line.Split(',', ' ');
            if (split.Length != 4 && split.Length != 3)
                throw new Exception($"CreepSpawnInfo line is not valid \n{line}");
            var creepType = split[1].Trim() switch
            {
                "S" => CreepType.Small,
                "M" => CreepType.Medium,
                "L" => CreepType.Large,
                _ => throw new Exception($"CreepSpawnInfo line is not valid \n{line}")
            };

            if (!int.TryParse(split[2].Trim(), out var count))
                throw new Exception($"CreepSpawnInfo line is not valid \n{line}");


            int waitAfterSpawn = 0;

            if (split.Length == 4)
                if (!int.TryParse(split[3].Trim(), out waitAfterSpawn))
                    throw new Exception($"CreepSpawnInfo line is not valid \n{line}");

            var timeParsed = TimeSpan.FromSeconds(waitAfterSpawn);

            creepSpawnInfos.Add(new WaveData.CreepSpawnInfo(creepType, timeParsed));
        }

        return creepSpawnInfos.ToArray();
    }
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

    private bool _didStartWaveGetCalled;
    private bool _waveStarted;
    private int _currentWaveIndex;
    private int _currentCreepSpawnInfoIndex;
    private TimeSpan _timeSinceLastCreepSpawn;
    private TimeSpan _timeSinceLastWave;

    const int WaveGracePeriod = 5;

    public void Update(TimeSpan deltaTime, State state)
    {
        if (!_didStartWaveGetCalled) return;
        if (!_waveStarted)
        {
            _timeSinceLastWave += deltaTime;
            if (_timeSinceLastWave >= TimeSpan.FromSeconds(WaveGracePeriod))
            {
                _waveStarted = true;
                _timeSinceLastWave = TimeSpan.Zero;
            }

            return;
        }

        _timeSinceLastCreepSpawn += deltaTime;

        if (_currentWaveIndex >= _mapWaveData.Waves.Length)
        {
            Console.WriteLine("No more waves resetting");
            _currentWaveIndex = 0;
            _waveStarted = false;
            return;
        }

        if (_timeSinceLastCreepSpawn >= _mapWaveData.Waves[_currentWaveIndex].CreepSpawnInfos[_currentCreepSpawnInfoIndex].WaitAfterSpawn)
        {
            Console.WriteLine(
                $"WAVE[{_currentWaveIndex}] Spawned creep {_currentCreepSpawnInfoIndex} next spawn in {_mapWaveData.Waves[_currentWaveIndex].CreepSpawnInfos[_currentCreepSpawnInfoIndex].WaitAfterSpawn}");
            _timeSinceLastCreepSpawn = TimeSpan.Zero;
            Creep creep = _mapWaveData.Waves[_currentWaveIndex].CreepSpawnInfos[_currentCreepSpawnInfoIndex].CreepType switch
            {
                CreepType.Small => new SmallCreep(),
                CreepType.Medium => new MediumCreep(),
                _ => throw new ArgumentOutOfRangeException()
            };
            state.AddCreep(creep);
            _currentCreepSpawnInfoIndex++;
            if (_currentCreepSpawnInfoIndex >= _mapWaveData.Waves[_currentWaveIndex].CreepSpawnInfos.Length)
            {
                _currentCreepSpawnInfoIndex = 0;
                _currentWaveIndex++;
                _waveStarted = false;
            }
        }
    }

    public void StartWave()
    {
        Console.WriteLine("StartWave");
        _didStartWaveGetCalled = true;
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
    private readonly object _projectilesLock = new();
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
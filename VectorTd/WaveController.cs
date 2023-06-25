using SDL2;
using SDLApplication;
using VectorTd.Creeps;

namespace VectorTd;

public class WaveController
{
    public WaveController(MapWaveData mapWaveData) => _mapWaveData = mapWaveData;

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

        var c = _mapWaveData.Waves[_currentWaveIndex].CreepSpawnInfos[_currentCreepSpawnInfoIndex];
        if (_timeSinceLastCreepSpawn >= c.WaitAfterSpawn)
        {
            Console.WriteLine($"WAVE[{_currentWaveIndex}] Spawned creep {_currentCreepSpawnInfoIndex} - {c.WaitAfterSpawn}");
            _timeSinceLastCreepSpawn = TimeSpan.Zero;
            var creep = CreepFactory.Create(c.CreepType);
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
using VectorTd.Creeps;

namespace VectorTd.Tiles;

public static class TileLoader
{
    const int MapSize = 15;

    public static (string readErr, Tile[,]?, MapWaveData?) LoadVMap(string path)
    {
        var lines = File.ReadAllLines(path).Take(MapSize).ToArray();
        if (lines.Length != MapSize) return ("Map file is not the correct size", null, null);
        var tilesFromFile = LoadTiles(lines).ToList();
        if (tilesFromFile.Count != State.MapSize * State.MapSize)
            return ($"Map size mismatch Got {tilesFromFile.Count}, expected 225", null, null);
        var tiles = new Tile[State.MapSize, State.MapSize];
        foreach (var tile in tilesFromFile) tiles[tile.X, tile.Y] = tile;

        var waveLines = File.ReadAllLines(path).Skip(MapSize).ToArray();
        var waveInstructions = LoadWaveInstructions(waveLines);
        //TODO: Load wave instructions and pass them out

        if (!String.IsNullOrEmpty(waveInstructions.error)) return (waveInstructions.error, null, null);
        return (string.Empty, tiles, new MapWaveData(waveInstructions.instructions));
    }

    public readonly struct MapWaveData
    {
        public readonly Wave[] Instructions;
        public MapWaveData(Wave[] instructions)
        {
            Instructions = instructions;
        }

    }

    public readonly struct Wave
    {

        public Wave(int reward, WaveInstruction[] instructions)
        {
            Reward = reward;
            Instructions = instructions;
        }

        public readonly int Reward;
        public readonly WaveInstruction[] Instructions;
    }

    public readonly struct WaveInstruction
    {
        public WaveInstruction(string instruction)
        {
            Instruction = instruction;
        }
        public readonly string Instruction;
    }


    private static (string error, Wave[] instructions) LoadWaveInstructions(string[] fileLines)
    {

        // ##### MAP ENEMY TYPES #####
        // # S = Small Enemy
        // # M = Medium Enemy
        // # L = Large Enemy
        // ###########################

        // # Wave 1
        // START_WAVE
        //     REWARD:100                  # reward for completing the wave
        //     START_WI
        //         SPAWN S,1               # spawn 1 small enemie
        //         WAIT 5                  # wait 5 seconds
        //         SPAWN S,1               # spawn 1 small enemie
        //     END_WI
        // END_WAVE

        // START_WAVE
        //     REWARD:110                  
        //     START_WI
        //         SPAWN S,2               
        //     END_WI
        // END_WAVE






        return (string.Empty, Array.Empty<Wave>());
    }





    private static IEnumerable<Tile> LoadTiles(string[] lines)
    {
        var y = 0;
        var x = 0;

        foreach (var line in lines)
        {
            foreach (var ch in line)
            {
                yield return ch switch
                {
                    '#' => TileFactory.Create(TileType.Void, x, y),
                    'P' => TileFactory.Create(TileType.Path, x, y),
                    'T' => TileFactory.Create(TileType.Tower, x, y),
                    'S' => TileFactory.Create(TileType.Start, x, y),
                    'E' => TileFactory.Create(TileType.End, x, y),
                    _ => throw new ArgumentOutOfRangeException(nameof(ch), ch, null)
                };
                x++;
            }

            x = 0;
            y++;
        }
    }
}
using VectorTd.Creeps;

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

            if (!double.TryParse(split[2].Trim(), out var count))
                throw new Exception($"CreepSpawnInfo line is not valid \n{line}");


            var waitAfterSpawn = 0.00;

            if (split.Length == 4)
                if (!double.TryParse(split[3].Trim(), out waitAfterSpawn))
                    throw new Exception($"CreepSpawnInfo line is not valid \n{line}");

            var timeParsed = TimeSpan.FromSeconds(waitAfterSpawn);

            creepSpawnInfos.Add(new WaveData.CreepSpawnInfo(creepType, timeParsed));
        }

        return creepSpawnInfos.ToArray();
    }
};
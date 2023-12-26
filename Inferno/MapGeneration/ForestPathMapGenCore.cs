using TinyGui.UiElements;

namespace Inferno.MapGeneration;

//******** HEY YOU DEV, HEADS UP, THIS IS A MADNESS FILE, IF YOU WANNA KNOW HOW IT WORKS, REALLY, THATS BETWEEN ME 
// AND GOD, GO CHECK OUT IMapGenCore FOR MORE DETAILS ON HOW YOU GET TO USE THIS MADNESS

internal class ForestPathMapGenCore : IMapGenCore
{
    //Basic rules are as fallows for forest path generation:
    //    First tile is the start of the path
    //    Path must be connected in cardinal direction

    public int Seed { get; } = DateTime.Now.Millisecond;
    public int Width { get; }
    public int Height { get; }
    public int[,] CurrentMap { get; }
    public int NextEvalPtX { get; private set; }
    public int NextEvalPtY { get; private set; }
    readonly Random _rand;

    private Point P1Connection { get; set; }
    private Point P2Connection { get; set; }


    private readonly Queue<Point> _tilePointsToGenerate = new();

    public ForestPathMapGenCore(int width,
        int height,
        int pathConnectLeft = -1,
        int pathConnectRight = -1,
        int pathConnectTop = -1,
        int pathConnectBottom = -1,
        int seed = 0
    )
    {
        if (seed == 0) seed = DateTime.Now.Millisecond;
        _rand = new Random(seed);
        Width = width;
        Height = height;
        CurrentMap = new int[Width, Height];

        bool gotP1 = false;
        bool gotP2 = false;

        if (pathConnectLeft != -1)
        {
            CurrentMap[0, pathConnectLeft] = (int)TileKind.Gravel;
            _tilePointsToGenerate.Enqueue(new Point(0 + 1, pathConnectLeft));
            var p = new Point(0, pathConnectLeft);
            if (!gotP1)
            {
                P1Connection = p;
                gotP1 = true;
            }
            else if (!gotP2)
            {
                P2Connection = p;
                gotP2 = true;
            }
            else throw new Exception("Too many connections");
        }

        if (pathConnectRight != -1)
        {
            CurrentMap[Width - 1, pathConnectRight] = (int)TileKind.Gravel;
            _tilePointsToGenerate.Enqueue(new Point(Width - 1 + 1, pathConnectRight));
            var p = new Point(Width - 1, pathConnectRight);
            if (!gotP1)
            {
                P1Connection = p;
                gotP1 = true;
            }
            else if (!gotP2)
            {
                P2Connection = p;
                gotP2 = true;
            }
            else throw new Exception("Too many connections");
        }

        if (pathConnectTop != -1)
        {
            CurrentMap[pathConnectTop, 0] = (int)TileKind.Gravel;
            _tilePointsToGenerate.Enqueue(new Point(pathConnectTop, 0 + 1));
            var p = new Point(pathConnectTop, 0);
            if (!gotP1)
            {
                P1Connection = p;
                gotP1 = true;
            }
            else if (!gotP2)
            {
                P2Connection = p;
                gotP2 = true;
            }
            else throw new Exception("Too many connections");
        }

        if (pathConnectBottom != -1)
        {
            CurrentMap[pathConnectBottom, Height - 1] = (int)TileKind.Gravel;
            _tilePointsToGenerate.Enqueue(new Point(pathConnectBottom, Height - 1 + 1));
            var p = new Point(pathConnectBottom, Height - 1);
            if (!gotP1)
            {
                P1Connection = p;
                gotP1 = true;
            }
            else if (!gotP2)
            {
                P2Connection = p;
                gotP2 = true;
            }
            else throw new Exception("Too many connections");
        }

        if (!gotP1 || !gotP2) throw new Exception("Not enough connections");


        //Seed trees
        for (int i = 0; i < (Width * Height) / 25; i++)
        {
            var x = _rand.Next(0, Width);
            var y = _rand.Next(0, Height);
            CurrentMap[x, y] = (int)TileKind.Wall;
        }

        //seed start points
        for (int i = 0; i < 100; i++)
        {
            var x = _rand.Next(0, Width);
            var y = _rand.Next(0, Height);
            _tilePointsToGenerate.Enqueue(new Point(x, y));
        }


        //draw a line with a random chance of turning
        for (int i = 0; i < 2; i++)
        {
            var x = _rand.Next(0, Width);
            var y = _rand.Next(0, Height);
            CurrentMap[x, y] = (int)TileKind.Gravel;
            while (true)
            {
                var dir = _rand.Next(0, 4);
                switch (dir)
                {
                    case 0:
                        x++;
                        break;
                    case 1:
                        x--;
                        break;
                    case 2:
                        y++;
                        break;
                    case 3:
                        y--;
                        break;
                }

                if (x < 0 || x >= Width || y < 0 || y >= Height) break;
                CurrentMap[x, y] = (int)TileKind.Gravel;
                if (_rand.NextDouble() < 0.1) break;
            }
        }
    }


    private TileKind GetTileAt(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height) return TileKind.Wall;
        return (TileKind)CurrentMap[x, y];
    }

    private void SetTileAt(Point pos, TileKind tile) => SetTileAt(pos.X, pos.Y, tile);

    private void SetTileAt(int x, int y, TileKind tileKind)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height) return;
        CurrentMap[x, y] = (int)tileKind;
    }

    enum GenStep
    {
        InitialGen,
        PathConnection,
        Done
    }

    private GenStep CurrentGenStep { get; set; }

    private bool TakeGenerationStep(GenStep step)
    {
        var stepDone = step switch
        {
            GenStep.InitialGen => InitialGenStep(),
            GenStep.PathConnection => PathConnectionStep(),
            GenStep.Done => true,
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null)
        };

        if (stepDone)
            CurrentGenStep = step switch
            {
                GenStep.InitialGen => GenStep.PathConnection,
                GenStep.PathConnection => GenStep.Done,

                GenStep.Done => GenStep.Done,
                _ => throw new ArgumentOutOfRangeException(nameof(step), step, null)
            };

        return step == GenStep.Done;
    }

    private bool PathConnectionStep()
    {
        //highlight points in the map where there are more then 8 path tiles around it

        var pathTiles = new List<Point>();
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                int tile = CurrentMap[x, y];
                if (tile == (int)TileKind.Gravel)
                {
                    var neighbors = new TileKind[8];
                    for (int i = 0; i < 8; i++)
                    {
                        var sx = i % 3 - 1;
                        var sy = i / 3 - 1;
                        neighbors[i] = GetTileAt(x + sx, y + sy);
                    }

                    if (neighbors.Count(x => x == TileKind.Gravel) > 5)
                        pathTiles.Add(new Point(x, y));
                }
            }
        }

        return false;
    }


    public string GetDebugString()
    {
        return $"dbg_p1 = {dbg_p1}\n" +
               $"dbg_p2 = {dbg_p2}\n" +
               $"CurrentGenStep = {CurrentGenStep}\n" +
               $"_tilePointsToGenerate.Count = {_tilePointsToGenerate.Count}\n"
            ;
    }

    private int dbg_p1 = 0;
    private int dbg_p2 = 0;

    private bool InitialGenStep()
    {
        if (_tilePointsToGenerate.Count == 0) return true;

        var pos = _tilePointsToGenerate.Dequeue();

        NextEvalPtX = pos.X; // set so we know on the early bail
        NextEvalPtY = pos.Y;

        if (pos.X < 0 || pos.X >= Width || pos.Y < 0 || pos.Y >= Height) return false; //out of bounds

        //if the tile has already been realized, or is off the grid, skip it
        if (CurrentMap[pos.X, pos.Y] != (int)TileKind.Unrealized || pos.X < 0 || pos.X >= Width || pos.Y < 0 ||
            pos.Y >= Height)
        {
            return false;
        }

        var extNeighbors = new TileKind[16];
        for (int i = 0; i < 16; i++)
        {
            var x = i % 4 - 1;
            var y = i / 4 - 1;
            extNeighbors[i] = GetTileAt(pos.X + x, pos.Y + y);
        }


        var neighbors = new TileKind[8];
        for (int i = 0; i < 8; i++)
        {
            var x = i % 3 - 1;
            var y = i / 3 - 1;
            neighbors[i] = GetTileAt(pos.X + x, pos.Y + y);
        }

        bool oneOnlyPathDirTrue = neighbors.Count(x => x == TileKind.Gravel) == 1;


        if (oneOnlyPathDirTrue)
        {
            if (extNeighbors.Count(x => x == TileKind.Gravel) > 1)
            {
                SetTileAt(pos, Rand(0.5) ? TileKind.Gravel : TileKind.Ground);
                dbg_p1++;
                goto AddNeighbors;
            }

            dbg_p2++;

            SetTileAt(pos, TileKind.Gravel);
            goto AddNeighbors;
        }


        if (extNeighbors.Any(x => x == TileKind.Wall))
        {
            //To be a wall, you must already have a wall as a neighbor
            var randomTileType = Rand(4) ? TileKind.Ground : TileKind.Wall;
            SetTileAt(pos, randomTileType);
        }
        else
        {
            //To be ground, you must have at least one ground neighbor
            SetTileAt(pos, TileKind.Ground);
        }


        AddNeighbors:
        //Add the tiles around to the queue
        if (neighbors[1] == TileKind.Unrealized) _tilePointsToGenerate.Enqueue(new Point(pos.X, pos.Y - 1));
        if (neighbors[6] == TileKind.Unrealized) _tilePointsToGenerate.Enqueue(new Point(pos.X, pos.Y + 1));
        if (neighbors[2] == TileKind.Unrealized) _tilePointsToGenerate.Enqueue(new Point(pos.X + 1, pos.Y - 1));
        if (neighbors[5] == TileKind.Unrealized) _tilePointsToGenerate.Enqueue(new Point(pos.X - 1, pos.Y + 1));
        if (neighbors[3] == TileKind.Unrealized) _tilePointsToGenerate.Enqueue(new Point(pos.X - 1, pos.Y));
        if (neighbors[7] == TileKind.Unrealized) _tilePointsToGenerate.Enqueue(new Point(pos.X + 1, pos.Y + 1));
        if (neighbors[0] == TileKind.Unrealized) _tilePointsToGenerate.Enqueue(new Point(pos.X - 1, pos.Y - 1));
        if (neighbors[4] == TileKind.Unrealized) _tilePointsToGenerate.Enqueue(new Point(pos.X + 1, pos.Y));


        if (_tilePointsToGenerate.TryPeek(out var nextPt))
        {
            NextEvalPtX = nextPt.X;
            NextEvalPtY = nextPt.Y;
        }

        return _tilePointsToGenerate.Count == 0;
    }

    //true for done
    public bool TakeGenerationStep() => TakeGenerationStep(CurrentGenStep);

    private TileKind GetTileAt(Point pos) => GetTileAt(pos.X, pos.Y);


    private bool Rand(double chance) => _rand.NextDouble() < chance;


    private class Point(int x, int y)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
    }

    public enum TileKind : int
    {
        DebugHighlight = -1,
        Unrealized = 0,
        Gravel = 1,
        Ground = 2,
        Wall = 3,
    }
}
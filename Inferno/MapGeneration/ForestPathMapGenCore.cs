using TinyGui.UiElements;

namespace Inferno.MapGeneration;

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


    private readonly Queue<Point> _tilePointsToGenerate = new();

    public ForestPathMapGenCore(int width, int height, int seed = 0)
    {
        if (seed == 0) seed = DateTime.Now.Millisecond;
        Width = width;
        Height = height;
        CurrentMap = new int[Width, Height];
        _rand = new Random(seed);
        _tilePointsToGenerate.Enqueue(new Point(3, _rand.Next(Height / 2, Height - 2)));
        _tilePointsToGenerate.Enqueue(new Point(Width - 3, _rand.Next(Height / 2, Height - 2)));
        _tilePointsToGenerate.Enqueue(new Point(Width / 2, _rand.Next(Height / 2, Height - 2)));
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

    public bool TakeGenerationStep() //true for done
    {
        if (_tilePointsToGenerate.Count == 0) return true;

        var pos = _tilePointsToGenerate.Dequeue();

        var neighbors = new TileKind[8];
        NextEvalPtX = pos.X; // set so we know on the early bail
        NextEvalPtY = pos.Y;

        if (pos.X < 0 || pos.X >= Width || pos.Y < 0 || pos.Y >= Height) return false; //out of bounds

        //if the tile has already been realized, or is off the grid, skip it
        if (CurrentMap[pos.X, pos.Y] != (int)TileKind.Unrealized || pos.X < 0 || pos.X >= Width || pos.Y < 0 ||
            pos.Y >= Height) return false;

        //if the tile is on the edge of the map, make it a wall
        if (pos.X == 0 || pos.X == Width - 1 || pos.Y == 0 || pos.Y == Height - 1)
        {
            CurrentMap[pos.X, pos.Y] = (int)TileKind.Wall;
            goto AddNeighbors;
        }

        neighbors[0] = GetTileAt(pos.X - 1, pos.Y - 1);
        neighbors[1] = GetTileAt(pos.X, pos.Y - 1);
        neighbors[2] = GetTileAt(pos.X + 1, pos.Y - 1);
        neighbors[3] = GetTileAt(pos.X - 1, pos.Y);
        neighbors[4] = GetTileAt(pos.X + 1, pos.Y);
        neighbors[5] = GetTileAt(pos.X - 1, pos.Y + 1);
        neighbors[6] = GetTileAt(pos.X, pos.Y + 1);
        neighbors[7] = GetTileAt(pos.X + 1, pos.Y + 1);

        if (neighbors.All(x => x == TileKind.Unrealized))
        {
            SetTileAt(pos, Rand(2) ? TileKind.Gravel : TileKind.Wall);
            goto AddNeighbors;
        }


        if (neighbors.All(x => x == TileKind.Ground))
        {
            SetTileAt(pos, TileKind.Wall);
            goto AddNeighbors;
        }

        //Paths can only be connected by cardinal directions
        var northPath = neighbors[1] == TileKind.Gravel;
        var southPath = neighbors[6] == TileKind.Gravel;
        var eastPath = neighbors[4] == TileKind.Gravel;
        var westPath = neighbors[3] == TileKind.Gravel;
        var couldBePath = northPath || southPath || eastPath || westPath;

        if (couldBePath && Rand(2))
        {
            SetTileAt(pos, TileKind.Gravel);
            goto AddNeighbors;
        }


        if (neighbors.Any(x => x == TileKind.Wall))
        {
            //To be a wall, you must already have a wall as a neighbor
            var randomTileType = Rand(2) ? TileKind.Ground : TileKind.Wall;
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

    private TileKind GetTileAt(Point pos) => GetTileAt(pos.X, pos.Y);


    private bool Rand(int chance) => _rand.Next(0, chance) == 0;


    private class Point(int x, int y)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
    }

    public enum TileKind : int
    {
        Unrealized = 0,
        Gravel = 1,
        Ground = 2,
        Wall = 3
    }
}
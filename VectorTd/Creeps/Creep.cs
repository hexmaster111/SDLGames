using System.Diagnostics;
using SDLApplication;
using VectorTd.Tiles;
using static SDL2.SDL;

namespace VectorTd.Creeps;

public abstract class Creep
{
    public Creep(SDL_Color color, double speed, double health, double maxHealth, int reward)
    {
        Color = color;
        Speed = speed;
        Health = health;
        MaxHealth = maxHealth;
        Reward = reward;
    }

    public const int SizePx = Tile.SizePx / 4;
    public double XPx { get; private set; }
    public double YPx { get; private set; }

    //This should center the creep on the tile
    public (int X, int Y) MapPosition
    {
        get => ((int)(XPx + SizePx / 2d) / Tile.SizePx, (int)(YPx + SizePx / 2d) / Tile.SizePx);
        set => (XPx, YPx) = (value.X * Tile.SizePx + Tile.SizePx / 2d - SizePx / 2d, value.Y * Tile.SizePx + Tile.SizePx / 2d - SizePx / 2d);
    }

    public double Speed { get; private set; }
    public double Health { get; private set; }
    public double MaxHealth { get; private set; }
    public SDL_Color Color { get; internal set; }
    public int Reward { get; private set; }

    public virtual void Render(RenderArgs args, ref SDL_Rect viewPort)
    {
        var rect = new SDL_Rect
        {
            x = (int)(XPx),
            y = (int)(YPx),
            w = SizePx,
            h = SizePx
        };

        SDL_RenderSetViewport(args.RendererPtr, ref viewPort);
        args.SetDrawColor(Color);
        SDL_RenderFillRect(args.RendererPtr, ref rect);
    }


    private static Tile? GetNextTile(IEnumerable<Tile?> tiles, State state)
    {
        var t = tiles
            .Where(t => t != null)
            .Cast<Tile>()
            .OrderBy(t => t.DistanceTo(state.EndTile!))
            .Where(t => t.Type is TileType.Path or TileType.End);

        var tls = t.ToArray();
        if (!tls.Any()) return null;

        if (tls.Length >= 1)
        {
            var r = new Random();
            var i = r.Next(0, tls.Length);

            var tile = tls[i];
            var dir = tile.Direction;
            if (dir != Direction.NotSet)
            {
                var (x, y) = (tile.X, tile.Y);
                var nextTile = _.InArrayOrNull(state.Map, x + dir.XAddr(), y + dir.YAdder());
                if (nextTile != null && nextTile.Type is TileType.Path or TileType.End)
                {
                    return nextTile;
                }
            }

            return tile;
        }

        return tls.First();
    }


    private readonly List<Tile> _visitedTiles = new();
    private Tile? _currentTargetTile;
    bool _isCentering = false;

    //true to remove
    public virtual bool Update(TimeSpan __, State state)
    {
        if (Health <= 0)
        {
            state.Money += Reward;
            return true;
        }

        const double deltaTime = 1d / 60;
        if (_isCentering)
        {
            //center the creep on the tile smoothly at the speed of the creep
            var (dx, dy) = (XPx - _currentTargetTile!.PxPosCenter.x, YPx - _currentTargetTile!.PxPosCenter.y);

            //make dx and dy be between -1 and 1
            dx /= Math.Abs(dx);
            dy /= Math.Abs(dy);


            XPx -= dx * deltaTime * Speed;
            YPx -= dy * deltaTime * Speed;


            var isXCentered = Math.Abs(XPx - _currentTargetTile.PxPosCenter.x) < 1;
            var isYCentered = Math.Abs(YPx - _currentTargetTile.PxPosCenter.y) < 1;

            if (isXCentered && isYCentered)
            {
                _isCentering = false;
                _currentTargetTile = null;
            }

            return false;
        }

        var tilesAround = new List<Tile?>
        {
            _.InArrayOrNull(state.Map, MapPosition.X - 1, MapPosition.Y),
            _.InArrayOrNull(state.Map, MapPosition.X + 1, MapPosition.Y),
            _.InArrayOrNull(state.Map, MapPosition.X, MapPosition.Y - 1),
            _.InArrayOrNull(state.Map, MapPosition.X, MapPosition.Y + 1)
        };


        //change the color of the tiles around to red
        // tilesAround.ForEach(t => t?.ChangeColor(SdlColors.Red));

        if (_currentTargetTile == null)
        {
            //filter out the tiles we have already visited from tiles around
            tilesAround = tilesAround.Where(t => t != null && !_visitedTiles.Contains(t)).ToList();
            _currentTargetTile = GetNextTile(tilesAround, state);
            if (_currentTargetTile == null)
            {
                return true;
            }
        }

        if (_currentTargetTile == null)
        {
            Debug.WriteLine("No path found");
            return true;
        }

        if (_currentTargetTile.Type == TileType.End)
        {
            Debug.WriteLine("Reached end");
            return true;
        }

        if (_currentTargetTile != null)
        {
            //change the color of the target tile to green
            // _currentTargetTile.ChangeColor(SdlColors.Green);
            var (tx, ty) = (_currentTargetTile.X - MapPosition.X, _currentTargetTile.Y - MapPosition.Y);

            if (tx == 0 && ty == 0)
            {
                _isCentering = true;

                //Add to visited tiles
                _visitedTiles.Add(_currentTargetTile);

                return false;
            }

            //Move speed amount of pixels in the direction of the target tile untel we reach the center of the tile
            var (dx, dy) = (tx * Speed * deltaTime, ty * Speed * deltaTime);
            XPx += dx;
            YPx += dy;
        }

        return false;
    }


    public void SetTile(Tile tile)
    {
        MapPosition = (tile.X, tile.Y);
    }

    //in map units, counting from movalbe path tiles
    public int DistanceTo(Tile tile)
    {
        //count how many tiles we have to go through to get to the target tile
        var (tx, ty) = (tile.X - MapPosition.X, tile.Y - MapPosition.Y);
        return Math.Abs(tx) + Math.Abs(ty);
    }

    public void Damage(double damage) => Health -= damage;
}
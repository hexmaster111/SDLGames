using System.Diagnostics;
using SDLApplication;
using VectorTd.Tiles;
using static SDL2.SDL;

namespace VectorTd.Creeps;

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

    public const int SizePx = Tile.SizePx / 4;
    public double XPx { get; private set; }
    public double YPx { get; private set; }

    //This should center the creep on the tile
    public (int X, int Y) MapPosition
    {
        get => ((int)(XPx + SizePx / 2d) / Tile.SizePx,
            (int)(YPx + SizePx / 2d) / Tile.SizePx);

        set => (XPx, YPx) = (value.X * Tile.SizePx + Tile.SizePx / 2d - SizePx / 2d,
            value.Y * Tile.SizePx + Tile.SizePx / 2d - SizePx / 2d);
    }

    public double Speed { get; private set; }
    public double Health { get; private set; }
    public double MaxHealth { get; private set; }
    public SDL_Color Color { get; private set; }
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


    private static Tile? GetNextTile(IEnumerable<Tile?> tiles, State state) => tiles
        .Where(t => t != null)
        .Cast<Tile>()
        .OrderBy(t => t.DistanceTo(state.EndTile!))
        .FirstOrDefault(t => t.Type == TileType.Path);

    private bool _isMovingInX;
    private bool _isMovingInY;

    private List<(int x, int y)> _visitedTiles = new();

    //true to remove
    public virtual bool Update(TimeSpan deltaTime, State state)
    {
        var tilesAround = new List<Tile?>
        {
            _.InArrayOrNull(state.Map, MapPosition.X - 1, MapPosition.Y),
            _.InArrayOrNull(state.Map, MapPosition.X + 1, MapPosition.Y),
            _.InArrayOrNull(state.Map, MapPosition.X, MapPosition.Y - 1),
            _.InArrayOrNull(state.Map, MapPosition.X, MapPosition.Y + 1)
        };


        //change the color of the tiles around to red
        tilesAround.ForEach(t => t?.ChangeColor(SdlColors.Red));


        var nextTile = GetNextTile(tilesAround, state);
        //Write out the tiles x and y and type all on one line
        Console.Write($"Tiles around: {string.Join(", ", tilesAround.Select(t => t == null ? "null" : $"{t.X}, {t.Y}, {t.Type}"))}");
        Console.WriteLine($" | X: {XPx:00.00}, Y: {YPx:00.00}, MapX: {MapPosition.X}, MapY: {MapPosition.Y} | Next tile: {nextTile?.X}, {nextTile?.Y}");

        while (nextTile != null && _visitedTiles.Contains((nextTile.X, nextTile.Y)))
        {
            tilesAround.Remove(nextTile);
            nextTile = GetNextTile(tilesAround, state);
        }

        if (nextTile == null)
        {
            // Console.WriteLine($"Dying due to no next tile {XPx}, {YPx}");
            // return true;
            return false;
        }


        //Now we have the next tile, we need to move towards at our speed, we dont need to center in the axis we are moving in,
        //as we want a smooth movement


        if (_isMovingInX == false && _isMovingInY == false)
        {
            _isMovingInX = nextTile.X != MapPosition.X;
            _isMovingInY = nextTile.Y != MapPosition.Y;
            Debug.Assert(_isMovingInX || _isMovingInY);
            Debug.Assert(!(_isMovingInX && _isMovingInY));
        }


        //We want the creep to move in the center of the tiles, in one axis at a time, smoothly
        //So we need to move in the axis we are moving in, until we are in the center of the next tile
        var preMoveTile = (MapPosition.X, MapPosition.Y);
        if (_isMovingInX) XPx += Speed * deltaTime.TotalSeconds * (nextTile.X > MapPosition.X ? 1 : -1);
        if (_isMovingInY) YPx += Speed * deltaTime.TotalSeconds * (nextTile.Y > MapPosition.Y ? 1 : -1);

        //Now we should check if we are in a different tile, if we are, we save the old tile, so we dont go back to it
        var currentTile = (MapPosition.X, MapPosition.Y);

        if (currentTile != preMoveTile && _visitedTiles.Contains(currentTile) == false)
        {
            _visitedTiles.Add(preMoveTile);
            //Set the color of the tiles around to green
            tilesAround.ForEach(t => t?.ChangeColor(SdlColors.Green));
        }


        return false;
    }

    public void SetTile(Tile tile)
    {
        MapPosition = (tile.X, tile.Y);
    }
}
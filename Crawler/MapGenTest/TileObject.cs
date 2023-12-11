using SDL2;

namespace MapGenTest;

internal class TileObject
{
    public SDL.SDL_Point Point;
    public required Sprite Sprite;


    public void Update(long now)
    {
        if (Sprite is AnimatedSprite animatedSprite) animatedSprite.Update(now);
    }

    public void Move(Direction direction, int spaces)
    {
        switch (direction)
        {
            case Direction.S:
                Point.y += spaces;
                break;

            case Direction.N:
                Point.y -= spaces;
                break;

            case Direction.E:
                Point.x -= spaces;
                break;

            case Direction.W:
                Point.x += spaces;
                break;

            case Direction.NE:
                Move(Direction.N, spaces);
                Move(Direction.E, spaces);
                break;

            case Direction.NW:
                Move(Direction.N, spaces);
                Move(Direction.W, spaces);
                break;

            case Direction.SE:
                Move(Direction.S, spaces);
                Move(Direction.E, spaces);
                break;

            case Direction.SW:
                Move(Direction.S, spaces);
                Move(Direction.W, spaces);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
}
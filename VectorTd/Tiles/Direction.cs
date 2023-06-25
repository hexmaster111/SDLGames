namespace VectorTd.Tiles;

public enum Direction
{
    NotSet = 0,
    Left,
    Right,
    Up,
    Down
}

public static class DirectionExtensions
{
    public static Direction GetDirection(int x, int y, int x2, int y2)
    {
        if (x == x2 && y == y2) return Direction.NotSet;
        if (x == x2) return y > y2 ? Direction.Up : Direction.Down;
        return x > x2 ? Direction.Left : Direction.Right;
    }

    public static int XAddr(this Direction direction) => direction switch
    {
        Direction.Left => -1,
        Direction.Right => 1,
        _ => 0
    };

    public static int YAdder(this Direction direction) => direction switch
    {
        Direction.Up => -1,
        Direction.Down => 1,
        _ => 0
    };
}
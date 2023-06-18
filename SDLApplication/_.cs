namespace VectorTd;

public static class _
{
    public static TWanted? GetItemOfType<TBase, TWanted>(TBase[,] items)
    {
        foreach (var item in items)
        {
            if (item is TWanted wanted) return wanted;
        }

        return default;
    }

    public static T? InArrayOrNull<T>(T[,] array, int x, int y)
    {
        if (x < 0 || y < 0 || x >= array.GetLength(0) || y >= array.GetLength(1))
            return default;
        return array[x, y];
    }


    public static void Lock(object obj, Action action)
    {
        lock (obj)
        {
            action();
        }
    }

    public static int Distance(int x, int y, int x2, int y2)
    {
        return (int)Math.Sqrt(Math.Pow(x - x2, 2) + Math.Pow(y - y2, 2));
    }
}
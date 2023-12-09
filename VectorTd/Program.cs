using VectorTd;

internal class Program
{
    public static void Main(string[] args)
    {
        Game game = new("maps\\Basic.vmap");
        game.App.Run();
    }
}

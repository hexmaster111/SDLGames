using VectorTd;

internal class Program
{
    public static void Main(string[] args)
    {
        Game game = new(Path.Combine("Maps", "Basic.vmap"));
        game.App.Run();
    }
}

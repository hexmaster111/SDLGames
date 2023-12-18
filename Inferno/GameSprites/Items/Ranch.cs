namespace Inferno.GameSprites.Items;

public sealed class Ranch : Item
{
    public Ranch() : base(Textures.ItemRanch)
    {
        IsDrinkable = true;
    }

    public override string Name => "Ranch";

    public override void Drink(Player p)
    {
        p.Inventory.Remove(this);
        Console.WriteLine("You drank the ranch, TODO: Do effect to player");
    }
}
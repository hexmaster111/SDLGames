namespace Inferno.GameSprites.Items;

public class LesserManaPotion() : Item(Textures.ItemLesserManaPotion)
{
    public override string Name => "Lesser Mana Potion";
    public override bool IsDrinkable { get; set; } = true;

    public override void Drink(Player p)
    {
        Console.WriteLine($"{p.Name} drinks a {Name}");
        p.Inventory.Remove(this);
    }
}
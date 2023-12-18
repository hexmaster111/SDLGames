namespace Inferno.GameSprites.Items;

public class LesserHealingPotion() : Item(Textures.ItemLesserHealingPotion)
{
    public override string Name => "Lesser Healing Potion";
    public override bool IsDrinkable { get; set; } = true;

    public override void Drink(Player p)
    {
        Console.WriteLine($"{p.Name} drinks a {Name}");
        p.Inventory.Remove(this);
    }
}
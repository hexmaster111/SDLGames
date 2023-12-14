namespace MapGenTest;

public class Inventory(int size)
{
    public int Capacity { get; } = size;
    public List<Item> Items { get; } = new();

    public PlayerArmorSlots ArmorSlots { get; } = new();
}

public class PlayerArmorSlots
{
    public Item Head { get; set; }
    public Item Chest { get; set; }
    public Item Legs { get; set; }
    public Item Feet { get; set; }
    public Item Hands { get; set; }
    public Item LeftHand { get; set; }
    public Item RightHand { get; set; }
}

public struct Item
{
    public ItemModifier Modifier { get; init; }
    public GameObjectType Type { get; init; }
}

public enum ItemModifier
{
    Normal,
    Zesty
}

public class Player
{
    public Player(string name)
    {
        Name = name;
        Inventory = new Inventory(10);
    }

    public string Name { get; set; }
    public Inventory Inventory { get; set; }
    public PlayerStats Stats { get; set; } = new();
}

public struct PlayerStats
{
    /*
     * Strength
     * Dexterity
     * Constitution     - health/healthyness    :: Rezistance to status effects
     * Intelligent      - Book knolage - magic - math
     * Wisdom           - Streat smarts stuff and stabiltiy of mind (application of mind)
     * Rizz             - How well you can chat people up
     *
     */
    public int HitPoints;
    public int ManaPoints;
    public int Level;
    public int Experience;

    public int PhysicalArmorClass;
    public int MagicArmorClass;

    public int Dexterity;
    public int Strength;
    public int Arcane;
    public int Wisdom;
    public int Rizz;
}

public class State
{
    public Player Player = new("D505");

    public KeyboardInputLocation KeyboardInputFocus;

    public enum KeyboardInputLocation
    {
        Game,
        Inventory
    }
}
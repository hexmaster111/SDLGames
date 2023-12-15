namespace MapGenTest;

public class PlayerInventory(int cap)
{
    public int Capacity { get; } = cap;

    /// <summary>
    ///     Items in the players bag
    /// </summary>
    public List<Item> Items { get; } = new();

    /// <summary>
    ///     Places the player will have there armor
    /// </summary>
    public PlayerBodySlots<Item> ArmorSlots { get; } = new();

    public void DropItem(int itemIndex, State state)
    {
        var item = Items[itemIndex];
        var tile = Game.Assets.NewTile(item.Type);
        tile.Point = Game.Inst._player.Point;
        Game.Inst.TileObjects.Add(tile);
        state.Player.PlayerInventory.Items.RemoveAt(itemIndex);
    }
}

public class PlayerBodySlots<T> where T : struct
{
    public T Head { get; set; }
    public T Chest { get; set; }
    public T Legs { get; set; }
    public T Hands { get; set; }
    public T Feet { get; set; }
}

public struct Item
{
    public required ItemModifier Modifier { get; init; }
    public required GameObjectType Type { get; init; }
    public required ItemType ItemType { get; init; }
    public required ItemEquitablePositions EquitablePositions { get; init; }
}

[Flags]
public enum ItemEquitablePositions
{
    NoWhere = 1 << 0,
    Head = 1 << 1,
    Chest = 1 << 2,
    Legs = 1 << 3,
    Feet = 1 << 4,
    Hands = 1 << 5,
}

public enum ItemType
{
    Weapon,
    Armor,
    Potion
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
        PlayerInventory = new PlayerInventory(10);
    }

    public string Name { get; set; }
    public PlayerInventory PlayerInventory { get; set; }
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
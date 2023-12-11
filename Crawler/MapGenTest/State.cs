namespace MapGenTest;

public class Inventory(string name, int size)
{

    public string Name { get; } = name;
    public int Size { get; } = size;
    public List<Item> Items { get; } = [];
}


public struct Item
{
    public ItemType Type { get; init; }
}

public enum ItemType { Stick, Dagger, ShortSward }


public class State
{
    public KeyboardInputLocation KeyboardInputFocus;
    public Inventory PlayerInventory = new("Player", 10);

    public enum KeyboardInputLocation
    {
        Game, Inventory
    }
}
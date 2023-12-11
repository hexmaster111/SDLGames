namespace MapGenTest;

public class Inventory
{
    public required string Name;

}


public class Item
{
    public enum ItemType { Stick, Dagger, ShortSward }
}


public struct State
{
    public KeyboardInputLocation KeyboardInputFocus { get; set; }


    public enum KeyboardInputLocation
    {
        Game, Inventory
    }
}
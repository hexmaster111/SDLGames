namespace Inferno.GameFramework;

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


}
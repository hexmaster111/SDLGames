using Inferno.GameSprites.Items;
using static SDL2.SDL;

namespace Inferno.GameSprites;

public class Player(string name) : GameObject<TextureWrapper>(Textures.Player)
{
    public string Name { get; set; } = name;
    public readonly List<Item> Inventory = new();

    public void AddItemToInventory(Item item)
    {
        item.IsInInventory = true;
        Inventory.Add(item);
    }
    
}
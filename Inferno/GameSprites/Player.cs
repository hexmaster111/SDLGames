using Inferno.GameFramework;

namespace Inferno.GameSprites;

public class Player(string name) : GameObject<TextureWrapper>(Textures.Player)
{
    public string Name { get; set; } = name;
    public PlayerInventory PlayerInventory { get; set; } = new(10);
    public PlayerStats Stats { get; set; } = new();
}
namespace Inferno.GameSprites;

public class Player(string name) : GameSprite<TextureWrapper>(Textures.Player)
{
    public string Name { get; set; } = name;
    public PlayerInventory PlayerInventory { get; set; } = new(10);
    public PlayerStats Stats { get; set; } = new();
}
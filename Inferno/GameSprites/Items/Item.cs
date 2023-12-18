namespace Inferno.GameSprites.Items;

public abstract class Item(TextureWrapper texture) : GameObject<TextureWrapper>(texture)
{
    private static void ThrowHelper()
    {
        throw new Exception($"Item type did not implement this method");
    }

    public virtual bool IsInInventory { get; set; }
    public abstract string Name { get; }
    public virtual bool IsDrinkable { get; set; }
    public virtual bool IsEquip { get; set; } //Can be equipped
    public virtual bool IsThrowable { get; set; }
    public virtual bool IsUsable { get; set; }

    public virtual void Equip(Player p) => ThrowHelper();
    public virtual void Drink(Player p) => ThrowHelper();
    public virtual void Throw(Player p) => ThrowHelper();
    public virtual void Use(Player p) => ThrowHelper();

    public void Drop(Player player)
    {
        player.Inventory.Remove(this);
        IsInInventory = false;
        Program.AddSprite(this, player.GridPosX, player.GridPosY);
    }
}
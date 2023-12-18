namespace Inferno;

internal static class Textures
{
    internal static TextureWrapper Player => new("Assets/PLAYER.png");
    internal static TextureWrapper Pot => new("Assets/CONTAINOR_POT.png");
    internal static StatefulAnimatedTextureWrapper Torch => new("Assets/TORCH.png", 32, 5);
    internal static TextureWrapper FloorStone => new("Assets/WALL_STONE.png");
    internal static StatefulAnimatedTextureWrapper LookBox => new("Assets/LOOK_BOX.png", 32, 2);
    internal static StatefulAnimatedTextureWrapper WallStoneDoor => new("Assets/WALL_STONE_DOOR.png", 32, 1);
    internal static StatefulAnimatedTextureWrapper EntitySlimeLv0 => new("Assets/ENTITY_SLIME_LV0.png", 32, 4);
    internal static StatefulAnimatedTextureWrapper EntityZombieLv0 => new("Assets/ENTITY_ZOMBIE_LV0.png", 32, 2);
    internal static StatefulAnimatedTextureWrapper ContainerChestWood => new("Assets/CONTAINOR_CHEST_WOOD.png", 32, 1);
    internal static TextureWrapper ItemStick => new("Assets/ITEMS/STICK.png");
    internal static TextureWrapper ItemDagger => new("Assets/ITEMS/DAGGER.png");
    internal static TextureWrapper ItemShortSward => new("Assets/ITEMS/SHORTSWORD.png");
    internal static TextureWrapper ItemRanch => new("Assets/ITEMS/RANCH.png");
    internal static TextureWrapper ItemLesserHealingPotion => new("Assets/ITEMS/LESSER_HEALING_POTION.png");
    internal static TextureWrapper ItemLesserManaPotion => new("Assets/ITEMS/LESSER_MANA_POTION.png");
}
namespace Inferno;

internal static class Textures
{
    internal static TextureWrapper Player => new("Assets/GFX/PLAYER.png");
    internal static TextureWrapper DevTexture => new("Assets/GFX/DEV_TEXTURE.png");

    //Game UI elements
    internal static StatefulAnimatedTextureWrapper LookBox => new("Assets/GFX/LOOK_BOX.png", 32, 2);


    //Decoration
    internal static StatefulAnimatedTextureWrapper Torch => new("Assets/GFX/TORCH.png", 32, 5);
    internal static TextureWrapper BgForest => new("Assets/GFX/BG_FOREST.png");


    //Enemies
    internal static StatefulAnimatedTextureWrapper EntitySlimeLv0 => new("Assets/GFX/ENTITY_SLIME_LV0.png", 32, 4);
    internal static StatefulAnimatedTextureWrapper EntityZombieLv0 => new("Assets/GFX/ENTITY_ZOMBIE_LV0.png", 32, 2);

    //Chests/Continors
    internal static TextureWrapper Pot => new("Assets/GFX/CONTAINOR_POT.png");

    internal static StatefulAnimatedTextureWrapper ContainerChestWood =>
        new("Assets/GFX/CONTAINOR_CHEST_WOOD.png", 32, 1);

    //Wall/Floor/Doors/Path
    internal static TextureWrapper FloorStone => new("Assets/GFX/WALL_STONE.png");
    internal static StatefulAnimatedTextureWrapper WallStoneDoor => new("Assets/GFX/WALL_STONE_DOOR.png", 32, 1);
    internal static TextureWrapper WallTrees => new("Assets/GFX/WALL_TREES.png");
    internal static TextureWrapper WallWoodenFence => new("Assets/GFX/WALL_WOODEN_FENCE.png");
    internal static TextureWrapper PathGravel => new("Assets/GFX/PATH_GRAVEL.png");
    internal static StatefulAnimatedTextureWrapper TrapDoor => new("Assets/GFX/TRAP_DOOR.png", 32, 1);

    //Items!

    internal static TextureWrapper ItemStick => new("Assets/GFX/ITEMS/STICK.png");
    internal static TextureWrapper ItemDagger => new("Assets/GFX/ITEMS/DAGGER.png");
    internal static TextureWrapper ItemShortSward => new("Assets/GFX/ITEMS/SHORTSWORD.png");
    internal static TextureWrapper ItemRanch => new("Assets/GFX/ITEMS/RANCH.png");
    internal static TextureWrapper ItemLesserHealingPotion => new("Assets/GFX/ITEMS/LESSER_HEALING_POTION.png");
    internal static TextureWrapper ItemLesserManaPotion => new("Assets/GFX/ITEMS/LESSER_MANA_POTION.png");


    //Hot reloading logic 
    public static event Action? ReloadFromDiskEvent;

    public static void ReloadFromDisk()
    {
        OnReloadFromDiskEvent();
    }

    private static void OnReloadFromDiskEvent()
    {
        ReloadFromDiskEvent?.Invoke();
    }
}
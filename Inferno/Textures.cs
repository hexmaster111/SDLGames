namespace Inferno;

internal static class Textures
{
    internal static readonly TextureWrapper Player = new("Assets/PLAYER.png");
    internal static readonly TextureWrapper Pot = new("Assets/CONTAINOR_POT.png");
    internal static readonly StatefulAnimatedTextureWrapper Torch = new("Assets/TORCH.png", 32, 5);
    internal static readonly TextureWrapper FloorStone = new("Assets/WALL_STONE.png");
    internal static readonly StatefulAnimatedTextureWrapper WallStoneDoor = new("Assets/WALL_STONE_DOOR.png", 32, 1);
    internal static readonly StatefulAnimatedTextureWrapper EntitySlimeLv0 = new("Assets/ENTITY_SLIME_LV0.png", 32, 4);
    internal static readonly StatefulAnimatedTextureWrapper EntityZombieLv0 = new("Assets/ENTITY_ZOMBIE_LV0.png", 32 , 2);
    internal static readonly StatefulAnimatedTextureWrapper ContainerChestWood = new("Assets/CONTAINOR_CHEST_WOOD.png", 32, 1);
    internal static readonly TextureWrapper ItemStick = new("Assets/ITEMS/STICK.png");
    internal static readonly TextureWrapper ItemDagger = new("Assets/ITEMS/DAGGER.png");
    internal static readonly TextureWrapper ItemShortSward = new("Assets/ITEMS/SHORTSWORD.png");
    internal static readonly TextureWrapper ItemRanch = new("Assets/ITEMS/RANCH.png");
}
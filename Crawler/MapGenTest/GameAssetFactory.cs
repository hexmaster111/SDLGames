using SDL2;

namespace MapGenTest;

public class GameAssetFactory
{
    private readonly TextureWrapper _playerTexture = new() { TexturePath = "Assets/PLAYER.png" };
    private readonly TextureWrapper _torchTexture = new() { TexturePath = "Assets/TORCH.png" };
    private readonly TextureWrapper _floorStoneTexture = new() { TexturePath = "Assets/WALL_STONE.png" };
    private readonly TextureWrapper _wallStoneDoorTexture = new() { TexturePath = "Assets/WALL_STONE_DOOR.png" };
    private readonly TextureWrapper _entitySlimeLv0 = new() { TexturePath = "Assets/ENTITY_SLIME_LV0.png" };
    private readonly TextureWrapper _entityZombieLv0 = new() { TexturePath = "Assets/ENTITY_ZOMBIE_LV0.png" };
    private readonly TextureWrapper _containerPot = new() { TexturePath = "Assets/CONTAINOR_POT.png" };
    private readonly TextureWrapper _containerChestWood = new() { TexturePath = "Assets/CONTAINOR_CHEST_WOOD.png" };
    private readonly TextureWrapper _itemStick = new() { TexturePath = "Assets/ITEMS/STICK.png" };
    private readonly TextureWrapper _itemDagger = new() { TexturePath = "Assets/ITEMS/DAGGER.png" };
    private readonly TextureWrapper _itemShortSward = new() { TexturePath = "Assets/ITEMS/SHORTSWORD.png" };
    private readonly TextureWrapper _itemRanch = new() { TexturePath = "Assets/ITEMS/RANCH.png" };
    public static readonly TextureWrapper FontWrapper = new() { TexturePath = "Assets/FONT.png" };


    private int GridAssetWidth => (int)(32 * Game.GameScaleWidth);
    private int GridAssetHeight => (int)(32 * Game.GameScaleHeight);


    internal GameAssetFactory()
    {
    }

    public void LoadTextures(IntPtr rendererPtr)
    {
        _playerTexture.Load(rendererPtr);
        _torchTexture.Load(rendererPtr);
        _floorStoneTexture.Load(rendererPtr);
        _wallStoneDoorTexture.Load(rendererPtr);
        _entitySlimeLv0.Load(rendererPtr);
        _entityZombieLv0.Load(rendererPtr);
        _containerPot.Load(rendererPtr);
        _containerChestWood.Load(rendererPtr);
        _itemStick.Load(rendererPtr);
        _itemDagger.Load(rendererPtr);
        _itemShortSward.Load(rendererPtr);
        _itemRanch.Load(rendererPtr);
        FontWrapper.Load(rendererPtr);
    }

    internal Sprite NewSprite(GameObjectType t) => t switch
    {
        GameObjectType.Player =>
            new Sprite(_playerTexture.TexturePtr, GridAssetWidth, GridAssetHeight),
        GameObjectType.Torch =>
            new AnimatedSprite(_torchTexture.TexturePtr, GridAssetWidth, GridAssetHeight, 5),
        GameObjectType.WallStone =>
            new Sprite(_floorStoneTexture.TexturePtr, GridAssetWidth, GridAssetHeight),
        GameObjectType.WallStoneDoor =>
            new StatefulSprite(_wallStoneDoorTexture.TexturePtr, GridAssetWidth, GridAssetHeight, 2, 1),
        GameObjectType.EntitySlimeLv0 =>
            new AnimatedSprite(_entitySlimeLv0.TexturePtr, GridAssetWidth, GridAssetHeight, 4),
        GameObjectType.EntityZombieLv0 =>
            new AnimatedSprite(_entityZombieLv0.TexturePtr, GridAssetWidth, GridAssetHeight, 2),
        GameObjectType.ContainerPot =>
            new Sprite(_containerPot.TexturePtr, GridAssetWidth, GridAssetHeight),
        GameObjectType.ContainerChestWood =>
            new StatefulSprite(_containerChestWood.TexturePtr, GridAssetWidth, GridAssetHeight, 2, 1),
        GameObjectType.Stick => new Sprite(_itemStick.TexturePtr, GridAssetWidth, GridAssetHeight),
        GameObjectType.Dagger => new Sprite(_itemDagger.TexturePtr, GridAssetWidth, GridAssetHeight),
        GameObjectType.ShortSward => new Sprite(_itemShortSward.TexturePtr, GridAssetWidth, GridAssetHeight),
        GameObjectType.Ranch => new Sprite(_itemRanch.TexturePtr, GridAssetWidth, GridAssetHeight),

        _ => throw new ArgumentOutOfRangeException()
    };


    internal TileObject NewTile(GameObjectType newObjectType, object? arg1 = null)
    {
        var sprite = NewSprite(newObjectType);

        if (newObjectType == GameObjectType.Dagger || newObjectType == GameObjectType.ShortSward ||
            newObjectType == GameObjectType.Stick || newObjectType == GameObjectType.Ranch)
        {
            if (arg1 is not Item item) throw new ArgumentNullException(nameof(arg1) + " is not an Item");
            return new ItemTileObject
            {
                Sprite = sprite,
                Type = newObjectType,
                Point = new SDL.SDL_Point(),
                Item = item
            };
        }

        return new TileObject
        {
            Sprite = sprite,
            Type = newObjectType,
            Point = new SDL.SDL_Point()
        };
    }
}
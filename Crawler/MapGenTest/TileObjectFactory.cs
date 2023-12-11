namespace MapGenTest;

public class TileObjectFactory
{
    private readonly TextureWrapper _playerTexture = new() { TexturePath = "Assets/PLAYER.png" };
    private readonly TextureWrapper _torchTexture = new() { TexturePath = "Assets/TORCH.png" };
    private readonly TextureWrapper _floorStoneTexture = new() { TexturePath = "Assets/WALL_STONE.png" };
    private readonly TextureWrapper _wallStoneDoorTexture = new() { TexturePath = "Assets/WALL_STONE_DOOR.png" };
    private readonly TextureWrapper _entitySlimeLv0 = new() { TexturePath = "Assets/ENTITY_SLIME_LV0.png" };
    private readonly TextureWrapper _entityZombieLv0 = new() { TexturePath = "Assets/ENTITY_ZOMBIE_LV0.png" };
    private readonly TextureWrapper _containerPot = new() { TexturePath = "Assets/CONTAINOR_POT.png" };
    private readonly TextureWrapper _containerChestWood = new() { TexturePath = "Assets/CONTAINOR_CHEST_WOOD.png" };


    private int GridAssetWidth => (int)(32 * _game.GameScaleWidth);
    private int GridAssetHeight => (int)(32 * _game.GameScaleHeight);

    private readonly Game _game;

    internal TileObjectFactory(Game game) => _game = game;

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
    }

    internal TileObject NewTile(GameObjectType newObjectType) => new()
    {
        Sprite = newObjectType switch
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
            _ => throw new ArgumentOutOfRangeException(nameof(newObjectType), newObjectType, null)
        }
    };
}
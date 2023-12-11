using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using static SDL2.SDL;
using static SDL2.SDL_image;

namespace MapGenTest;

public class TextureWrapper
{
    public required string TexturePath;
    public IntPtr TexturePtr = IntPtr.Zero;
    public IntPtr Load(IntPtr renderPtr) => TexturePtr = LoadGameAssetTexture(TexturePath, renderPtr);

    private static IntPtr LoadGameAssetTexture(string path, IntPtr renderPtr)
    {
        IntPtr tmpSurfacePtr;
        if ((tmpSurfacePtr = IMG_Load(path)) == IntPtr.Zero) ThrowFailedToLoad(path);
        var s = Marshal.PtrToStructure<SDL_Surface>(tmpSurfacePtr);
        SDL_SetColorKey(tmpSurfacePtr, 1, SDL_MapRGB(s.format, 0xff, 0x66, 0xff));
        var retSurfacePtr = SDL_CreateTextureFromSurface(renderPtr, tmpSurfacePtr);
        SDL_FreeSurface(tmpSurfacePtr);
        return retSurfacePtr;
    }

    [DoesNotReturn]
    private static void ThrowFailedToLoad(string textureName) => throw new Exception($"Failed to load {textureName}");
}

public class GameObjectFactory
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

    internal GameObjectFactory(Game game) => _game = game;

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

    internal Sprite NewSprite(GameObjectType newObjectType) => newObjectType switch
    {
        GameObjectType.Player => new Sprite(_playerTexture.TexturePtr, GridAssetWidth, GridAssetHeight),
        GameObjectType.Torch => new AnimatedSprite(_torchTexture.TexturePtr, GridAssetWidth, GridAssetHeight, 5),
        GameObjectType.WallStone => new Sprite(_floorStoneTexture.TexturePtr, GridAssetWidth, GridAssetHeight),
        GameObjectType.WallStoneDoor =>
            new StatefulSprite(_wallStoneDoorTexture.TexturePtr, GridAssetWidth, GridAssetHeight, 2, 1),
        GameObjectType.EntitySlimeLv0 => new AnimatedSprite(_entitySlimeLv0.TexturePtr, GridAssetWidth, GridAssetHeight,
            4),
        GameObjectType.EntityZombieLv0 => new AnimatedSprite(_entityZombieLv0.TexturePtr, GridAssetWidth,
            GridAssetHeight, 2),
        GameObjectType.ContainerPot => new Sprite(_containerPot.TexturePtr, GridAssetWidth, GridAssetHeight),
        GameObjectType.ContainerChestWood =>
            new StatefulSprite(_containerChestWood.TexturePtr, GridAssetWidth, GridAssetHeight, 2, 1),
        _ => throw new ArgumentOutOfRangeException(nameof(newObjectType), newObjectType, null)
    };
}

public enum GameObjectType
{
    Player,
    Torch,
    WallStone,
    WallStoneDoor,
    EntitySlimeLv0,
    EntityZombieLv0,
    ContainerPot,
    ContainerChestWood
}
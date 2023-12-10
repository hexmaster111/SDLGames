using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using static SDL2.SDL;
using static SDL2.SDL_image;

namespace MapGenTest;

public class GameAssetFactory
{
    private IntPtr _playerTexturePtr = IntPtr.Zero;
    private IntPtr _torchTexturePtr = IntPtr.Zero;
    private IntPtr _floorStoneTexturePtr = IntPtr.Zero;
    private int GridAssetWidth => (int)(32 * _game.GameScaleWidth);
    private int GridAssetHeight => (int)(32 * _game.GameScaleHeight);

    private Game _game;

    internal GameAssetFactory(Game game)
    {
        _game = game;
    }

    [DoesNotReturn]
    private static void ThrowFailedToLoad(string textureName) => throw new Exception($"Failed to load {textureName}");

    public void LoadTextures(IntPtr rendererPtr)
    {
        _playerTexturePtr = LoadGameAssetTexture("Assets/PLAYER.png", rendererPtr);
        _torchTexturePtr = LoadGameAssetTexture("Assets/TORCH.png", rendererPtr);
        _floorStoneTexturePtr = LoadGameAssetTexture("Assets/FLOOR_STONE.png", rendererPtr);
    }


    public static IntPtr LoadGameAssetTexture(string path, IntPtr renderPtr)
    {
        IntPtr tmpSurfacePtr;
        if ((tmpSurfacePtr = IMG_Load(path)) == IntPtr.Zero) ThrowFailedToLoad(path);
        var s = Marshal.PtrToStructure<SDL_Surface>(tmpSurfacePtr);
        SDL_SetColorKey(tmpSurfacePtr, 1, SDL_MapRGB(s.format, 0xff, 0x66, 0xff));
        var retSurfacePtr = SDL_CreateTextureFromSurface(renderPtr, tmpSurfacePtr);
        SDL_FreeSurface(tmpSurfacePtr);
        return retSurfacePtr;
    }


    internal Sprite NewSprite(GameAssetType newAssetType) => newAssetType switch
    {
        GameAssetType.Player => new Sprite(_playerTexturePtr, GridAssetWidth, GridAssetHeight),
        GameAssetType.Torch => new AnimatedSprite(_torchTexturePtr, GridAssetWidth, GridAssetHeight, 5),
        GameAssetType.WallStone => new Sprite(_floorStoneTexturePtr, GridAssetWidth, GridAssetHeight),
        _ => throw new ArgumentOutOfRangeException(nameof(newAssetType), newAssetType, null)
    };
}

public enum GameAssetType
{
    Player,
    Torch,
    WallStone,
}
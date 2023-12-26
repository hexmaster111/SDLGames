using System.Diagnostics;
using SDLApplication;
using TinyGui.UiElements;
using static SDL2.SDL;

namespace Inferno.MapGeneration;

internal class MapGenerator
{
    private SimpleTimer _collapseTimer = new(0);
    private IMapGenCore _mapGenCore;
    TextElement _textElement;
    private TextureWrapper _lookBox = Textures.LookBox;

    private List<TextureWrapper> _tileTextures = new()
    {
        /*0 - unrealized texture*/Textures.DevTexture,
        /*1 - path */ Textures.PathGravel,
        /*2 - Ground*/ Textures.BgForest, //TODO: make this ground
        /*3 - wall */ Textures.WallTrees,
    };

    private const int TileSize = 8;


    private void NewMap()
    {
        _mapGenCore = new ForestPathMapGenCore(200, 100, 50, -1, 50);
        _textElement = new TextElement($"seed = {_mapGenCore.Seed}")
        {
            X = Program.App.ScreenWidth - 200,
            Y = 0,
        };
    }

    public MapGenerator()
    {
        NewMap();

        foreach (var tw in _tileTextures)
        {
            tw.OutputWidth = TileSize;
            tw.OutputHeight = TileSize;
        }

        _lookBox.OutputWidth = TileSize;
        _lookBox.OutputHeight = TileSize;
        Debug.Assert(_mapGenCore != null && _textElement != null);
    }

    public void Update(long now)
    {
        if (_collapseTimer.Evaluate(now))
        {
            if (_mapGenCore.TakeGenerationStep())
            {
                NewMap();
            }
        }
    }

    public void Render(RenderArgs renderArgs)
    {
        int[,] map = _mapGenCore.CurrentMap;
        int width = _mapGenCore.Width;
        int height = _mapGenCore.Height;

        for (int y = 0; y < height; y++)
        {
            int yPx = y * TileSize;
            for (int x = 0; x < width; x++)
            {
                int xPx = x * TileSize;
                int tile = map[x, y];


                if (tile == -1)
                {
                    _lookBox.Render(xPx, yPx);
                    continue;
                }

                var texture = _tileTextures[tile];
                texture.Render(xPx, yPx);
            }
        }

        _lookBox.Render(_mapGenCore.NextEvalPtX * TileSize, _mapGenCore.NextEvalPtY * TileSize);
        _textElement.Text = $"seed:{_mapGenCore.Seed}\n" + _mapGenCore.GetDebugString();
        _textElement.Render();
    }
}
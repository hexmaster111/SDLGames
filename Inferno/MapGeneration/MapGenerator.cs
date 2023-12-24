using SDLApplication;
using TinyGui.UiElements;
using static SDL2.SDL;

namespace Inferno.MapGeneration;

internal class MapGenerator
{
    private SimpleTimer _collapseTimer = new(0);
    private IMapGenCore _mapGenCore = new ForestPathMapGenCore(20, 15);
    TextElement _textElement;
    private TextureWrapper _lookBox = Textures.LookBox;

    private List<TextureWrapper> _tileTextures = new()
    {
        /*0 - unrealized texture*/Textures.DevTexture,
        /*1 - path */ Textures.PathGravel,
        /*2 - Ground*/ Textures.BgForest, //TODO: make this ground
        /*3 - wall */ Textures.WallTrees,
    };


    public MapGenerator()
    {
        _textElement = new TextElement($"seed = {_mapGenCore.Seed}")
        {
            X = Program.App.ScreenWidth - 200,
            Y = 0,
        };

        // foreach (var tw in _tileTextures)
        // {
        //     tw.   
        // }
    }

    public void Update(long now)
    {
        if (_collapseTimer.Evaluate(now))
        {
            if (_mapGenCore.TakeGenerationStep())
            {
                _mapGenCore = new ForestPathMapGenCore(20, 15);
                _textElement.Text = $"seed = {_mapGenCore.Seed}";
            }
        }
    }

    public void Render(RenderArgs renderArgs)
    {
        int[,] map = _mapGenCore.CurrentMap;
        int width = _mapGenCore.Width;
        int height = _mapGenCore.Height;
        int tileSize = 32;
        for (int y = 0; y < height; y++)
        {
            int yPx = y * tileSize;
            for (int x = 0; x < width; x++)
            {
                int xPx = x * tileSize;
                int tile = map[x, y];
                var texture = _tileTextures[tile];
                texture.Render(xPx, yPx);
            }
        }

        _lookBox.Render(_mapGenCore.NextEvalPtX * tileSize, _mapGenCore.NextEvalPtY * tileSize);

        _textElement.Render();
    }
}
using SDLApplication;
using TinyGui.UiElements;

namespace Inferno.GameSprites;

public class LookBox : GameObject<StatefulAnimatedTextureWrapper>
{
    public LookBox() : base(Textures.LookBox, "LookBox")
    {
        Solidity = Solidity.Passable;
        Moved += OnMoved;
    }

    private void OnMoved()
    {
        _ItemInfoUi.Children.Clear();
        
    }

    private SimpleTimer _textureTimer = new(500);

    private StackPanel _ItemInfoUi = new()
    {
        EnableSelection = false,
    };

    public override void Render(int camXPx, int camYPx)
    {
        if (Hide)
        {
            return;
        }

        base.Render(camXPx, camYPx);
        _ItemInfoUi.Render();
    }

    public override void Update(long now)
    {
        if (Hide)
        {
            return;
        }

        if (_textureTimer.Evaluate(now))
        {
            _texture.NextFrame();
        }
    }

    public bool Hide { get; set; }
}
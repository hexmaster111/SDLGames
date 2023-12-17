﻿using SDLApplication;

namespace Inferno.GameSprites;

public class Torch() : GameObject<StatefulAnimatedTextureWrapper>(Textures.Torch)
{
    private SimpleTimer _textureTimer = new(250);

    public override void Update(long now)
    {
        if (_textureTimer.Evaluate(now))
        {
            _texture.NextFrame();
        }
    }
}
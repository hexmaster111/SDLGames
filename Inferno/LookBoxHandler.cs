using Inferno.GameSprites;
using SDL2;
using SDLApplication;

namespace Inferno;

internal class LookBoxHandler(LookBox lookBox)
{
    public IGameObject Sprite => lookBox;
    
    public void HandleEvent(SDL.SDL_Event sdlEvent)
    {
        if (sdlEvent.type == SDL.SDL_EventType.SDL_KEYDOWN)
        {
            switch (sdlEvent.key.keysym.sym)
            {
                case SDL.SDL_Keycode.SDLK_ESCAPE:
                    State.ActiveFocus = State.UiFocusE.Game;
                    lookBox.Hide = true;
                    Program.FocusedObject = Program.Player;
                    break;

                case SDL.SDL_Keycode.SDLK_UP:
                    lookBox.Y -= 1;
                    break;
                case SDL.SDL_Keycode.SDLK_DOWN:
                    lookBox.Y += 1;
                    break;
                case SDL.SDL_Keycode.SDLK_LEFT:
                    lookBox.X -= 1;
                    break;
                case SDL.SDL_Keycode.SDLK_RIGHT:
                    lookBox.X += 1;
                    break;
            }
        }
    }

    public void Render(RenderArgs args)
    {
        lookBox.Hide = false;
    }

    public void Focus(IGameObject obj)
    {
        lookBox.X = obj.X;
        lookBox.Y = obj.Y;
    }
}
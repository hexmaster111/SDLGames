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
                    lookBox.GridPosY -= 1;
                    break;
                case SDL.SDL_Keycode.SDLK_DOWN:
                    lookBox.GridPosY += 1;
                    break;
                case SDL.SDL_Keycode.SDLK_LEFT:
                    lookBox.GridPosX -= 1;
                    break;
                case SDL.SDL_Keycode.SDLK_RIGHT:
                    lookBox.GridPosX += 1;
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
        lookBox.GridPosX = obj.GridPosX;
        lookBox.GridPosY = obj.GridPosY;
    }
}
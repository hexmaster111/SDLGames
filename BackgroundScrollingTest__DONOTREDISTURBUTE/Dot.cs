using static SDL2.SDL;

namespace BackgroundScrollingTest__DONOTREDISTURBUTE;

internal class Pot(TextureWrapper texture) : GameGridSprite(texture);

internal class Dot : GameGridSprite
{
    public const int HeightPx = 20;
    public const int WidthPx = 20;
    public const int Velocity = 10;


    public int VelX { get; private set; }
    public int VelY { get; private set; }


    public void HandleEvent(SDL_Event e)
    {
        //If a key was pressed
        if (e.type == SDL_EventType.SDL_KEYDOWN && e.key.repeat == 0)
        {
            //Adjust the velocity
            switch (e.key.keysym.sym)
            {
                case SDL_Keycode.SDLK_UP:
                    VelY -= Velocity;
                    break;
                case SDL_Keycode.SDLK_DOWN:
                    VelY += Velocity;
                    break;
                case SDL_Keycode.SDLK_LEFT:
                    VelX -= Velocity;
                    break;
                case SDL_Keycode.SDLK_RIGHT:
                    VelX += Velocity;
                    break;
            }
        }
        //If a key was released
        else if (e.type == SDL_EventType.SDL_KEYUP && e.key.repeat == 0)
        {
            //Adjust the velocity
            switch (e.key.keysym.sym)
            {
                case SDL_Keycode.SDLK_UP:
                    VelY += Velocity;
                    break;
                case SDL_Keycode.SDLK_DOWN:
                    VelY -= Velocity;
                    break;
                case SDL_Keycode.SDLK_LEFT:
                    VelX += Velocity;
                    break;
                case SDL_Keycode.SDLK_RIGHT:
                    VelX -= Velocity;
                    break;
            }
        }
    }

    public void Update()
    {
        Move();
    }

    private void Move()
    {
        //Move the dot left or right
        PosXPx += VelX;

        //If the dot went too far to the left or right
        if (PosXPx < 0 || PosXPx + WidthPx > Program.LevelWidthPx)
        {
            //Move back
            PosXPx -= VelX;
        }

        //Move the dot up or down
        PosYPx += VelY;

        //If the dot went too far up or down
        if (PosYPx < 0 || PosYPx + HeightPx > Program.LevelHeightPx)
        {
            //Move back
            PosYPx -= VelY;
        }
    }

    public Dot(TextureWrapper texture) : base(texture)
    {
    }
}
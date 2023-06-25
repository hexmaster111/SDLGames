using SDL2;
using SDLApplication;
using VectorTd.Towers;

namespace VectorTd;

public class GlobalState
{
    public static int MouseX;
    public static int MouseY;

    public static bool IsPlacingTower;
    public static Tower? PlacingTower;


    public void Render(RenderArgs args)
    {
        //Set the viewport to the entire screen
        var viewport = new SDL.SDL_Rect
        {
            x = 0,
            y = 0,
            w = Game.ScreenWidth,
            h = Game.ScreenHeight
        };
        SDL.SDL_RenderSetViewport(args.RendererPtr, ref viewport);
        
        
        if(IsPlacingTower && PlacingTower != null)
        {
            PlacingTower.RenderPlacing(args, ref viewport, MouseX, MouseY);
        }
        
    }
}
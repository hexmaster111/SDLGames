using SDLApplication;
using static SDL2.SDL;

namespace VectorTd.Ui;

public class InGameUi
{
    public const int Width = 200;
    public const int Height = Game.GridScreenHeight;

    public void Render(RenderArgs args, ref State state, ref SDL_Rect sidebarViewport)
    {
        SDL_RenderSetViewport(args.RendererPtr, ref sidebarViewport);
        args.SetDrawColor(SdlColors.DarkYellow);
        args.FillRect(new SDL_Rect { x = 0, y = 0, w = Width, h = Height });
        args.SetDrawColor(SdlColors.White);
        
    }
}
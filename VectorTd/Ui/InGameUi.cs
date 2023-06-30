using SDLApplication;
using VectorTd.Towers;
using static SDL2.SDL;

namespace VectorTd.Ui;

public class InGameUi
{
    public const int Width = 150;
    public const int Height = Game.GridScreenHeight;
    private const int FontSpace = 14;
    private SDL_Rect _viewPort;

    private record TowerRenderInfo(string Name, int Cost, int Y);

    private TowerRenderInfo[] _towerRenderInfos;

    public InGameUi(SDL_Rect viewPort)
    {
        _viewPort = viewPort;
        _towerRenderInfos = TowerFactory.RefrenceTowers.Select((t, i) => new TowerRenderInfo(t.Name, t.Cost, i * FontSpace + 20)).ToArray();
    }


    private void RenderTowerList(RenderArgs args, ref State state, ref SDL_Rect sidebarViewport)
    {
        SDL_RenderSetViewport(args.RendererPtr, ref sidebarViewport);
        args.SetDrawColor(SdlColors.DarkYellow);
        args.FillRect(new SDL_Rect { x = 0, y = 0, w = Width, h = Height });
        args.SetDrawColor(SdlColors.White);
        foreach (var tower in _towerRenderInfos)
        {
            var color = SdlColors.White;
            var background = SdlColors.DarkYellow;
            //If is being placed, highlight the tower that is being placed
            if (GlobalState.IsPlacingTower && GlobalState.PlacingTower?.Name == tower.Name)
            {
                color = SdlColors.Yellow;
                background = SdlColors.DarkRed;
            }

            var text = $"{tower.Name} - {tower.Cost}";
            args.RenderText(text, 0, tower.Y, color, background);
            args.SetDrawColor(SdlColors.White);
        }
    }

    public void Render(RenderArgs args, ref State state)
    {
        SDL_RenderSetViewport(args.RendererPtr, ref _viewPort);
        args.SetDrawColor(SdlColors.DarkYellow);
        args.FillRect(new SDL_Rect { x = 0, y = 0, w = Width, h = Height });
        args.SetDrawColor(SdlColors.White);
        RenderTowerList(args, ref state, ref _viewPort);

        if (GlobalState.IsPlacingTower)
        {
            args.RenderText("Cancel", _viewPort.w / 2, _viewPort.h / 4, SdlColors.White, SdlColors.DarkRed);
        }
    }


    public void Click(int x, int y, ref State state)
    {
        Console.WriteLine($"Clicked InGameUi {x}, {y}");

        var debugString = string.Join(", ", _towerRenderInfos.Select(t => t.Y));
        Console.WriteLine(debugString);

        if (GlobalState.IsPlacingTower)
        {
            GlobalState.IsPlacingTower = false;
            GlobalState.PlacingTower = null;
            return;
        }


        var tower = _towerRenderInfos.FirstOrDefault(t => t.Y <= y && t.Y + 20 >= y);
        if (tower != null)
        {
            GlobalState.IsPlacingTower = true;
            GlobalState.PlacingTower = TowerFactory.RefrenceTowers.First(t => t.Name == tower.Name);
            Console.WriteLine($"Clicked InGameUi {x}, {y}");
        }


    }
}
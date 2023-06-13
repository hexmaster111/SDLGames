using SDLApplication;
using VectorTd.Tiles;

namespace VectorTd.Towers;

public class SimpleTower : Tower
{
    public SimpleTower(int x, int y) : base(x, y, SdlColors.Yellow, Tile.SizePx / 2, TowerFactory.TowerType.Simple)
    {
    }

    public override void Update(TimeSpan deltaTime, State state)
    {
        //shoot at enemies
    }

    public override string Name => "Simple Tower";
    public override int Cost => 10;
}
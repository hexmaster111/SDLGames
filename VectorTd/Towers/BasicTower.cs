using SDLApplication;
using VectorTd.Tiles;

namespace VectorTd.Towers;

public class BasicTower : Tower
{
    public BasicTower(int x, int y) : base(x, y, SdlColors.Red, Tile.SizePx / 2, TowerFactory.TowerType.Basic)
    {
    }

    public override void Update(TimeSpan deltaTime, State state)
    {
        //shoot at enemies
    }

    public override string Name => "Basic Tower";
    public override int Cost => 10;
}
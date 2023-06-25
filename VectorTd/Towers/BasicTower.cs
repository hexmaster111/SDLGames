using SDLApplication;
using VectorTd.Tiles;

namespace VectorTd.Towers;

public class BasicTower : Tower
{
    public BasicTower(int x, int y) : base(x, y, SdlColors.Red, Tile.SizePx / 2, TowerFactory.TowerType.Basic)
    {
    }

    public override double Damage { get; } = 1;
    public override int Range { get; } = 1;
    public override double FireRate { get; } = 1d;
    

    public override string Name => "Basic Tower";
    public override int Cost => 50;
}
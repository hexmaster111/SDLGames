using SDLApplication;
using VectorTd.Tiles;

namespace VectorTd.Towers;

public class SimpleTower : Tower
{
    public SimpleTower(int x, int y) : base(x, y, SdlColors.Yellow, Tile.SizePx / 2, TowerFactory.TowerType.Simple)
    {
    }


    public override double Damage { get; } = .25d;
    public override int Range { get; } = 1;
    public override double FireRate { get; } = 2;
    public override string Name => "Simple Tower";
    public override int Cost => 10;
}
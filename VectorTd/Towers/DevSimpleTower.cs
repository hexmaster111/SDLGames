using SDLApplication;
using VectorTd.Tiles;

namespace VectorTd.Towers;

public class DevSimpleTower : Tower
{
    public DevSimpleTower(int x, int y) : base(x, y, SdlColors.Yellow, Tile.SizePx / 2, TowerFactory.TowerType.DEV_Simple)
    {
    }


    public override double Damage { get; } = .25d;
    public override int Range { get; } = 1;
    public override double FireRate { get; } = 2;
    public override string Name => "Dev Simple Tower";
    public override int Cost => 0;
}
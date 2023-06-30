using SDLApplication;
using VectorTd.Tiles;

namespace VectorTd.Towers;

public class DevGodTower : Tower
{
    public DevGodTower(int x, int y) : base(x, y, SdlColors.Red, Tile.SizePx / 2, TowerFactory.TowerType.DEV_God)
    {
    }

    public override double Damage { get; } = 100;
    public override int Range { get; } = 100;
    public override double FireRate { get; } = .125;


    public override string Name => "Dev God Tower";
    public override int Cost => 0;
}
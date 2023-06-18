using SDL2;
using SDLApplication;
using VectorTd.Creeps;
using VectorTd.Towers;

namespace VectorTd.Projectiles;

public class LaserProjectile : Projectile
{
    public LaserProjectile(int x, int y, Creep target, double damage, int range,SDL.SDL_Color color) : base(x, y, target, damage,range,  color, TimeSpan.FromSeconds(.5))
    {
    }

}
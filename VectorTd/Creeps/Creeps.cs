using SDL2;
using SDLApplication;

namespace VectorTd.Creeps;

public class SmallCreep : Creep
{
    public SmallCreep() : base(SdlColors.White, 50, 1, 1, 1)
    {
    }
}

public class MediumCreep : Creep
{
    public MediumCreep() : base(SdlColors.Pink, 55, 2, 2, 2)
    {
    }
}


public class LargeCreep : Creep
{
    public LargeCreep() : base(SdlColors.DarkYellow, 50, 4, 4, 4)
    {
    }
}
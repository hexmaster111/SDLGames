namespace VectorTd.Creeps;

public static class CreepFactory
{
    public static Creep Create(CreepType type) => type switch
    {
        CreepType.Small => new SmallCreep(),
        CreepType.Medium => new MediumCreep(),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };
}

public enum CreepType 
{
    Small, 
    Medium
}
namespace Inferno.GameFramework;

[Flags]
public enum ItemBodyUsePosition
{
    NoWhere = 1 << 0,
    Head = 1 << 1,
    Chest = 1 << 2,
    Legs = 1 << 3,
    Feet = 1 << 4,
    Hands = 1 << 5,
}
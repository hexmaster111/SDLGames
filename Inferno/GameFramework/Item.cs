namespace Inferno.GameFramework;

public struct Item
{
    public required GameObjectType Type { get; init; }
    public required ItemType ItemType { get; init; }
    public required ItemBodyUsePosition BodyUsePosition { get; init; }
}
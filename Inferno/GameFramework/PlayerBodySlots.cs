namespace Inferno.GameFramework;

public class PlayerBodySlots<T> where T : struct
{
    public T Head { get; set; }
    public T Chest { get; set; }
    public T Legs { get; set; }
    public T Hands { get; set; }
    public T Feet { get; set; }
}
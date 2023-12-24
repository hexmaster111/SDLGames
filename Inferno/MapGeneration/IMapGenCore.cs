namespace Inferno.MapGeneration;

internal interface IMapGenCore
{
    int Seed { get; }
    int Width { get; }
    int Height { get; }
    public int[,] CurrentMap { get; }
    public int NextEvalPtX { get; }
    public int NextEvalPtY { get; }

    public bool TakeGenerationStep(); //returns true if generation is complete
}
namespace TinyGui;

public static class TinyGuiShared
{
    public static IntPtr RendererPtr { get; private set; } = IntPtr.Zero;
    internal static FontTextureWrapper TextTexture { get; private set; } = null!;

    public static void Init(IntPtr rendererPtr)
    {
        RendererPtr = rendererPtr;
        TextTexture = new FontTextureWrapper(Path.Combine("GuiAssets", "FONT.png"), 16, 16);
    }
}
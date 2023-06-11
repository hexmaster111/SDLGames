namespace SDLApplication;

public struct RenderArgs
{
    public IntPtr WindowPtr;
    public IntPtr RendererPtr;
    public IntPtr FontPtr;
    public int Fps;
    public double DeltaTime;
    public int ScreenWidth_Px;
    public int ScreenHeight_px;
    public RenderArgs(IntPtr windowPtr, nint rendererPtr, nint fontPtr, int fps, double deltaTime, int width_px, int height_px)
    {
        RendererPtr = rendererPtr;
        FontPtr = fontPtr;
        Fps = fps;
        DeltaTime = deltaTime;
        WindowPtr = windowPtr;
        ScreenWidth_Px = width_px;
        ScreenHeight_px = height_px;
    }

}
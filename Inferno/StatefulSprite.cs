namespace Inferno;

internal class
    StatefulSprite(IntPtr texturePrt, int gridAssetWidth, int gridAssetHeight, int stateCount, int animationFrames)
    : AnimatedSprite(texturePrt, gridAssetWidth, gridAssetHeight, animationFrames)
{
    public void SetState(int stateNumber)
    {
        if (stateNumber > stateCount) throw new Exception("State Out of range");
        CurrentSpriteRect.x = NextFrameIncAmount * stateNumber;
    }
}
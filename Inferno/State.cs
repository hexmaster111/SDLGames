namespace Inferno;

internal static class State
{
    public static UiFocusE ActiveFocus = UiFocusE.Game;

    public enum UiFocusE
    {
        Game,
        Inventory,
        Grab,
        LookBox,
        OpenMenu,
        CloseMenu
    }
}
using static SDL2.SDL;

namespace SDLApplication;

public static class SdlColors
{
    public static SDL_Color Transparent { get; } = new() { a = 0, b = 0, g = 0, r = 0 };
    public static SDL_Color Black { get; } = new() { a = 255, b = 0, g = 0, r = 0 };
    public static SDL_Color White { get; } = new() { a = 255, b = 255, g = 255, r = 255 };
    public static SDL_Color Red { get; } = new() { a = 255, b = 0, g = 0, r = 255 };
    public static SDL_Color Green { get; } = new() { a = 255, b = 0, g = 255, r = 0 };
    public static SDL_Color Blue { get; } = new() { a = 255, b = 255, g = 0, r = 0 };
    public static SDL_Color Yellow { get; } = new() { a = 255, b = 0, g = 255, r = 255 };
    public static SDL_Color Cyan { get; } = new() { a = 255, b = 255, g = 255, r = 0 };
    public static SDL_Color Magenta { get; } = new() { a = 255, b = 255, g = 0, r = 255 };
    public static SDL_Color Silver { get; } = new() { a = 255, b = 192, g = 192, r = 192 };
    public static SDL_Color Gray { get; } = new() { a = 255, b = 128, g = 128, r = 128 };
    public static SDL_Color Maroon { get; } = new() { a = 255, b = 0, g = 0, r = 128 };
    public static SDL_Color Olive { get; } = new() { a = 255, b = 0, g = 128, r = 128 };
    public static SDL_Color GreenDark { get; } = new() { a = 255, b = 0, g = 128, r = 0 };
    public static SDL_Color Purple { get; } = new() { a = 255, b = 128, g = 0, r = 128 };
    public static SDL_Color Teal { get; } = new() { a = 255, b = 128, g = 128, r = 0 };
    public static SDL_Color Navy { get; } = new() { a = 255, b = 128, g = 0, r = 0 };
    public static SDL_Color DarkYellow { get; } = new() { a = 255, b = 0, g = 128, r = 128 };
    public static SDL_Color DarkGray { get; } = new() { a = 255, b = 64, g = 64, r = 64 };
    public static SDL_Color DarkGreen { get; } = new() { a = 255, b = 0, g = 64, r = 0 };
    public static SDL_Color Salmon { get; } = new() { a = 255, b = 112, g = 112, r = 255 };
    public static SDL_Color DarkRed { get; } = new() { a = 255, b = 0, g = 0, r = 64 };
    public static SDL_Color Tan { get; } = new() { a = 255, b = 128, g = 192, r = 192 };
    public static SDL_Color Pink { get; } = new() { a = 255, b = 255, g = 192, r = 192 };
}
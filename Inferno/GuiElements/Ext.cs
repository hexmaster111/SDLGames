using SDL2;

namespace Inferno.GuiElements;

public static class Ext
{
    public static int MeasureFontHeight(this string text, int fontSize)
    {
        var sr = new StringReader(text);
        int r;
        var pos = new SDL.SDL_Point();
        var max = 0;
        while ((r = sr.Read()) != -1)
        {
            switch (r)
            {
                case '\n':
                    pos.y += fontSize;
                    continue;
                case '\r':
                    pos.x = 0;
                    continue;
            }

            pos.x += fontSize;
            if (pos.x > max) max = pos.x;
        }

        return max;
    }


    public static int MeasureFontWidth(this string text, int fontSize)
    {
        var sr = new StringReader(text);
        int r;
        var pos = new SDL.SDL_Point();
        var max = 0;
        while ((r = sr.Read()) != -1)
        {
            switch (r)
            {
                case '\n':
                    pos.y += fontSize;
                    continue;
                case '\r':
                    pos.x = 0;
                    continue;
            }

            pos.x += fontSize;
            if (pos.x > max) max = pos.x;
        }

        return max;
    }
}
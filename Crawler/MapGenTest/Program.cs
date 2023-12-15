﻿// See https://aka.ms/new-console-template for more information

using SDL2;
using SDLApplication;

namespace MapGenTest;

internal static class Program
{
    internal static Game Game;
    internal static SdlApp App;

    public static void Main(string[] args)
    {
        App = new SdlApp(EventHandler, RenderHandler, UpdateHandler,
            targetFps: 30,
            height: 600,
            width: 800,
            targetUpdatesPerSec: 10);
        Game = new Game();
        App.Run();
    }

    private static void UpdateHandler(TimeSpan _, long now) => Game.Update(now);

    private static void RenderHandler(RenderArgs args) => Game.Render(args);

    private static void EventHandler(SDL.SDL_Event e) => Game.Event(e);
}
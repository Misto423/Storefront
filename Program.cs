using System;

namespace Storefront
{
#if WINDOWS || XBOX
    static class Program
    {
        public static Input.SFConsole gameConsole;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                gameConsole = new Input.SFConsole(game);
                game.Run();
            }
        }
    }
#endif
}


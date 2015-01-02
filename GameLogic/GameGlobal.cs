using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace Storefront.GameLogic
{
    //enum to handle the states of the game
    public enum GameState
    {
        Startup,
        MainMenu,
        NewGame,
        GameStandard,
        InventoryDisplay,
        OfficeMode,
        ChapterTransition,
        EndGame,
        Options
    }

    public static class GameGlobal
    {
        //handles the games current state
        public static GameState CurrentGS;
        //the previous state (used to go back in certain cases)
        public static GameState PreviousGS;

        //handles input for the entire game
        public static Input.InputHelper InputControl = new Input.InputHelper();

        //graphics readonly vars
        public static readonly int TILESIZE = 32;
        //screensize vars
        public static int GameWidth, GameHeight;
        //global gamefont
        public static SpriteFont gameFont;

        //player info
        public static Player player;

        //class to handle orders throughout the game
        public static MainGamePlay.OrderHandler orderHandler;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Storefront
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private bool initialLoad = true;
        public static bool newState = false;
        public static bool closeTrigger = false;
        public static bool pauseInput = false;

        //state class vars
        GameLogic.NewGameStuff.NewGameLogic newGame;
        Storefront.GameLogic.MainGamePlay.GameplayView gv;
        Menus.IntroMenu introMenus;

        /// <summary>
        /// Initialize the game.
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //set window properties
#if WINDOWS
            graphics.IsFullScreen = false;
            //show mouse on windows platform
            IsMouseVisible = true;
            this.Window.Title = "StoreFront";
#else
            graphics.IsFullScreen = true;
#endif
            //set screen size
            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            //set the screen size to be able to be accessed by entire game
            GameLogic.GameGlobal.GameWidth = graphics.PreferredBackBufferWidth;
            GameLogic.GameGlobal.GameHeight = graphics.PreferredBackBufferHeight;

            //set to a default 30 fps
            TargetElapsedTime = TimeSpan.FromTicks(333333);
            IsFixedTimeStep = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //TESTING - set to STARTUP for regular game
            GameLogic.GameGlobal.CurrentGS = GameLogic.GameState.MainMenu;
            //init console
            Program.gameConsole.Initialize();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            if (initialLoad)
            {
                // Create a new SpriteBatch, which can be used to draw textures.
                spriteBatch = new SpriteBatch(GraphicsDevice);
                //load the game's font
                GameLogic.GameGlobal.gameFont = Content.Load<SpriteFont>(@"Text\GameFont");
                Program.gameConsole.AddLine("Font Loaded", Microsoft.Xna.Framework.Color.Green);
                //load global textures
                Graphics.GlobalGfx.initGfx(Content, GraphicsDevice);
                //create a new texture to be used as the "empty texture" for the gfx class
                Texture2D t = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                Graphics.SimpleDraw.initGfx(t);
                //switch to false so main stuff only loaded once.
                initialLoad = false;
                //create the global order handler.
                GameLogic.GameGlobal.orderHandler = new GameLogic.MainGamePlay.OrderHandler();         
            }

            switch (GameLogic.GameGlobal.CurrentGS)
            {
                case GameLogic.GameState.Startup:

                    break;
                default:
                case GameLogic.GameState.MainMenu:
                    introMenus = new Menus.IntroMenu(Content);
                    introMenus.loadIntroMenus();
                    break;
                case GameLogic.GameState.NewGame:
                    newGame = new GameLogic.NewGameStuff.NewGameLogic(Content);
                    newGame.loadNGLContent();
                    break;
                case GameLogic.GameState.OfficeMode:

                    break;
                case GameLogic.GameState.Options:

                    break;
                case GameLogic.GameState.GameStandard:
                    newGame.unloadIntroEdit();
                    gv = new GameLogic.MainGamePlay.GameplayView();
                    gv.loadGV(Content);
                    break;
                case GameLogic.GameState.ChapterTransition:

                    break;
                case GameLogic.GameState.EndGame:

                    break;
            }

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();
            if (closeTrigger)
            {
                CloseGame();
            }


            //update controls
            if (!pauseInput)
            {
                GameLogic.GameGlobal.InputControl.Update();
            }


            //update the console
            if (GameLogic.GameGlobal.InputControl.IsNewPress(Keys.Tab))
            {
                Program.gameConsole.Visible = !Program.gameConsole.Visible;
            }
            if (Program.gameConsole.Visible)
            {
                Program.gameConsole.Update(gameTime);
            }
            

            switch (GameLogic.GameGlobal.CurrentGS)
            {
                case GameLogic.GameState.Startup:

                    break;
                default:
                case GameLogic.GameState.MainMenu:
                    introMenus.updateIntroMenus(gameTime);
                    break;
                case GameLogic.GameState.NewGame:
                    newGame.updateNGL(gameTime);
                    break;
                case GameLogic.GameState.OfficeMode:

                    break;
                case GameLogic.GameState.Options:

                    break;
                case GameLogic.GameState.GameStandard:
                    gv.updateGV(gameTime, Content);
                    break;
                //case GameLogic.GameState.InventoryDisplay:
                //    id.updateInvDisplay(gameTime);
                //    break;
                case GameLogic.GameState.ChapterTransition:

                    break;
                case GameLogic.GameState.EndGame:

                    break;
            }

            if (newState)
            {
                LoadContent();
                newState = false;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (GameLogic.GameGlobal.CurrentGS)
            {
                case GameLogic.GameState.Startup:

                    break;
                default:
                case GameLogic.GameState.MainMenu:
                    introMenus.drawIntroMenus(spriteBatch, gameTime);
                    break;
                case GameLogic.GameState.NewGame:
                    newGame.drawNGL(spriteBatch, gameTime);
                    break;
                case GameLogic.GameState.OfficeMode:

                    break;
                case GameLogic.GameState.Options:

                    break;
                case GameLogic.GameState.GameStandard:
                    gv.drawGV(spriteBatch, gameTime, GraphicsDevice);
                    break;
                case GameLogic.GameState.ChapterTransition:

                    break;
                case GameLogic.GameState.EndGame:

                    break;
            }

            //TESTING CUSTOMER PALETTE SWAPPING
            //if (once)
            //{
            //    once = false;
            //    test = Graphics.GlobalGfx.createCustomerTexture(GraphicsDevice, spriteBatch);
            //}
            //spriteBatch.Begin();
            //spriteBatch.Draw(test, new Vector2(0, 0), Color.White);
            //spriteBatch.End();
            //END TEST

            //draw the console
            if (Program.gameConsole.Visible)
            {
                Program.gameConsole.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        private void CloseGame()
        {
            UnloadContent();
            this.Exit();
        }
    }
}

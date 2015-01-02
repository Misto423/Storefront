using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.Input
{
    //stores all commands here.
    public enum CommandList
    {

    }

    public struct SFConsoleMessage
    {
        public string msg;
        public Color color;

        public SFConsoleMessage(string msg, Color msgColor)
        {
            this.color = msgColor;
            this.msg = msg;
        }
    }

    public class SFConsole : DrawableGameComponent
    {
        private List<SFConsoleMessage> consoleList = new List<SFConsoleMessage>();
        SFTextField commandField;
        SpriteBatch sb;
        Vector2 draw = new Vector2(0,GameLogic.GameGlobal.GameHeight / 1.25f);
        int YSpace;
        bool triggerOnce = true;

        public SFConsole(Game1 game) : base(game)
        {

        }

        public override void Initialize()
        {
            this.Visible = false;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            sb = new SpriteBatch(GraphicsDevice);
            
            base.LoadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (triggerOnce)
            {
                triggerOnce = false;
                YSpace = (int)GameLogic.GameGlobal.gameFont.MeasureString("K").Y;
            }
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.Black);
            sb.Begin();
            int size = consoleList.Count - 1;
            for (int index = size; index >= 0; index--)
            {
                sb.DrawString(GameLogic.GameGlobal.gameFont, consoleList[index].msg, draw, consoleList[index].color);
                draw.Y -= YSpace;

                if (draw.Y <= 0)
                {
                    break;
                }
            }
            draw.Y = GameLogic.GameGlobal.GameHeight / 1.25f;
            sb.End();
        }

        public void AddLine(string line, Color msgColor)
        {
            consoleList.Add(new SFConsoleMessage(line, msgColor));
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}

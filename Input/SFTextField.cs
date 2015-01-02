using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Storefront.Input
{
    public class SFTextField
    {
        private Vector2 location;
        private Rectangle bounds;
        private Texture2D fieldTex = null;
        private Texture2D cursor;
        private Texture2D backgroundTex;
        private bool focused = false;
        private bool initialFocus = true, resetFocus = false;
        private string fieldText = "";
        private char input;
        private int charLimit;
        private bool numOnly = false;
        //time stuff
        private int interval = 1;
        private double timePassed = 0;
        private bool cursorToggle = false;
        private Vector2 stringLength;


        /// <summary>
        /// SFTextField Constructor
        /// </summary>
        /// <param name="bounds">The rectangle bounds of the typing area.</param>
        /// <param name="fieldTex">The texture to use for typing area</param>
        /// <param name="cursor">The cursor texture.</param>
        /// <param name="limit">Character limit for the text field </param>
        public SFTextField(Rectangle bounds, Texture2D fieldTex, Texture2D cursor, int limit)
        {
            this.bounds = bounds;
            this.fieldTex = fieldTex;
            charLimit = limit;
            this.cursor = cursor;
        }

        /// <summary>
        /// SFTextField Constructor
        /// </summary>
        /// <param name="location">The top left location to draw the background image</param>
        /// <param name="background">The background image for the text field</param>
        /// <param name="bounds">The rectangle bounds of the typing area.</param>
        /// <param name="fieldTex">The texture to use for typing area</param>
        /// <param name="cursor">The cursor texture.</param>
        /// <param name="limit">Character limit for the text field </param>
        public SFTextField(Vector2 location, Texture2D background, Rectangle bounds, Texture2D fieldTex, Texture2D cursor, int limit)
        {
            this.bounds = bounds;
            this.fieldTex = fieldTex;
            charLimit = limit;
            this.location = location;
            this.backgroundTex = background;
            this.cursor = cursor;
        }

        #region Attributes
        public string Text
        {
            get { return fieldText; }
            set { fieldText = value; }
        }

        public bool NumbersOnly
        {
            get { return numOnly; }
            set { numOnly = value; }
        }

        public bool Focus
        {
            get { return focused; }
        }

        public Rectangle Bounds
        {
            get { return bounds; }
        }

        public bool ResetFocus
        {
            get { return resetFocus; }
            set { resetFocus = value; }
        }
        #endregion

        /// <summary>
        /// Updates the control of the text field and gets typed characters.
        /// </summary>
        public void updateText()
        {
            //if the mouse is inside the text field
            if (GameLogic.GameGlobal.InputControl.CurrentMouseState.X >= bounds.Left &&
                GameLogic.GameGlobal.InputControl.CurrentMouseState.X <= bounds.Right &&
                GameLogic.GameGlobal.InputControl.CurrentMouseState.Y >= bounds.Top &&
                GameLogic.GameGlobal.InputControl.CurrentMouseState.Y <= bounds.Bottom)
            {
                //and if the left click on mouse occurs while inside.
                if (GameLogic.GameGlobal.InputControl.IsNewPress(MouseBtns.LeftClick))
                {
                    focused = true;
                }
            }
            else
            {
                //if the mouse is outside and left click occurs, lose focus
                if (GameLogic.GameGlobal.InputControl.IsNewPress(MouseBtns.LeftClick))
                {
                    focused = false;
                }
            }

            //if focused is true, capture keyboard input and update the fieldText accordingly
            if (focused)
            {
                if (initialFocus && resetFocus)
                {
                    fieldText = "";
                    initialFocus = false;
                }
                //get the keyboard input
                if (TryConvertKeyboardInput(GameLogic.GameGlobal.InputControl.CurrentKeyboardState,
                                            GameLogic.GameGlobal.InputControl.LastKeyboardState, out input))
                {
                    //if the input is a backspace
                    if (input == 3000)
                    {
                        //and there are characters in the string
                        if (fieldText.Length > 0)
                        {
                            //take the last character off the string
                            fieldText = fieldText.Substring(0, fieldText.Length - 1);
                        }
                    }
                    else //if input is any other character from keyboard
                    {
                        //append it to the string if it does no exceed char limit
                        if (fieldText.Length < charLimit)
                        {
                            fieldText += input;
                        }
                    }
                }
            }
            else
            {
                initialFocus = true;
            }
        }

        /// <summary>
        /// Draws the textures for the field, as well as text that is typed.
        /// </summary>
        /// <param name="sb">The spritebatch used to draw the text field.</param>
        /// <param name="gt">The game time object used to draw cursor.</param>
        public void drawField(SpriteBatch sb, GameTime gt)
        {
            sb.Begin();
            //draw background
            if (backgroundTex != null)
            {
                sb.Draw(backgroundTex, location, Color.White);
            }
            //draw text area texture
            sb.Draw(fieldTex, bounds, Color.White);
            //draw cursor
            if (focused)
            {
                timePassed += gt.ElapsedGameTime.TotalSeconds;
                stringLength = GameLogic.GameGlobal.gameFont.MeasureString(fieldText);
                if (timePassed >= interval)
                {
                    timePassed = 0;
                    cursorToggle = !cursorToggle;
                }
                if (cursorToggle)
                {
                    sb.Draw(cursor,
                            new Rectangle((int)centerFieldText().X + (int)GameLogic.GameGlobal.gameFont.MeasureString(this.fieldText).X,
                                bounds.Top + 2, cursor.Width, cursor.Height * 2),
                            Color.Red);
                }
                else
                {
                    sb.Draw(cursor,
                            new Rectangle((int)centerFieldText().X + (int)GameLogic.GameGlobal.gameFont.MeasureString(this.fieldText).X, 
                                bounds.Top + 2, cursor.Width, cursor.Height * 2),
                            Color.White);
                }
            }
            //draw string
            sb.DrawString(GameLogic.GameGlobal.gameFont, fieldText, centerFieldText(), Color.White);
            sb.End();
        }

        public void unloadField()
        {
            location = Vector2.Zero;
            stringLength = Vector2.Zero;
            fieldText = "";
            bounds = Rectangle.Empty;
        }

        /// <summary>
        /// Center the text in the field
        /// </summary>
        /// <returns>Vector2 position to draw the text.</returns>
        private Vector2 centerFieldText()
        {
            //get the string dimensions
            Vector2 stringMeas = GameLogic.GameGlobal.gameFont.MeasureString(this.fieldText);
            //center the text based on string dimensions and button bounds
            Vector2 pos = new Vector2(bounds.Center.X - (stringMeas.X / 2), bounds.Center.Y - (stringMeas.Y / 2));
            //return the centered vector to draw the text.
            return pos;
        }

        //huge switch statement bs that is needed to convert the keyboard state
        //to an actual char value to be added to the string
        private bool TryConvertKeyboardInput(KeyboardState keyboard, KeyboardState oldKeyboard, out char key)
        {
            Keys[] keys = keyboard.GetPressedKeys();
            bool shift = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);

            for (int i = 0; i < keys.Length; i++)
            {
                if (!oldKeyboard.IsKeyDown(keys[i]))
                {
                    if (numOnly == false)
                    {
                        switch (keys[i])
                        {
                            //Alphabet keys
                            case Keys.A: if (shift) { key = 'A'; } else { key = 'a'; } return true;
                            case Keys.B: if (shift) { key = 'B'; } else { key = 'b'; } return true;
                            case Keys.C: if (shift) { key = 'C'; } else { key = 'c'; } return true;
                            case Keys.D: if (shift) { key = 'D'; } else { key = 'd'; } return true;
                            case Keys.E: if (shift) { key = 'E'; } else { key = 'e'; } return true;
                            case Keys.F: if (shift) { key = 'F'; } else { key = 'f'; } return true;
                            case Keys.G: if (shift) { key = 'G'; } else { key = 'g'; } return true;
                            case Keys.H: if (shift) { key = 'H'; } else { key = 'h'; } return true;
                            case Keys.I: if (shift) { key = 'I'; } else { key = 'i'; } return true;
                            case Keys.J: if (shift) { key = 'J'; } else { key = 'j'; } return true;
                            case Keys.K: if (shift) { key = 'K'; } else { key = 'k'; } return true;
                            case Keys.L: if (shift) { key = 'L'; } else { key = 'l'; } return true;
                            case Keys.M: if (shift) { key = 'M'; } else { key = 'm'; } return true;
                            case Keys.N: if (shift) { key = 'N'; } else { key = 'n'; } return true;
                            case Keys.O: if (shift) { key = 'O'; } else { key = 'o'; } return true;
                            case Keys.P: if (shift) { key = 'P'; } else { key = 'p'; } return true;
                            case Keys.Q: if (shift) { key = 'Q'; } else { key = 'q'; } return true;
                            case Keys.R: if (shift) { key = 'R'; } else { key = 'r'; } return true;
                            case Keys.S: if (shift) { key = 'S'; } else { key = 's'; } return true;
                            case Keys.T: if (shift) { key = 'T'; } else { key = 't'; } return true;
                            case Keys.U: if (shift) { key = 'U'; } else { key = 'u'; } return true;
                            case Keys.V: if (shift) { key = 'V'; } else { key = 'v'; } return true;
                            case Keys.W: if (shift) { key = 'W'; } else { key = 'w'; } return true;
                            case Keys.X: if (shift) { key = 'X'; } else { key = 'x'; } return true;
                            case Keys.Y: if (shift) { key = 'Y'; } else { key = 'y'; } return true;
                            case Keys.Z: if (shift) { key = 'Z'; } else { key = 'z'; } return true;

                            //Decimal keys
                            case Keys.D0: if (shift) { key = ')'; } else { key = '0'; } return true;
                            case Keys.D1: if (shift) { key = '!'; } else { key = '1'; } return true;
                            case Keys.D2: if (shift) { key = '@'; } else { key = '2'; } return true;
                            case Keys.D3: if (shift) { key = '#'; } else { key = '3'; } return true;
                            case Keys.D4: if (shift) { key = '$'; } else { key = '4'; } return true;
                            case Keys.D5: if (shift) { key = '%'; } else { key = '5'; } return true;
                            case Keys.D6: if (shift) { key = '^'; } else { key = '6'; } return true;
                            case Keys.D7: if (shift) { key = '&'; } else { key = '7'; } return true;
                            case Keys.D8: if (shift) { key = '*'; } else { key = '8'; } return true;
                            case Keys.D9: if (shift) { key = '('; } else { key = '9'; } return true;

                            //Decimal numpad keys
                            case Keys.NumPad0: key = '0'; return true;
                            case Keys.NumPad1: key = '1'; return true;
                            case Keys.NumPad2: key = '2'; return true;
                            case Keys.NumPad3: key = '3'; return true;
                            case Keys.NumPad4: key = '4'; return true;
                            case Keys.NumPad5: key = '5'; return true;
                            case Keys.NumPad6: key = '6'; return true;
                            case Keys.NumPad7: key = '7'; return true;
                            case Keys.NumPad8: key = '8'; return true;
                            case Keys.NumPad9: key = '9'; return true;

                            //Special keys
                            case Keys.OemTilde: if (shift) { key = '~'; } else { key = '`'; } return true;
                            case Keys.OemSemicolon: if (shift) { key = ':'; } else { key = ';'; } return true;
                            case Keys.OemQuotes: if (shift) { key = '"'; } else { key = '\''; } return true;
                            case Keys.OemQuestion: if (shift) { key = '?'; } else { key = '/'; } return true;
                            case Keys.OemPlus: if (shift) { key = '+'; } else { key = '='; } return true;
                            case Keys.OemPipe: if (shift) { key = '|'; } else { key = '\\'; } return true;
                            case Keys.OemPeriod: if (shift) { key = '>'; } else { key = '.'; } return true;
                            case Keys.OemOpenBrackets: if (shift) { key = '{'; } else { key = '['; } return true;
                            case Keys.OemCloseBrackets: if (shift) { key = '}'; } else { key = ']'; } return true;
                            case Keys.OemMinus: if (shift) { key = '_'; } else { key = '-'; } return true;
                            case Keys.OemComma: if (shift) { key = '<'; } else { key = ','; } return true;
                            case Keys.Space: key = ' '; return true;

                            //handle backspace
                            case Keys.Back: key = (char)3000; return true;
                        }
                    }
                    else
                    {
                        switch (keys[i])
                        {
                             //Decimal keys
                            case Keys.D0: key = '0'; return true;
                            case Keys.D1: key = '1'; return true;
                            case Keys.D2: key = '2'; return true;
                            case Keys.D3: key = '3'; return true;
                            case Keys.D4: key = '4'; return true;
                            case Keys.D5: key = '5'; return true;
                            case Keys.D6: key = '6'; return true;
                            case Keys.D7: key = '7'; return true;
                            case Keys.D8: key = '8'; return true;
                            case Keys.D9: key = '9'; return true;

                            //Decimal numpad keys
                            case Keys.NumPad0: key = '0'; return true;
                            case Keys.NumPad1: key = '1'; return true;
                            case Keys.NumPad2: key = '2'; return true;
                            case Keys.NumPad3: key = '3'; return true;
                            case Keys.NumPad4: key = '4'; return true;
                            case Keys.NumPad5: key = '5'; return true;
                            case Keys.NumPad6: key = '6'; return true;
                            case Keys.NumPad7: key = '7'; return true;
                            case Keys.NumPad8: key = '8'; return true;
                            case Keys.NumPad9: key = '9'; return true;

                            //handle backspace
                            case Keys.Back: key = (char)3000; return true;
                        }
                    }
                }
            }

            key = (char)0;
            return false;
        }
    }
}

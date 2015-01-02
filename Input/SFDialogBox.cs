using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.Input
{
    public enum DefaultChoices
    {
        Yes_No,
        Close,
        Ok_Cancel,
    }

    public class SFDialogBox
    {
        private Rectangle bounds;
        private string dialog;
        private string[] choices;
        private int selection = -1;
        private Texture2D texture;

        /// <summary>
        /// Use this constructor for custom choices.
        /// </summary>
        /// <param name="bounds">The bounds of the text box.  Use Rectangle.Empty for default size.</param>
        /// <param name="texture">The texture of the dialog box.</param>
        /// <param name="dialog">The message to display.</param>
        /// <param name="choices">String array of possible choices for the user to select.</param>
        public SFDialogBox(Rectangle bounds, Texture2D texture, string dialog, string[] choices)
        {
            if (!bounds.IsEmpty)
            {
                this.bounds = bounds;
            }
            else
            {
                this.bounds = new Rectangle(GameLogic.GameGlobal.GameWidth / 4, GameLogic.GameGlobal.GameHeight / 4,
                    GameLogic.GameGlobal.GameWidth / 2, GameLogic.GameGlobal.GameHeight / 2);
            }
            this.texture = texture;
            this.dialog = dialog;
            this.choices = choices;
            selection = -1;
        }

        /// <summary>
        /// Use this constructor for common/preset choices.
        /// </summary>
        /// <param name="bounds">The bounds of the text box.  Use Rectangle.Empty for default size.</param>
        /// <param name="texture">The texture of the dialog box.</param>
        /// <param name="dialog">The message to display.</param>
        /// <param name="choices">A Default Choice enum to display a common set of choices.</param>
        public SFDialogBox(Rectangle bounds, Texture2D texture, string dialog, DefaultChoices choices)
        {
            if (!bounds.IsEmpty)
            {
                this.bounds = bounds;
            }
            else
            {
                this.bounds = new Rectangle(GameLogic.GameGlobal.GameWidth / 4, GameLogic.GameGlobal.GameHeight / 4,
                    GameLogic.GameGlobal.GameWidth / 2, GameLogic.GameGlobal.GameHeight / 2);
            }
            this.texture = texture;
            this.dialog = dialog;
            this.choices = DefaultChoiceBuilder(choices);
            selection = -1;
        }

        /// <summary>
        /// Takes a DefaultChoice var and creates the string array for it.
        /// </summary>
        /// <param name="dc">A preset choice.</param>
        /// <returns>String array containing a set of choices.</returns>
        private string[] DefaultChoiceBuilder(DefaultChoices dc)
        {
            string[] choices;
            switch (dc)
            {
                case DefaultChoices.Ok_Cancel:
                    //create the string array
                    choices = new string[2];
                    //assign the strings
                    choices[0] = "Accept";
                    choices[1] = "Cancel";
                    break;
                case DefaultChoices.Yes_No:
                    //create the string array
                    choices = new string[2];
                    choices[0] = "Yes";
                    choices[1] = "No";
                    break;
                case DefaultChoices.Close:
                default:
                    //create the string array
                    choices = new string[1];
                    choices[0] = "Close";
                    break;
            }
            return choices;
        }

        /// <summary>
        /// Gets the selected choice.
        /// </summary>
        public int Selection
        {
            get 
            {
                int temp = selection;
                selection = -1; //resets the selection, so it doesnt carry over to another selection.
                return temp; 
            }
        }

        /// <summary>
        /// Sets the dialog of the dialog box, useful for changing text.
        /// </summary>
        public string Dialog
        {
            set { dialog = value; }
        }

        /// <summary>
        /// Set a new array of choices to select from.
        /// </summary>
        public string[] CustomChoices
        {
            set { choices = value; }
        }

        /// <summary>
        /// Sets the choices to a preset array.
        /// </summary>
        public DefaultChoices PresetChoices
        {
            set { choices = DefaultChoiceBuilder(value); }
        }

        public void updateDialogBox()
        {
            if (GameLogic.GameGlobal.InputControl.IsNewPress(Keys.D1) || GameLogic.GameGlobal.InputControl.IsNewPress(Keys.NumPad1))
            {
                selection = 0;
            }
            #region Selection Branches for choices over 1
            if (choices.Length > 1)
            {
                if (GameLogic.GameGlobal.InputControl.IsNewPress(Keys.D2) || GameLogic.GameGlobal.InputControl.IsNewPress(Keys.NumPad2))
                {
                    selection = 1;
                }
                if (GameLogic.GameGlobal.InputControl.IsNewPress(Keys.D3) || GameLogic.GameGlobal.InputControl.IsNewPress(Keys.NumPad3))
                {
                    selection = 2;
                }
                if (GameLogic.GameGlobal.InputControl.IsNewPress(Keys.D4) || GameLogic.GameGlobal.InputControl.IsNewPress(Keys.NumPad4))
                {
                    selection = 3;
                }
                if (GameLogic.GameGlobal.InputControl.IsNewPress(Keys.D5) || GameLogic.GameGlobal.InputControl.IsNewPress(Keys.NumPad5))
                {
                    selection = 4;
                }
                if (GameLogic.GameGlobal.InputControl.IsNewPress(Keys.D6) || GameLogic.GameGlobal.InputControl.IsNewPress(Keys.NumPad6))
                {
                    selection = 5;
                }
                if (GameLogic.GameGlobal.InputControl.IsNewPress(Keys.D7) || GameLogic.GameGlobal.InputControl.IsNewPress(Keys.NumPad7))
                {
                    selection = 6;
                }
                if (GameLogic.GameGlobal.InputControl.IsNewPress(Keys.D8) || GameLogic.GameGlobal.InputControl.IsNewPress(Keys.NumPad8))
                {
                    selection = 7;
                }
                if (GameLogic.GameGlobal.InputControl.IsNewPress(Keys.D9) || GameLogic.GameGlobal.InputControl.IsNewPress(Keys.NumPad9))
                {
                    selection = 8;
                }
            }
            #endregion
        }

        public void drawDialogBox(SpriteBatch sb, GameTime gt)
        {
            sb.Begin();
            sb.Draw(texture, bounds, Color.White);
            float yPos = (bounds.Height / 10f) + bounds.Top;
            float endDialogPos;
            if (GameLogic.GameGlobal.gameFont.MeasureString(dialog).X > bounds.Width - 30)
            {
                List<string> dialogBreaks = WordWrapDialog();
                
                foreach (string s in dialogBreaks)
                {
                    sb.DrawString(GameLogic.GameGlobal.gameFont, s + "\n", new Vector2((bounds.Width / 15f) + bounds.Left, yPos), Color.WhiteSmoke);
                    yPos += GameLogic.GameGlobal.gameFont.MeasureString(s).Y + 5;
                }
                endDialogPos = yPos;
            }
            else
            {
                sb.DrawString(GameLogic.GameGlobal.gameFont, dialog, new Vector2((bounds.Width / 15f) + bounds.Left, yPos), Color.WhiteSmoke);
                yPos += GameLogic.GameGlobal.gameFont.MeasureString(dialog).Y + 5;
                endDialogPos = yPos;
            }

            int index = 1;
            foreach (string s in choices)
            {
                if (MouseOver(s, yPos, index))
                {   //highlight the hovered over choice
                    sb.DrawString(GameLogic.GameGlobal.gameFont, index + ") " + s, new Vector2((bounds.Width / 15f) + bounds.Left, yPos), Color.Red);
                    //get mouse clicked event here since the text and measurements can't be easily gotten in update
                    if (GameLogic.GameGlobal.InputControl.IsNewPress(MouseBtns.LeftClick))
                    {
                        selection = (index - 1); //get array index of selected choice if clicked.
                    }
                }
                else
                {
                    sb.DrawString(GameLogic.GameGlobal.gameFont, index + ") " + s, new Vector2((bounds.Width / 15f) + bounds.Left, yPos), Color.WhiteSmoke);
                }
                //update positions
                yPos += GameLogic.GameGlobal.gameFont.MeasureString(dialog).Y + 5;
                index++;
            }
            sb.End();
        }

        /// <summary>
        /// Creates lines that fit inside the dialog box.
        /// </summary>
        /// <returns>A list of lines.</returns>
        private List<string> WordWrapDialog()
        {
            List<string> dialogBreaks = new List<string>();
            List<string> words = DecomposeString();
            string curLine = "";
            int curLineLength = 0;
            int whitespaceLength = (int)GameLogic.GameGlobal.gameFont.MeasureString(" ").X;

            foreach (string s in words)
            {
                int wordLength = (int)GameLogic.GameGlobal.gameFont.MeasureString(s).X;
                if (curLineLength + wordLength + whitespaceLength < (bounds.Width - 30))
                {
                    curLine = curLine + s + " ";
                    curLineLength = curLineLength + wordLength + whitespaceLength;
                }
                else
                {
                    //add the line to the list and reset length and line string
                    dialogBreaks.Add(curLine);
                    curLineLength = wordLength + whitespaceLength;
                    curLine = s + " ";
                }
            }
            //add the last line
            dialogBreaks.Add(curLine);

            return dialogBreaks;
        }

        /// <summary>
        /// Decomposes the dialog string into individual words.
        /// </summary>
        /// <returns>A list of words.</returns>
        private List<string> DecomposeString()
        {
            List<string> words = new List<string>();
            int start = 0;
            char[] whiteSpace = {' ', '\t'}; //add other characters that could be broken up.

            while (true) //omg
            {
                int index = dialog.IndexOfAny(whiteSpace, start);

                if (index == -1)
                { //return case if no spaces or tabs are left
                    words.Add(dialog.Substring(start));
                    return words;
                }
                words.Add(dialog.Substring(start, index - start));
                start = index + 1;
            }
        }

        /// <summary>
        /// Gets whether the mouse is overing over a choice
        /// </summary>
        /// <returns>True is hovering over a choice, false if not.</returns>
        private bool MouseOver(string text, float yPos, int index)
        {
            float mx, my, choiceStartX;
            mx = GameLogic.GameGlobal.InputControl.CurrentMouseState.X;
            my = GameLogic.GameGlobal.InputControl.CurrentMouseState.Y;
            choiceStartX = (bounds.Width / 15f) + bounds.Left;

            if (mx >= choiceStartX && mx <= choiceStartX + GameLogic.GameGlobal.gameFont.MeasureString(index + ") " + text).X &&
                my >= yPos && my <= yPos + GameLogic.GameGlobal.gameFont.MeasureString(text).Y)
            {
                return true;
            }
            return false;
        }

        public void unloadDialogBox()
        {
            bounds = Rectangle.Empty;
            dialog = "";
            for (int u = 0; u < choices.Length; u++)
            {
                choices[u] = "";
            }
            texture = null;
        }
    }
}

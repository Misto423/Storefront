using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Storefront.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.GameLogic.OfficeMenuClasses
{
    public class PlayerView : SubMenuView
    {
        private SFButton[] statButtons = new SFButton[14];
        private SFButton acceptStats;
        private SFButton closeBtn;
        private SFButton backBtn;
        private bool btnToggle = false;
        private int xPos, yPos;
        private bool newLevel = true;
        private byte statTip = 0;

        /// <summary>
        /// Allow points to be allocated again.
        /// </summary>
        public void LevelUp()
        {
            newLevel = true;
        }

        public override void InitView(ContentManager cm)
        {
            xPos = GameGlobal.GameWidth;
            xPos = (int)(xPos / 1.35);
            yPos = GameGlobal.GameHeight;
            yPos = (int)(yPos / 20);

            //setup positions of all the stats buttons
            for (int index = 0; index < statButtons.Length; index++)
            {
                if (!btnToggle)
                {
                    statButtons[index] = new SFButton(new Rectangle(xPos, yPos, 30, 30), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "-", false, true, true);
                    xPos += 100;
                }
                else
                {
                    statButtons[index] = new SFButton(new Rectangle(xPos, yPos, 30, 30), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "+", false, true, true);
                    xPos -= 100;
                }
                if (index % 2 == 1)
                {
                    yPos += 60;
                }
                btnToggle = !btnToggle;
            }

            acceptStats = new SFButton(new Rectangle((int)(GameGlobal.GameWidth / 1.35), (int)(GameGlobal.GameHeight / 1.35), 130, 30), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover,
                                        Graphics.GlobalGfx.btnActive, "Accept", false, true, true);

            closeBtn = new SFButton(new Rectangle(0, GameGlobal.GameHeight / 2, 130, 30), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover,
                                    Graphics.GlobalGfx.btnActive, "Close", false, true, true);

            backBtn = new SFButton(new Rectangle(0, GameGlobal.GameHeight / 2 + 30, 130, 30), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover,
                                Graphics.GlobalGfx.btnActive, "Back", false, true, true);
        }

        public override void LoadView()
        {
            
        }

        public override void UnloadView()
        {
            int l = statButtons.Length;
            for (int u = 0; u < l; u++)
            {
                statButtons[u].unloadBtn();
            }
            acceptStats.unloadBtn();
            closeBtn.unloadBtn();
            if (backBtn != null)
            {
                backBtn.unloadBtn();
            }
        }

        public override void UpdateView(GameTime gt, out bool? stateSwitch)
        {
            stateSwitch = null;
            //get length of button array for loops
            int numButtons = statButtons.Length;
            //update the accept button
            acceptStats.updateButton();
            //update clsoe button
            closeBtn.updateButton();
            //close the screen if close button clicked
            if (closeBtn.isDown())
            {
                stateSwitch = true;
                return;
            }

            //update back button
            backBtn.updateButton();
            if (backBtn.isDown())
            {
                stateSwitch = true;
                return;
            }
            //allow allocating points on a new level, but not after accept button is clicked until a new level occurs again
            if (newLevel)
            {
                //if no points are available but the accept button is not yet clicked
                if (GameGlobal.player.AvailablePoints <= 0 && !acceptStats.isDown())
                {
                    //remove the increment buttons for each stat
                    for (int index = 1; index < numButtons; index += 2)
                    {
                        statButtons[index].IsEnabled = false;
                        statButtons[index].IsVisible = false;
                        //statButtons[index].updateButton();
                    }
                    //allow button actions
                    ButtonActions();
                }
                //if there are points still available and the accept button is not yet clicked
                else if (GameGlobal.player.AvailablePoints > 0 && !acceptStats.isDown())
                {
                    //allow both increment and decrement buttons
                    for (int index = 0; index < numButtons; index++)
                    {
                        //statButtons[index].updateButton();
                        statButtons[index].IsEnabled = true;
                        statButtons[index].IsVisible = true;
                    }
                    //allow button actions
                    ButtonActions();
                }
                else if (acceptStats.isDown()) //if the button was pressed to accept stat changes
                {
                    newLevel = false;
                    //remove all increment and decrement buttons
                    for (int index = 0; index < numButtons; index++)
                    {
                        statButtons[index].IsEnabled = false;
                        statButtons[index].IsVisible = false;
                    }
                }
            }

            //update all the buttons after attributes change
            for (int index = 0; index < numButtons; index++)
            {
                statButtons[index].updateButton();
            }

            //check for mouse position to display the stat descriptions
            #region Stat Desc
            if (GameGlobal.InputControl.CurrentMouseState.X >= (int)(GameGlobal.GameWidth / 1.25) - 200 &&
                GameGlobal.InputControl.CurrentMouseState.Y >= (int)(GameGlobal.GameHeight / 20) &&
                GameGlobal.InputControl.CurrentMouseState.Y < (int)(GameGlobal.GameHeight / 20) + 60)
            {
                statTip = 1;
            }
            else if (GameGlobal.InputControl.CurrentMouseState.X >= (int)(GameGlobal.GameWidth / 1.25) - 200 &&
                GameGlobal.InputControl.CurrentMouseState.Y >= (int)(GameGlobal.GameHeight / 20) + 60 &&
                GameGlobal.InputControl.CurrentMouseState.Y < (int)(GameGlobal.GameHeight / 20) + 120)
            {
                statTip = 2;
            }
            else if (GameGlobal.InputControl.CurrentMouseState.X >= (int)(GameGlobal.GameWidth / 1.25) - 200 &&
                    GameGlobal.InputControl.CurrentMouseState.Y >= (int)(GameGlobal.GameHeight / 20) + 120 &&
                    GameGlobal.InputControl.CurrentMouseState.Y < (int)(GameGlobal.GameHeight / 20) + 180)
            {
                statTip = 3;
            }
            else if (GameGlobal.InputControl.CurrentMouseState.X >= (int)(GameGlobal.GameWidth / 1.25) - 200 &&
                    GameGlobal.InputControl.CurrentMouseState.Y >= (int)(GameGlobal.GameHeight / 20) + 180 &&
                    GameGlobal.InputControl.CurrentMouseState.Y < (int)(GameGlobal.GameHeight / 20) + 240)
            {
                statTip = 4;
            }
            else if (GameGlobal.InputControl.CurrentMouseState.X >= (int)(GameGlobal.GameWidth / 1.25) - 200 &&
                    GameGlobal.InputControl.CurrentMouseState.Y >= (int)(GameGlobal.GameHeight / 20) + 240 &&
                    GameGlobal.InputControl.CurrentMouseState.Y < (int)(GameGlobal.GameHeight / 20) + 300)
            {
                statTip = 5;
            }
            else if (GameGlobal.InputControl.CurrentMouseState.X >= (int)(GameGlobal.GameWidth / 1.25) - 200 &&
                    GameGlobal.InputControl.CurrentMouseState.Y >= (int)(GameGlobal.GameHeight / 20) + 300 &&
                    GameGlobal.InputControl.CurrentMouseState.Y < (int)(GameGlobal.GameHeight / 20) + 360)
            {
                statTip = 6;
            }
            else if (GameGlobal.InputControl.CurrentMouseState.X >= (int)(GameGlobal.GameWidth / 1.25) - 200 &&
                    GameGlobal.InputControl.CurrentMouseState.Y >= (int)(GameGlobal.GameHeight / 20) + 360 &&
                    GameGlobal.InputControl.CurrentMouseState.Y < (int)(GameGlobal.GameHeight / 20) + 420)
            {
                statTip = 7;
            }
            else
            {
                statTip = 0;
            }
            #endregion
        }

        /// <summary>
        /// All the click events for adding and removing points available.
        /// </summary>
        public void ButtonActions()
        {
            //Strength Buttons
            if (statButtons[0].isDown() && GameGlobal.player.Strength > GameGlobal.player.StartingStats[0])
            {
                GameGlobal.player.Strength -= 1;
                GameGlobal.player.AvailablePoints += 1;
            }
            if (statButtons[1].isDown())
            {
                GameGlobal.player.Strength += 1;
                GameGlobal.player.AvailablePoints -= 1;
            }
            //Intelligence Buttons
            if (statButtons[2].isDown() && GameGlobal.player.Intelligence > GameGlobal.player.StartingStats[1])
            {
                GameGlobal.player.Intelligence -= 1;
                GameGlobal.player.AvailablePoints += 1;
            }
            if (statButtons[3].isDown())
            {
                GameGlobal.player.Intelligence += 1;
                GameGlobal.player.AvailablePoints -= 1;
            }
            //Awareness Buttons
            if (statButtons[4].isDown() && GameGlobal.player.Awareness > GameGlobal.player.StartingStats[2])
            {
                GameGlobal.player.Awareness -= 1;
                GameGlobal.player.AvailablePoints += 1;
            }
            if (statButtons[5].isDown())
            {
                GameGlobal.player.Awareness += 1;
                GameGlobal.player.AvailablePoints -= 1;
            }
            //Persuasion Buttons
            if (statButtons[6].isDown() && GameGlobal.player.Persuasion > GameGlobal.player.StartingStats[3])
            {
                GameGlobal.player.Persuasion -= 1;
                GameGlobal.player.AvailablePoints += 1;
            }
            if (statButtons[7].isDown())
            {
                GameGlobal.player.Persuasion += 1;
                GameGlobal.player.AvailablePoints -= 1;
            }
            //Barter Buttons
            if (statButtons[8].isDown() && GameGlobal.player.Barter > GameGlobal.player.StartingStats[4])
            {
                GameGlobal.player.Barter -= 1;
                GameGlobal.player.AvailablePoints += 1;
            }
            if (statButtons[9].isDown())
            {
                GameGlobal.player.Barter += 1;
                GameGlobal.player.AvailablePoints -= 1;
            }
            //Maintenance Buttons
            if (statButtons[10].isDown() && GameGlobal.player.Maintenance > GameGlobal.player.StartingStats[5])
            {
                GameGlobal.player.Maintenance -= 1;
                GameGlobal.player.AvailablePoints += 1;
            }
            if (statButtons[11].isDown())
            {
                GameGlobal.player.Maintenance += 1;
                GameGlobal.player.AvailablePoints -= 1;
            }
            //Special Buttons
            if (statButtons[12].isDown() && GameGlobal.player.StoreStat > GameGlobal.player.StartingStats[6])
            {
                GameGlobal.player.StoreStat -= 1;
                GameGlobal.player.AvailablePoints += 1;
            }
            if (statButtons[13].isDown())
            {
                GameGlobal.player.StoreStat += 1;
                GameGlobal.player.AvailablePoints -= 1;
            }
        }

        public override void DrawView(GameTime gt, SpriteBatch sb)
        {
            sb.Begin();
            //reset label positions
            xPos = GameGlobal.GameWidth;
            xPos = (int)(xPos / 1.25);
            yPos = GameGlobal.GameHeight;
            yPos = (int)(yPos / 20);

            sb.DrawString(GameGlobal.gameFont, "Strength", new Vector2(xPos - 200, yPos), Color.SpringGreen);
            sb.DrawString(GameGlobal.gameFont, "Intelligence", new Vector2(xPos - 200, yPos + 60), Color.SpringGreen);
            sb.DrawString(GameGlobal.gameFont, "Awareness", new Vector2(xPos - 200, yPos + 120), Color.SpringGreen);
            sb.DrawString(GameGlobal.gameFont, "Persuasion", new Vector2(xPos - 200, yPos + 180), Color.SpringGreen);
            sb.DrawString(GameGlobal.gameFont, "Barter", new Vector2(xPos - 200, yPos + 240), Color.SpringGreen);
            sb.DrawString(GameGlobal.gameFont, "Maintenance", new Vector2(xPos - 200, yPos + 300), Color.SpringGreen);
            //switch statement based on store, to display the store specific ability
            switch (GameGlobal.player.PlayerStore.ToString())
            {
                case "Equipment Store":
                    sb.DrawString(GameGlobal.gameFont, "Blacksmith", new Vector2(xPos - 200, yPos + 360), Color.SpringGreen);
                    break;
                case "General Store":
                    sb.DrawString(GameGlobal.gameFont, "Survival", new Vector2(xPos - 200, yPos + 360), Color.SpringGreen);
                    break;
                case "Alchemy Lab":
                    sb.DrawString(GameGlobal.gameFont, "Alchemy", new Vector2(xPos - 200, yPos + 360), Color.SpringGreen);
                    break;
                case "Stables":
                    sb.DrawString(GameGlobal.gameFont, "Veterinary Med", new Vector2(xPos - 200, yPos + 360), Color.SpringGreen);
                    break;
                case "Inn":
                    sb.DrawString(GameGlobal.gameFont, "Medicine", new Vector2(xPos - 200, yPos + 360), Color.SpringGreen);
                    break;
                case "Bar":
                    sb.DrawString(GameGlobal.gameFont, "Cooking", new Vector2(xPos - 200, yPos + 360), Color.SpringGreen);
                    break;
                default:

                    break;
            }

            //draw values for stats
            sb.DrawString(GameGlobal.gameFont, GameGlobal.player.Strength.ToString(), new Vector2(xPos + 11, yPos), Color.SpringGreen);
            sb.DrawString(GameGlobal.gameFont, GameGlobal.player.Intelligence.ToString(), new Vector2(xPos + 11, yPos + 60), Color.SpringGreen);
            sb.DrawString(GameGlobal.gameFont, GameGlobal.player.Awareness.ToString(), new Vector2(xPos + 11, yPos + 120), Color.SpringGreen);
            sb.DrawString(GameGlobal.gameFont, GameGlobal.player.Persuasion.ToString(), new Vector2(xPos + 11, yPos + 180), Color.SpringGreen);
            sb.DrawString(GameGlobal.gameFont, GameGlobal.player.Barter.ToString(), new Vector2(xPos + 11, yPos + 240), Color.SpringGreen);
            sb.DrawString(GameGlobal.gameFont, GameGlobal.player.Maintenance.ToString(), new Vector2(xPos + 11, yPos + 300), Color.SpringGreen);
            sb.DrawString(GameGlobal.gameFont, GameGlobal.player.StoreStat.ToString(), new Vector2(xPos + 11, yPos + 360), Color.SpringGreen);

            //draw placeholder lines to section off the player name, race, gender.
            Graphics.SimpleDraw.drawLine(sb, new Vector2(0, GameGlobal.GameHeight / 2), new Vector2(GameGlobal.GameWidth / 2.5f, GameGlobal.GameHeight / 2), 1, Color.Gainsboro);
            Graphics.SimpleDraw.drawLine(sb, new Vector2(GameGlobal.GameWidth / 2.5f, 0), new Vector2(GameGlobal.GameWidth / 2.5f, GameGlobal.GameHeight / 2), 1, Color.Gainsboro);
            //Draw the player's name
            sb.DrawString(GameGlobal.gameFont, GameGlobal.player.Name, new Vector2(GameGlobal.GameWidth / 30, GameGlobal.GameHeight / 20), Color.Goldenrod);
            //Draw Race and Gender to the Screen
            if (GameGlobal.player.Gender)
            {
                sb.DrawString(GameGlobal.gameFont, "Male " + GameGlobal.player.PlayerRace.ToString(), new Vector2(GameGlobal.GameWidth / 30, GameGlobal.GameHeight / 10), Color.Goldenrod);
            }
            else
            {
                sb.DrawString(GameGlobal.gameFont, "Female " + GameGlobal.player.PlayerRace.ToString(), new Vector2(GameGlobal.GameWidth / 30, GameGlobal.GameHeight / 10), Color.Goldenrod);
            }

            //draw store information
            sb.DrawString(GameGlobal.gameFont, "Owner of the ", new Vector2(GameGlobal.GameWidth / 30, GameGlobal.GameHeight / 6.75f), Color.Goldenrod);
            sb.DrawString(GameGlobal.gameFont, GameGlobal.player.PlayerStore.Name, new Vector2(GameGlobal.GameWidth / 30, GameGlobal.GameHeight / 5f), Color.Goldenrod);
            sb.DrawString(GameGlobal.gameFont, GameGlobal.player.PlayerStore.ToString(), new Vector2(GameGlobal.GameWidth / 30, GameGlobal.GameHeight / 4), Color.Goldenrod);
            sb.DrawString(GameGlobal.gameFont, "Available Points: " + GameGlobal.player.AvailablePoints, new Vector2(GameGlobal.GameWidth / 30, GameGlobal.GameHeight / 3.25f), Color.Goldenrod);

            //draw stat tip info
            #region Drawing Stat Desc.
            switch (statTip)
            {
                case 1:
                    sb.DrawString(GameGlobal.gameFont, "Effects how well you are able to defend yourself and your store.", new Vector2(GameGlobal.GameWidth / 30, GameGlobal.GameHeight / 1.15f), Color.Goldenrod);
                    break;
                case 2:
                    sb.DrawString(GameGlobal.gameFont, "Effects your ability to research new ideas and outsmart others.", new Vector2(GameGlobal.GameWidth / 30, GameGlobal.GameHeight / 1.15f), Color.Goldenrod);
                    break;
                case 3:
                    sb.DrawString(GameGlobal.gameFont, "Effects how you percieve customers and their actions.", new Vector2(GameGlobal.GameWidth / 30, GameGlobal.GameHeight / 1.15f), Color.Goldenrod);
                    break;
                case 4:
                    sb.DrawString(GameGlobal.gameFont, "Effects how well you can get information or items from customers.", new Vector2(GameGlobal.GameWidth / 30, GameGlobal.GameHeight / 1.15f), Color.Goldenrod);
                    break;
                case 5:
                    sb.DrawString(GameGlobal.gameFont, "Effects your ability to buy and sell goods at benefical prices.", new Vector2(GameGlobal.GameWidth / 30, GameGlobal.GameHeight / 1.15f), Color.Goldenrod);
                    break;
                case 6:
                    sb.DrawString(GameGlobal.gameFont, "Effects how well you can repair your store and its equipment.", new Vector2(GameGlobal.GameWidth / 30, GameGlobal.GameHeight / 1.15f), Color.Goldenrod);
                    break;
                case 7:
                    switch (GameGlobal.player.PlayerStore.ToString())
                    {
                        case "Equipment Store":
                            sb.DrawString(GameGlobal.gameFont, "Effects what type of weapons and armor you can buy or make.", new Vector2(GameGlobal.GameWidth / 30, GameGlobal.GameHeight / 1.15f), Color.Goldenrod);
                            break;
                        case "General Store":
                            sb.DrawString(GameGlobal.gameFont, "Effects your knowledge of surviving various situations.", new Vector2(GameGlobal.GameWidth / 30, GameGlobal.GameHeight / 1.15f), Color.Goldenrod);
                            break;
                        case "Alchemy Lab":
                            sb.DrawString(GameGlobal.gameFont, "Effects the types and success of making potions.", new Vector2(GameGlobal.GameWidth / 30, GameGlobal.GameHeight / 1.15f), Color.Goldenrod);
                            break;
                        case "Stables":
                            sb.DrawString(GameGlobal.gameFont, "Effects how well you can treat injured animals.", new Vector2(GameGlobal.GameWidth / 30, GameGlobal.GameHeight / 1.15f), Color.Goldenrod);
                            break;
                        case "Inn":
                            sb.DrawString(GameGlobal.gameFont, "Effects how well you can treat injured people.", new Vector2(GameGlobal.GameWidth / 30, GameGlobal.GameHeight / 1.15f), Color.Goldenrod);
                            break;
                        case "Bar":
                            sb.DrawString(GameGlobal.gameFont, "Effects the types of food/meals that can be created.", new Vector2(GameGlobal.GameWidth / 30, GameGlobal.GameHeight / 1.15f), Color.Goldenrod);
                            break;
                        default:

                            break;
                    }
                    break;
                default:
                    break;
            }
            #endregion
            sb.End();

            //draw accept button and increment/decrement buttons
            acceptStats.drawButton(sb, true);
            closeBtn.drawButton(sb, true);

            backBtn.drawButton(sb, true);

            for (int index = 0; index < statButtons.Length; index++)
            {
                statButtons[index].drawButton(sb, true);
            }
        }
    }
}

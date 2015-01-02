using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Storefront.Input;

namespace Storefront.GameLogic.OfficeMenuClasses
{
    public class EmployView : SubMenuView
    {
        private SFButton assignBtn;
        private SFButton hireBtn;
        private SFButton acceptBtn;
        private SFButton closeBtn;
        private SFTextField wageField;

        private SFButton employedUpBtn, employedDownBtn;

        private int selectedAppIndex = -1;
        private int selectedHireIndex = -1;

        private int lineSpacing;
        private int employedStartIndex;

        private List<MainGamePlay.Employees.EmployeeMain> applicants;

        public override void InitView(ContentManager cm)
        {
            lineSpacing = (((int)(GameGlobal.GameHeight / 1.3f)) - (int)(GameGlobal.GameHeight / 20f)) / 8 + 1;

            GenerateRandomApplicants();

            //setup buttons
            employedUpBtn = new SFButton(
                new Rectangle(GameGlobal.GameWidth - (GameGlobal.GameWidth / 25), GameGlobal.GameHeight / 20,
                    (GameGlobal.GameWidth / 25), lineSpacing), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover,
                    Graphics.GlobalGfx.btnActive, "^", false, true, true);

            employedDownBtn = new SFButton(
                new Rectangle(GameGlobal.GameWidth - (GameGlobal.GameWidth / 25), (int)(GameGlobal.GameHeight / 1.3f) - lineSpacing,
                    (GameGlobal.GameWidth / 25), lineSpacing), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover,
                    Graphics.GlobalGfx.btnActive, "V", false, true, true);

        }

        public override void LoadView()
        {

        }

        public override void UpdateView(GameTime gt, out bool? stateSwitch)
        {
            stateSwitch = null;
            //get which field is selected
            getSelectedField();

            //update buttons
            employedDownBtn.updateButton();
            employedUpBtn.updateButton();


        }

        public override void DrawView(GameTime gt, SpriteBatch sb)
        {
            //draw buttons
            employedUpBtn.drawButton(sb, true);
            employedDownBtn.drawButton(sb, true);

            sb.Begin();
            #region Tables
            //vertical lines
            Graphics.SimpleDraw.drawLine(sb, new Vector2(GameGlobal.GameWidth / 2, 0),
                new Vector2(GameGlobal.GameWidth / 2, (int)(GameGlobal.GameHeight / 1.3f)), 3.0f, Color.DarkKhaki);

            Graphics.SimpleDraw.drawLine(sb, new Vector2(GameGlobal.GameWidth - employedUpBtn.Bounds.Width, (int)(GameGlobal.GameHeight / 20f)),
                new Vector2(GameGlobal.GameWidth - employedUpBtn.Bounds.Width, (int)(GameGlobal.GameHeight / 1.3f)), 1.0f, Color.DarkKhaki);

            int count = 0;
            for (int index = (int)(GameGlobal.GameHeight / 20f); 
                index <= (int)(GameGlobal.GameHeight / 1.3f) + 1; index += lineSpacing)
            {
                //dividers
                Graphics.SimpleDraw.drawLine(sb, new Vector2(0, index),
                    new Vector2(GameGlobal.GameWidth - employedUpBtn.Bounds.Width, index), 1.0f, Color.DarkKhaki);
                //employee info
                if (count < applicants.Count)
                {
                    sb.DrawString(GameGlobal.gameFont, applicants[count].Name, new Vector2(10, index), Color.DarkKhaki);
                    string occupation = applicants[count].GetType().Name;
                    sb.DrawString(GameGlobal.gameFont, occupation,
                        new Vector2(10, index + GameGlobal.gameFont.MeasureString(occupation).Y), Color.DarkKhaki);
                    count++;
                }
            }

            //draw headings
            sb.DrawString(GameGlobal.gameFont, "Applicants",
                new Vector2((GameGlobal.GameWidth / 4f) - (GameGlobal.gameFont.MeasureString("Applicants").X / 2f), 0),
                Color.DarkKhaki);

            sb.DrawString(GameGlobal.gameFont, "Employed",
                new Vector2((3 * GameGlobal.GameWidth / 4f) - (GameGlobal.gameFont.MeasureString("Employed").X / 2f), 0),
                Color.DarkKhaki);

            #endregion

            #region Highlight Selection
            if (selectedAppIndex > -1 && selectedAppIndex < 8)
            {
                Graphics.SimpleDraw.fillArea(sb, 
                    new Rectangle(0, (int)((GameGlobal.GameHeight / 20f) + (selectedAppIndex * lineSpacing)),
                        GameGlobal.GameWidth / 2, lineSpacing), new Color(128, 56, 56, 96));
            }
            if (selectedHireIndex > -1 && selectedHireIndex < 8)
            {
                Graphics.SimpleDraw.fillArea(sb,
                    new Rectangle(GameGlobal.GameWidth/2, (int)((GameGlobal.GameHeight / 20f) + (selectedHireIndex * lineSpacing)),
                        GameGlobal.GameWidth / 2 - employedUpBtn.Bounds.Width, lineSpacing), new Color(128, 56, 56, 96));
            }
            #endregion

            sb.End();
        }

        public override void UnloadView()
        {
            throw new NotImplementedException();
        }

        private void GenerateRandomApplicants()
        {
            Random rand = new Random();
            applicants = new List<MainGamePlay.Employees.EmployeeMain>();

            for (int index = 0; index < 8; index++)
            {
                switch (rand.Next(1, 5))
                {
                    default:
                    case 1:
                        applicants.Add(new MainGamePlay.Employees.Clerk(rand));
                        break;
                    case 2:
                        applicants.Add(new MainGamePlay.Employees.Slave(rand));
                        break;
                    case 3:
                        applicants.Add(new MainGamePlay.Employees.Maintenance(rand));
                        break;
                    case 4:
                        applicants.Add(new MainGamePlay.Employees.Guard(rand));
                        break;
                }
            }
        }

        private void getSelectedField()
        {
            if (GameGlobal.InputControl.IsNewPress(MouseBtns.LeftClick))
            {
                //get applicant selection
                if (GameGlobal.InputControl.CurrentMouseState.X < (GameGlobal.GameWidth / 2 - 1))
                {
                    if (GameGlobal.InputControl.CurrentMouseState.Y >= (int)(GameGlobal.GameHeight / 20f) &&
                        GameGlobal.InputControl.CurrentMouseState.Y < (int)(GameGlobal.GameHeight / 20f) + (8 * lineSpacing))
                    {
                        selectedAppIndex = (int)((GameGlobal.InputControl.CurrentMouseState.Y + (GameGlobal.GameHeight / 20f)) / lineSpacing) - 1;
                        selectedHireIndex = -1;
                    }
                }
                else if (GameGlobal.InputControl.CurrentMouseState.X > (GameGlobal.GameWidth / 2) &&
                    GameGlobal.InputControl.CurrentMouseState.X < (GameGlobal.GameWidth - employedUpBtn.Bounds.Width))
                    //get hired selection
                {
                    if (GameGlobal.InputControl.CurrentMouseState.Y >= (int)(GameGlobal.GameHeight / 20f) &&
                        GameGlobal.InputControl.CurrentMouseState.Y <= (int)(GameGlobal.GameHeight / 20f) + (8 * lineSpacing))
                    {
                        selectedHireIndex = (int)((GameGlobal.InputControl.CurrentMouseState.Y + (GameGlobal.GameHeight / 20f)) / lineSpacing) - 1;
                        selectedAppIndex = -1;
                    }
                }
            }
        }
    }
}

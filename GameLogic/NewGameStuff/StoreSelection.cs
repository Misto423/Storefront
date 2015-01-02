using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Storefront.Input;

namespace Storefront.GameLogic.NewGameStuff
{
    public class StoreSelection
    {
        private SFButton acceptBtn, leftBtn, rightBtn, layoutABtn, layoutBBtn, layoutCBtn, layoutDBtn;
        private SFRadioButtons layoutBtns;
        private SFTextField nameField;
        private SFButton backBtn;
        private byte storeIndex = 0;
        private Stores.Layout layoutPreview = new Stores.Layout(1);
        private Vector2 previewCoord;
        private int layoutWidth, layoutHeight;

        public void initStoreSelection(ContentManager cm)
        {
            //init vector position for layout prevoew
            previewCoord = new Vector2();

            //initialize all buttons
            //accept button
            acceptBtn = new SFButton(new Rectangle(0, 0, 75, 50), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "Accept", false, true, true);
            //back button
            backBtn = new SFButton(new Rectangle(0, acceptBtn.Bounds.Bottom, 75, 50), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "Back", false, true, true);
            //left selection button
            leftBtn = new SFButton(new Rectangle(10, GameGlobal.GameHeight / 3, 25, 25), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "", false, true, true);
            //right selection button
            rightBtn = new SFButton(new Rectangle(GameGlobal.GameWidth - 35, GameGlobal.GameHeight / 3, 25, 25), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "", false, true, true);
            //layout A button
            layoutABtn = new SFButton(new Rectangle((int)(GameGlobal.GameWidth / (800f / 700f)), 0, 50, 50), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "1", true, true, true);
            layoutABtn.ButtonState = SFButtonState.Down;
            //layout B button
            layoutBBtn = new SFButton(new Rectangle((int)(GameGlobal.GameWidth / (800f / 700f)) + 50, 0, 50, 50), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "2", true, true, true);
            //layout C button
            layoutCBtn = new SFButton(new Rectangle((int)(GameGlobal.GameWidth / (800f / 700f)), 50, 50, 50), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "3", true, true, true);
            //layout D button
            layoutDBtn = new SFButton(new Rectangle((int)(GameGlobal.GameWidth / (800f / 700f)) + 50, 50, 50, 50), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "4", true, true, true);
            //init radio buttons and add layout buttons to set
            layoutBtns = new SFRadioButtons();
            layoutBtns.addButton(layoutABtn);
            layoutBtns.addButton(layoutBBtn);
            layoutBtns.addButton(layoutCBtn);
            layoutBtns.addButton(layoutDBtn);

            //load text field for inputting name of the store
            nameField = new SFTextField(new Rectangle((int)(GameGlobal.GameWidth / 2) - 150, 5, 300, 30), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.fieldCursor, 25);

            Stores.RoomDataFiller.loadRoomTextures(cm);
        }

        public void updateStoreSelection(out bool? stateSwitch)
        {
            //update buttons
            acceptBtn.updateButton();
            leftBtn.updateButton();
            rightBtn.updateButton();
            layoutBtns.updateRadioBtns();
            nameField.updateText();
            backBtn.updateButton();

            //button actions
            //update the store index for left button
            if (leftBtn.isDown())
            {
                if (storeIndex == 0)
                {
                    storeIndex = 5;
                }
                else
                {
                    storeIndex--;
                }
            }
            //update the store index for right button
            if (rightBtn.isDown())
            {
                if (storeIndex == 5)
                {
                    storeIndex = 0;
                }
                else
                {
                    storeIndex++;
                }
            }
            if (layoutBtns.CurrentSelectedIndex == 0)
            {
                layoutPreview.LayoutIndex = 1;
            }
            else if (layoutBtns.CurrentSelectedIndex == 1)
            {
                layoutPreview.LayoutIndex = 2;
            }
            else if (layoutBtns.CurrentSelectedIndex == 2)
            {
                layoutPreview.LayoutIndex = 3;
            }
            else if (layoutBtns.CurrentSelectedIndex == 3)
            {
                layoutPreview.LayoutIndex = 4;
            }

            stateSwitch = false;
            if (backBtn.isDown())
            {
                stateSwitch = null;
            }
            //if the accept button is pressed and there is a name for the store.
            if (acceptBtn.isDown() && nameField.Text.Length > 0)
            {
                //assign player store.
                switch (storeIndex)
                {
                    default:
                    case 0:
                        GameGlobal.player.PlayerStore = new Stores.EquipmentStore(nameField.Text, layoutPreview.LayoutIndex);
                        break;
                    case 1:
                        GameGlobal.player.PlayerStore = new Stores.GeneralStore(nameField.Text, layoutPreview.LayoutIndex);
                        break;
                    case 2:
                        GameGlobal.player.PlayerStore = new Stores.AlchemyLab(nameField.Text, layoutPreview.LayoutIndex);
                        break;
                    case 3:
                        GameGlobal.player.PlayerStore = new Stores.Stables(nameField.Text, layoutPreview.LayoutIndex);
                        break;
                    case 4:
                        GameGlobal.player.PlayerStore = new Stores.Inn(nameField.Text, layoutPreview.LayoutIndex);
                        break;
                    case 5:
                        GameGlobal.player.PlayerStore = new Stores.Bar(nameField.Text, layoutPreview.LayoutIndex);
                        break;
                }
                stateSwitch = true;
            }
        }

        public void drawStoreSelection(SpriteBatch sb, GameTime gt)
        {
            sb.Begin();
            //draw dividers (placeholders till art)
            Graphics.SimpleDraw.drawLine(sb, new Vector2(0, GameGlobal.GameHeight / 1.5f), new Vector2(GameGlobal.GameWidth, GameGlobal.GameHeight / 1.5f), 1, Color.Gainsboro);
            Graphics.SimpleDraw.drawLine(sb, new Vector2(0, GameGlobal.GameHeight / 1.2f), new Vector2(GameGlobal.GameWidth, GameGlobal.GameHeight / 1.2f), 1, Color.Gainsboro);
            Graphics.SimpleDraw.drawLine(sb, new Vector2(GameGlobal.GameWidth / 2, GameGlobal.GameHeight / 1.5f), new Vector2(GameGlobal.GameWidth / 2, GameGlobal.GameHeight / 1.2f), 1, Color.Gainsboro);
            
            //draw label of selected store
            #region Store Descriptions Drawing

            int xSP, ySP;
            xSP = GameGlobal.GameWidth / 20;
            ySP = GameGlobal.GameHeight - 95;

            switch (storeIndex)
            {
                case 0:
                    //store type (under name of store)
                    sb.DrawString(GameGlobal.gameFont, "Equipment Store", centerName(), Color.Gainsboro);
                    //Description of store
                    sb.DrawString(GameGlobal.gameFont, "Deals in weapons and armor.", new Vector2(10, GameGlobal.GameHeight / 1.4f), Color.Gainsboro);
                    //ability and stat for selected store
                    sb.DrawString(GameGlobal.gameFont, "Craft: Make Weapons and armor\n       from materials.\nStat:  Blacksmith", 
                        new Vector2(GameGlobal.GameWidth / 2 + 10, GameGlobal.GameHeight / 1.45f), Color.Gainsboro);
                    foreach (Texture2D r in Stores.RoomDataFiller.eqRooms)
                    {
                        sb.Draw(r, new Vector2(xSP, ySP), Color.White);
                        xSP += 150;
                    }
                    break;
                case 1:
                    sb.DrawString(GameGlobal.gameFont, "General Store", centerName(), Color.Gainsboro);
                    sb.DrawString(GameGlobal.gameFont, "Deals in clothing, food,\n and general supplies.", new Vector2(10, GameGlobal.GameHeight / 1.4f), Color.Gainsboro);
                    sb.DrawString(GameGlobal.gameFont, "Variety: Can store more in\n         shelving units.\nStat:    Survival",
                        new Vector2(GameGlobal.GameWidth / 2 + 10, GameGlobal.GameHeight / 1.45f), Color.Gainsboro);
                    foreach (Texture2D r in Stores.RoomDataFiller.genRooms)
                    {
                        sb.Draw(r, new Vector2(xSP, ySP), Color.White);
                        xSP += 150;
                    }
                    break;
                case 2:
                    sb.DrawString(GameGlobal.gameFont, "Alchemy Lab", centerName(), Color.Gainsboro);
                    sb.DrawString(GameGlobal.gameFont, "Deals in magic items and potions.", new Vector2(10, GameGlobal.GameHeight / 1.4f), Color.Gainsboro);
                    sb.DrawString(GameGlobal.gameFont, "Chemistry: Can craft magic potions.\nStat:      Alchemy",
                        new Vector2(GameGlobal.GameWidth / 2 + 10, GameGlobal.GameHeight / 1.4f), Color.Gainsboro);
                    foreach (Texture2D r in Stores.RoomDataFiller.alcRooms)
                    {
                        sb.Draw(r, new Vector2(xSP, ySP), Color.White);
                        xSP += 150;
                    }
                    break;
                case 3:
                    sb.DrawString(GameGlobal.gameFont, "Stables", centerName(), Color.Gainsboro);
                    sb.DrawString(GameGlobal.gameFont, "Cares for animals and provides\n transportation services.", new Vector2(10, GameGlobal.GameHeight / 1.4f), Color.Gainsboro);
                    sb.DrawString(GameGlobal.gameFont, "Breed: Can breed animals for\n       transportation or food.\nStat:  Veterinary Med",
                        new Vector2(GameGlobal.GameWidth / 2 + 10, GameGlobal.GameHeight / 1.45f), Color.Gainsboro);
                    foreach (Texture2D r in Stores.RoomDataFiller.stRooms)
                    {
                        sb.Draw(r, new Vector2(xSP, ySP), Color.White);
                        xSP += 150;
                    }
                    break;
                case 4:
                    sb.DrawString(GameGlobal.gameFont, "Inn", centerName(), Color.Gainsboro);
                    sb.DrawString(GameGlobal.gameFont, "Provides a place to rest\n for travelers.", new Vector2(10, GameGlobal.GameHeight / 1.4f), Color.Gainsboro);
                    sb.DrawString(GameGlobal.gameFont, "Heal: Can heal status effects\n      on travelers.\nStat: Medicine",
                        new Vector2(GameGlobal.GameWidth / 2 + 10, GameGlobal.GameHeight / 1.45f), Color.Gainsboro);
                    foreach (Texture2D r in Stores.RoomDataFiller.innRooms)
                    {
                        sb.Draw(r, new Vector2(xSP, ySP), Color.White);
                        xSP += 150;
                    }
                    break;
                case 5:
                    sb.DrawString(GameGlobal.gameFont, "Bar", centerName(), Color.Gainsboro);
                    sb.DrawString(GameGlobal.gameFont, "Where most socializing and shady\n dealings take place.", new Vector2(10, GameGlobal.GameHeight / 1.4f), Color.Gainsboro);
                    sb.DrawString(GameGlobal.gameFont, "Steal: Can steal information or\n       items from patrons.\nStat:  Cooking",
                        new Vector2(GameGlobal.GameWidth / 2 + 10, GameGlobal.GameHeight / 1.45f), Color.Gainsboro);
                    foreach (Texture2D r in Stores.RoomDataFiller.barRooms)
                    {
                        sb.Draw(r, new Vector2(xSP, ySP), Color.White);
                        xSP += 150;
                    }
                    break;
            }
            #endregion

            //Draw the layout Preview
            //Set initial positions
            layoutWidth = layoutPreview.StoreLayout.GetLength(1) * GameGlobal.TILESIZE;
            layoutHeight = layoutPreview.StoreLayout.GetLength(0) * GameGlobal.TILESIZE;
            previewCoord.X = (GameGlobal.GameWidth / 2) - (layoutWidth / 2);
            previewCoord.Y = (GameGlobal.GameWidth / 3.5f) - (layoutHeight / 2);
            //loop through layout array
            for (int vert = 0; vert < layoutPreview.StoreLayout.GetLength(0); vert++)
            {
                for (int horz = 0; horz < layoutPreview.StoreLayout.GetLength(1); horz++)
                {
                    //if the space is usable, draw it
                    if (layoutPreview.StoreLayout[vert, horz] == true)
                    {
                        sb.Draw(Graphics.GlobalGfx.gridLargeTest, previewCoord, Color.White);
                    }
                    //update position for next space
                    previewCoord.X += GameGlobal.TILESIZE;
                }
                //update position for next row
                previewCoord.Y += GameGlobal.TILESIZE;
                previewCoord.X = (GameGlobal.GameWidth / 2) - (layoutWidth / 2);
            }

            sb.End();

            //draw buttons
            acceptBtn.drawButton(sb,true);
            leftBtn.drawButton(sb,true);
            backBtn.drawButton(sb, true);
            rightBtn.drawButton(sb,true);
            layoutBtns.drawRadioBtns(sb);
            nameField.drawField(sb, gt);
        }

        public void unloadStoreSelection()
        {
            acceptBtn.unloadBtn();
            acceptBtn = null;
            leftBtn.unloadBtn();
            leftBtn = null;
            rightBtn.unloadBtn();
            rightBtn = null;
            layoutABtn.unloadBtn();
            layoutABtn = null;
            layoutBBtn.unloadBtn();
            layoutBBtn = null;
            layoutCBtn.unloadBtn();
            layoutCBtn = null;
            layoutDBtn.unloadBtn();
            layoutDBtn = null;
            backBtn.unloadBtn();
            backBtn = null;
            nameField.unloadField();
            nameField = null;
            previewCoord = Vector2.Zero;
            layoutPreview = null;
        }

        /// <summary>
        /// Centers the type of store under the name field
        /// </summary>
        /// <returns>The vector2 position for where to start drawing the string.</returns>
        private Vector2 centerName()
        {
            Vector2 pos = new Vector2();
            float length;
            string text = "";
            switch (storeIndex)
            {
                case 0:
                    text = "Equipment Store";
                    break;
                case 1:
                    text = "General Store";
                    break;
                case 2:
                    text = "Alchemy Lab";
                    break;
                case 3:
                    text = "Stables";
                    break;
                case 4:
                    text = "Inn";
                    break;
                case 5:
                    text = "Bar";
                    break;
            }
            length = GameGlobal.gameFont.MeasureString(text).X;
            pos.X = nameField.Bounds.Center.X - (length / 2);
            pos.Y = 35;
            return pos;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Storefront.Input;

namespace Storefront.GameLogic.NewGameStuff
{
    //sub state for the new game stuff
    public enum NewGameState
    {
        Intro, //not needed?
        CharacterIntroEdit,
        CharacterStatsEdit,
        CharacterBackstory,
        StoreSelection,
        StoreEdit
    }
    public class NewGameLogic
    {
        //Content Manager
        private ContentManager cm;
        //state
        private NewGameState ngs = NewGameState.CharacterIntroEdit;
        private bool? stateSwitch; //handles switching states.
        //controls and graphics vars
        private SFTextField nameField;

        //controls for the first part of the character edit
        private SFButton HumanBtn, ElfButton, DwarfBtn, GnomeBtn, UndeadBtn, GoblinBtn, MaleBtn, FemaleBtn;
        private SFRadioButtons rbtns, gbtns;
        private SFButton AdvanceBtn;
        //private Texture2D playerSprite;

        //handles selecting the store for a new game
        private StoreSelection storeSel = new StoreSelection();
        //for initial piece placement
        private InitialPiecePlacement ipp = new InitialPiecePlacement();


        public NewGameLogic(ContentManager cm)
        {
            this.cm = cm;
        }

        //load all textures/sounds/etc here.
        public void loadNGLContent()
        {

            switch (ngs)
            {
                case NewGameState.CharacterIntroEdit:
                    #region Character Intro Edit Load

                        nameField = new Input.SFTextField(new Rectangle(GameGlobal.GameWidth / 3, GameGlobal.GameHeight / 20, 285, 30), Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.fieldCursor, 25);

                        #region Race Radio Button Init
                        //race radio buttons
                        HumanBtn = new SFButton(new Rectangle(GameGlobal.GameWidth / 5, GameGlobal.GameHeight / 8, 100, 30),
                            Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "Human", true, true, true);
                        ElfButton = new SFButton(new Rectangle(GameGlobal.GameWidth / 5 + 100, GameGlobal.GameHeight / 8, 100, 30),
                            Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "Elf", true, true, true);
                        DwarfBtn = new SFButton(new Rectangle(GameGlobal.GameWidth / 5 + 200, GameGlobal.GameHeight / 8, 100, 30),
                            Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "Dwarf", true, true, true);
                        GnomeBtn = new SFButton(new Rectangle(GameGlobal.GameWidth / 5 + 300, GameGlobal.GameHeight / 8, 100, 30),
                            Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "Gnome", true, true, true);
                        UndeadBtn = new SFButton(new Rectangle(GameGlobal.GameWidth / 5 + 400, GameGlobal.GameHeight / 8, 100, 30),
                            Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "Undead", true, true, true);
                        GoblinBtn = new SFButton(new Rectangle(GameGlobal.GameWidth / 5 + 500, GameGlobal.GameHeight / 8, 100, 30),
                            Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "Goblin", true, true, true);

                        //init race radio buttons
                        rbtns = new SFRadioButtons();
                        rbtns.addButton(HumanBtn);
                        rbtns.addButton(ElfButton);
                        rbtns.addButton(DwarfBtn);
                        rbtns.addButton(GnomeBtn);
                        rbtns.addButton(UndeadBtn);
                        rbtns.addButton(GoblinBtn);
                        #endregion

                        #region Gender Radio Buttons Init
                        MaleBtn = new SFButton(new Rectangle(GameGlobal.GameWidth / 5, GameGlobal.GameHeight / 5, 300, 30),
                            Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "Male", true, true, true);

                        FemaleBtn = new SFButton(new Rectangle(GameGlobal.GameWidth / 5 + 300, GameGlobal.GameHeight / 5, 300, 30),
                                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "Female", true, true, true);

                        //init gender radio buttons
                        gbtns = new SFRadioButtons();
                        gbtns.addButton(MaleBtn);
                        gbtns.addButton(FemaleBtn);
                        #endregion

                        //init the button to advance to the next screen
                        AdvanceBtn = new SFButton(new Rectangle(GameGlobal.GameWidth / 2 - 50, GameGlobal.GameHeight / 2 - 50, 100, 100),
                            Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "Continue", false, false, false);

                    #endregion
                    break;
                case NewGameState.StoreSelection:
                    #region Store Selection Load
                    storeSel.initStoreSelection(cm);
                    #endregion
                    break;
                case NewGameState.StoreEdit:
                    #region Initial Piece Placement
                    ipp.initIPP(cm);
                    #endregion
                    break;
            }

        }

        public void unloadIntroEdit()
        {
            nameField.unloadField();
            nameField = null;
            HumanBtn.unloadBtn();
            HumanBtn = null;
            GnomeBtn.unloadBtn();
            GnomeBtn = null;
            ElfButton.unloadBtn();
            ElfButton = null;
            GoblinBtn.unloadBtn();
            GoblinBtn = null;
            UndeadBtn.unloadBtn();
            UndeadBtn = null;
            DwarfBtn.unloadBtn();
            DwarfBtn = null;
            MaleBtn.unloadBtn();
            MaleBtn = null;
            FemaleBtn.unloadBtn();
            FemaleBtn = null;
            AdvanceBtn.unloadBtn();
            AdvanceBtn = null;
            storeSel.unloadStoreSelection();
            storeSel = null;
            ipp.unloadIPP();
            ipp = null;
            GC.Collect();
        }

        public void updateNGL(GameTime gt)
        {
            switch (ngs)
            {
                case NewGameState.CharacterIntroEdit:
                    #region Character Intro Edit Update
                    nameField.updateText();

                    rbtns.updateRadioBtns();
                    gbtns.updateRadioBtns();

                    AdvanceBtn.updateButton();

                    //check to see that data is filled in for character.
                    if (rbtns.CurrentSelectedIndex != -1 && gbtns.CurrentSelectedIndex != -1 && nameField.Text.Length > 0)
                    {
                        AdvanceBtn.IsEnabled = true;
                        AdvanceBtn.IsVisible = true;
                    }
                    else
                    {
                        AdvanceBtn.IsEnabled = false;
                        AdvanceBtn.IsVisible = false;
                    }

                    //if the advanced button is clicked, move to the store selection screen
                    //perhaps add either a back button to return to intro edit screen, or a confirmation.
                    if (AdvanceBtn.isDown())
                    {
                        ngs = NewGameState.StoreSelection;
                        //assign data from the first screen to the player class
                        GameGlobal.player = new Player();
                        
                        GameGlobal.player.Name = nameField.Text;
                        switch (rbtns.CurrentSelectedIndex)
                        {
                            case 0:
                                GameGlobal.player.PlayerRace = PRace.Human;
                                break;
                            case 1:
                                GameGlobal.player.PlayerRace = PRace.Elf;
                                break;
                            case 2:
                                GameGlobal.player.PlayerRace = PRace.Dwarf;
                                break;
                            case 3:
                                GameGlobal.player.PlayerRace = PRace.Gnome;
                                break;
                            case 4:
                                GameGlobal.player.PlayerRace = PRace.Undead;
                                break;
                            case 5:
                                GameGlobal.player.PlayerRace = PRace.Goblin;
                                break;
                        }
                        if (gbtns.CurrentSelectedIndex == 0)
                        {
                            GameGlobal.player.Gender = true;
                        }
                        else
                        {
                            GameGlobal.player.Gender = false;
                        }
                        //init Player Stats
                        GameGlobal.player.initPlayer();
                        //load next scene
                        loadNGLContent();
                    }
                    #endregion
                    break;
                case NewGameState.StoreSelection:
                    //update the store selection screen
                    storeSel.updateStoreSelection(out stateSwitch);
                    if (stateSwitch == true)
                    {
                        ngs = NewGameState.StoreEdit;
                        loadNGLContent();
                    }
                    else if (stateSwitch == null)
                    {
                        ngs = NewGameState.CharacterIntroEdit;
                        loadNGLContent();
                    }
                    break;
                case NewGameState.StoreEdit:
                    ipp.updateIPP(out stateSwitch);
                    if (stateSwitch == true)
                    {
                        //close new game stuff and load gameplay
                        GameGlobal.CurrentGS = GameState.GameStandard;
                        Game1.newState = true;
                    }
                    else if (stateSwitch == null)
                    {
                        ngs = NewGameState.StoreSelection;
                        loadNGLContent();
                    }
                    break;
            }
        }

        public void drawNGL(SpriteBatch sb, GameTime gt)
        {
            switch (ngs)
            {
                case NewGameState.CharacterIntroEdit:
                    #region Character Intro Edit Draw
                    sb.Begin();
                    //Draw the Character Name Box
                    sb.DrawString(GameLogic.GameGlobal.gameFont, "Character Name:",
                        new Vector2(GameGlobal.GameWidth / 10, GameGlobal.GameHeight / 20), Color.Cyan);

                    sb.DrawString(GameLogic.GameGlobal.gameFont, "Race:", new Vector2(GameGlobal.GameWidth / 10, GameGlobal.GameHeight / 8), Color.Cyan);

                    sb.DrawString(GameLogic.GameGlobal.gameFont, "Gender:", new Vector2(GameGlobal.GameWidth / 10, GameGlobal.GameHeight / 5), Color.Cyan);



                    sb.End();
                    //draw controls
                    nameField.drawField(sb, gt);

                    rbtns.drawRadioBtns(sb);
                    gbtns.drawRadioBtns(sb);

                    AdvanceBtn.drawButton(sb,true);
                    #endregion
                    break;
                case NewGameState.StoreSelection:
                    //draw the store selection screen
                    storeSel.drawStoreSelection(sb, gt);
                    break;
                case NewGameState.StoreEdit:
                    ipp.drawIPP(sb, gt);
                    break;
            }
        }
    }
}

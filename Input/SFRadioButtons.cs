using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.Input
{
    public class SFRadioButtons
    {
        private bool enabled;
        private bool vis;
        private bool mustSelect;
        private int defaultBtn;
        private List<SFButton> buttonCollection;
        private int currentActive = -1;

        //constructors
        /// <summary>
        /// SFRadioButtons Constructor
        /// </summary>
        /// <param name="bounds">The rectangle that the buttons should be grouped into.</param>
        /// <param name="orient">True for horizontal, false for vertical.</param>
        public SFRadioButtons()
        {
            buttonCollection = new List<SFButton>();
            mustSelect = false;
        }

        public SFRadioButtons(bool mustSelect, int defaultBtn)
        {
            buttonCollection = new List<SFButton>();
            this.mustSelect = mustSelect;
            this.defaultBtn = defaultBtn;
        }

        #region Attributes
        /// <summary>
        /// Gets or Sets whether the set of buttons is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// Gets or Sets whether the set of buttons is visible.
        /// </summary>
        public bool IsVisible
        {
            get { return vis; }
            set { vis = value; }
        }

        /// <summary>
        /// Gets or sets the Index of the selected button.  Use this to set the default selected button.
        /// </summary>
        public int CurrentSelectedIndex
        {
            get { return currentActive; }
            set { currentActive = value; }
        }
        #endregion

        /// <summary>
        /// Adds a button to the end of the collection of buttons
        /// to be used as a set of radio buttons.
        /// </summary>
        /// <param name="btn">The button to be added to the collection.</param>
        public void addButton(SFButton btn)
        {
            buttonCollection.Add(btn);
        }

        public void updateRadioBtns()
        {
            //for each button in the set of radio buttons.
            foreach (SFButton btn in buttonCollection)
            {
                //if a button must be selected and the active is -1 (none selected)
                //force the default button to be selected.
                if (mustSelect && currentActive == -1)
                {
                    buttonCollection[defaultBtn].ButtonState = SFButtonState.Down;
                }

                //update the buttons.
                btn.updateButton();

                //if there is no button selected and a button is clicked.
                if (currentActive == -1 && btn.isDown())
                {
                    //set the current selected button to the index of clicked button.
                    currentActive = buttonCollection.IndexOf(btn);
                }

                //if a button is selected and not equal to the index of the current selected.
                if (btn.isDown() && buttonCollection.IndexOf(btn) != currentActive)
                {
                    //set the active as the button that was clicked
                    currentActive = buttonCollection.IndexOf(btn);
                    //set that buttons state to down (clicked)
                    buttonCollection[currentActive].ButtonState = SFButtonState.Down;
                    //flip all other buttons to up (deselected)
                    for (int index = 0; index < buttonCollection.Count; index++)
                    {
                        if (index != currentActive)
                        {
                            buttonCollection[index].ButtonState = SFButtonState.Up;
                        }
                    }
                }
            }

            //check to see that at least one button is selected, else set the current active to -1 (none selected)
            for (int index = 0; index < buttonCollection.Count; index++)
            {
                //if one button is found, exit
                if (buttonCollection[index].isDown())
                {
                    break;
                }
                //if all the button are checked and none are selected
                if (index == buttonCollection.Count - 1)
                {
                    //if one must be selected, force the current to stay selected.
                    if (mustSelect)
                    {
                        buttonCollection[currentActive].ButtonState = SFButtonState.Down;
                    }
                    else //otherwise set to -1 for none selected
                    {
                        currentActive = -1;
                    }
                }
            }
        }

        public void drawRadioBtns(SpriteBatch sb)
        {
            foreach (SFButton btn in buttonCollection)
            {
                btn.drawButton(sb, true);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Storefront.Input
{
    public enum MouseBtns
    {
        LeftClick,
        MiddleClick,
        RightClick,
        Btn1,
        Btn2
    }

    public class InputHelper
    {
        #region StateVars
        private GamePadState _lastGamepadState;
        private GamePadState _currentGamepadState;
#if (!XBOX)
        private KeyboardState _lastKeyboardState;
        private KeyboardState _currentKeyboardState;

        private MouseState _lastMouseState;
        private MouseState _currentMouseState;
#endif
        private PlayerIndex _index = PlayerIndex.One;
        private bool refreshData = false;
        #endregion


        public void Update() //Fetches the latest input states.
        {
                if (!refreshData)
                    refreshData = true;
                if (_lastGamepadState == null && _currentGamepadState == null)
                {
                    _lastGamepadState = _currentGamepadState = GamePad.GetState(_index);
                }
                else
                {
                    _lastGamepadState = _currentGamepadState;
                    _currentGamepadState = GamePad.GetState(_index);
                }
#if (!XBOX)
                if (_lastKeyboardState == null && _currentKeyboardState == null)
                {
                    _lastKeyboardState = _currentKeyboardState = Keyboard.GetState();
                }
                else
                {
                    _lastKeyboardState = _currentKeyboardState;
                    _currentKeyboardState = Keyboard.GetState();
                }
                //Mouse input
                if (_lastMouseState == null && _currentMouseState == null)
                {
                    _lastMouseState = _currentMouseState = Mouse.GetState();
                }
                else
                {
                    _lastMouseState = _currentMouseState;
                    _currentMouseState = Mouse.GetState();
                }
#endif
        }

        #region GetStates
        public GamePadState LastGamepadState
        {
            get { return _lastGamepadState; }
        }
        public GamePadState CurrentGamepadState
        {
            get { return _currentGamepadState; }
        }

        //the index that is used to poll the gamepad.
        public PlayerIndex Index
        {
            get { return _index; }
            set
            {
                _index = value;
                if (refreshData)
                {
                    Update();
                    Update();
                }
            }
        }

#if (!XBOX)
        public KeyboardState LastKeyboardState
        {
            get { return _lastKeyboardState; }
        }
        public KeyboardState CurrentKeyboardState
        {
            get { return _currentKeyboardState; }
        }

        //Get Mouse States

        public MouseState LastMouseState
        {
            get { return _lastMouseState; }
        }
        public MouseState CurrentMouseState
        {
            get { return _currentMouseState; }
        }
#endif
        #endregion

        #region AnalogStick
        public Vector2 LeftStickPosition
        {
            get
            {
                return new Vector2(
                    _currentGamepadState.ThumbSticks.Left.X,
                    -CurrentGamepadState.ThumbSticks.Left.Y);
            }
        }

        public Vector2 LeftStickVelocity
        {
            get
            {
                Vector2 temp =
                    _currentGamepadState.ThumbSticks.Left -
                    _lastGamepadState.ThumbSticks.Left;
                return new Vector2(temp.X, -temp.Y);
            }
        }
        #endregion

        #region Triggers
        public float LeftTriggerPosition
        {
            get { return _currentGamepadState.Triggers.Left; }
        }

        public float RightTriggerPosition
        {
            get { return _currentGamepadState.Triggers.Right; }
        }

        public float LeftTriggerVelocity
        {
            get
            {
                return _currentGamepadState.Triggers.Left - _lastGamepadState.Triggers.Left;
            }
        }

        public float RightTriggerVelocity
        {
            get
            {
                return _currentGamepadState.Triggers.Right - _lastGamepadState.Triggers.Right;
            }
        }
        #endregion

        #region New/Current/Old Press Checker
        //selected button is being pressed in the current state AND NOT the last state.
        public bool IsNewPress(Buttons button)
        {
            return (_lastGamepadState.IsButtonUp(button) && _currentGamepadState.IsButtonDown(button));
        }

        //selected button is being pressed in the current state AND in the last state.
        public bool IsCurPress(Buttons button)
        {
            return (_lastGamepadState.IsButtonDown(button) && _currentGamepadState.IsButtonDown(button));
        }

        //selected button is NOT being pressed in the current state AND is being pressed in the last state.
        public bool IsOldPress(Buttons button)
        {
            return (_lastGamepadState.IsButtonDown(button) && _currentGamepadState.IsButtonUp(button));
        }

#if (!XBOX360)
        //selected key is being pressed in the current state AND NOT in the last state.
        public bool IsNewPress(Keys key)
        {
            return (_lastKeyboardState.IsKeyUp(key) && _currentKeyboardState.IsKeyDown(key));
        }

        //selected key is being pressed in the current state AND in the last state.
        public bool IsCurPress(Keys key)
        {
            return (_lastKeyboardState.IsKeyDown(key) && _currentKeyboardState.IsKeyDown(key));
        }
        //selected key is NOT being pressed in the current state AND being pressed in the last state.
        public bool IsOldPress(Keys key)
        {
            return (_lastKeyboardState.IsKeyDown(key) && _currentKeyboardState.IsKeyUp(key));
        }

        //New/Current/Old Mouse Button Presses
        public bool IsNewPress(MouseBtns mBtn)
        {
            bool isNew = false;
            switch (mBtn)
            {
                case MouseBtns.LeftClick:
                    isNew = (_lastMouseState.LeftButton == ButtonState.Released && _currentMouseState.LeftButton == ButtonState.Pressed);
                    break;
                case MouseBtns.MiddleClick:
                    isNew = (_lastMouseState.MiddleButton == ButtonState.Released && _currentMouseState.MiddleButton == ButtonState.Pressed);
                    break;
                case MouseBtns.RightClick:
                    isNew = (_lastMouseState.RightButton == ButtonState.Released && _currentMouseState.RightButton == ButtonState.Pressed);
                    break;
                case MouseBtns.Btn1:
                    isNew = (_lastMouseState.XButton1 == ButtonState.Released && _currentMouseState.XButton1 == ButtonState.Pressed);
                    break;
                case MouseBtns.Btn2:
                    isNew = (_lastMouseState.XButton2 == ButtonState.Released && _currentMouseState.XButton2 == ButtonState.Pressed);
                    break;
                default:
                    return false;

            }
            return isNew;
        }

        //selected key is being pressed in the current state AND in the last state.
        public bool IsCurPress(MouseBtns mBtn)
        {
            bool isNew = false;
            switch (mBtn)
            {
                case MouseBtns.LeftClick:
                    isNew = (_lastMouseState.LeftButton == ButtonState.Pressed && _currentMouseState.LeftButton == ButtonState.Pressed);
                    break;
                case MouseBtns.MiddleClick:
                    isNew = (_lastMouseState.MiddleButton == ButtonState.Pressed && _currentMouseState.MiddleButton == ButtonState.Pressed);
                    break;
                case MouseBtns.RightClick:
                    isNew = (_lastMouseState.RightButton == ButtonState.Pressed && _currentMouseState.RightButton == ButtonState.Pressed);
                    break;
                case MouseBtns.Btn1:
                    isNew = (_lastMouseState.XButton1 == ButtonState.Pressed && _currentMouseState.XButton1 == ButtonState.Pressed);
                    break;
                case MouseBtns.Btn2:
                    isNew = (_lastMouseState.XButton2 == ButtonState.Pressed && _currentMouseState.XButton2 == ButtonState.Pressed);
                    break;
                default:
                    return false;
            }
            return isNew;
        }
        //selected key is NOT being pressed in the current state AND being pressed in the last state.
        public bool IsOldPress(MouseBtns mBtn)
        {
            bool isNew = false;
            switch (mBtn)
            {
                case MouseBtns.LeftClick:
                    isNew = (_lastMouseState.LeftButton == ButtonState.Pressed && _currentMouseState.LeftButton == ButtonState.Released);
                    break;
                case MouseBtns.MiddleClick:
                    isNew = (_lastMouseState.MiddleButton == ButtonState.Pressed && _currentMouseState.MiddleButton == ButtonState.Released);
                    break;
                case MouseBtns.RightClick:
                    isNew = (_lastMouseState.RightButton == ButtonState.Pressed && _currentMouseState.RightButton == ButtonState.Released);
                    break;
                case MouseBtns.Btn1:
                    isNew = (_lastMouseState.XButton1 == ButtonState.Pressed && _currentMouseState.XButton1 == ButtonState.Released);
                    break;
                case MouseBtns.Btn2:
                    isNew = (_lastMouseState.XButton2 == ButtonState.Pressed && _currentMouseState.XButton2 == ButtonState.Released);
                    break;
                default:
                    return false;
            }
            return isNew;
        }
#endif
        #endregion
    }
}

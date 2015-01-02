using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.GameLogic.OfficeMenuClasses
{
    public class FinanceView : SubMenuView
    {
        private Database.FinanceReport week, month, year;
        private Input.SFButton closeBtn;

        public override void InitView(Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            
        }

        public override void LoadView()
        {
            closeBtn = new Input.SFButton(new Rectangle((int)(GameGlobal.GameWidth * .8f),
                            (int)(GameGlobal.GameHeight / 1.0833f),
                            (int)(GameGlobal.GameWidth - (GameGlobal.GameWidth * .8f)),
                            (int)(GameGlobal.GameHeight - (GameGlobal.GameHeight / 1.0833f))),
                    Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive,
                    "Close", false, true, true);

            week = new Database.FinanceReport();
            month = new Database.FinanceReport();
            year = new Database.FinanceReport();
        }

        public override void UpdateView(GameTime gt, out bool? stateSwitch)
        {
            stateSwitch = null;
        }

        public override void DrawView(GameTime gt, SpriteBatch sb)
        {
            
        }

        public override void UnloadView()
        {
            
        }
    }
}

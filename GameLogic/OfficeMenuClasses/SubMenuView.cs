using System;

namespace Storefront.GameLogic.OfficeMenuClasses
{
    public abstract class SubMenuView
    {
        public abstract void InitView(Microsoft.Xna.Framework.Content.ContentManager cm);

        public abstract void LoadView();

        public abstract void UpdateView(Microsoft.Xna.Framework.GameTime gt, out bool? stateSwitch);

        public abstract void DrawView(Microsoft.Xna.Framework.GameTime gt, 
            Microsoft.Xna.Framework.Graphics.SpriteBatch sb);

        public abstract void UnloadView();
    }
}

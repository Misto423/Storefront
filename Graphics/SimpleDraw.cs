using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.Graphics
{
    public static class SimpleDraw
    {
        public static Texture2D emptyTex;

        public static void initGfx(Texture2D t)
        {
            emptyTex = t;
            emptyTex.SetData(new[] { Color.White });
        }

        public static void drawLine(SpriteBatch SB, Vector2 p1, Vector2 p2, float width, Color c)
        {
            float angle = (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
            float length = Vector2.Distance(p1, p2);

            //SB.Draw(emptyTex, p1, null, c, angle, new Vector2(0f, (float)width / 2), new Vector2(length, 1f), SpriteEffects.None, 0f);
            SB.Draw(emptyTex, p1, null, c, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0f);
        }

        public static void fillArea(SpriteBatch SB, Rectangle r, Color c)
        {
            SB.Draw(emptyTex, r, c);
        }
    }
}


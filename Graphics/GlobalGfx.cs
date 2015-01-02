using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.Graphics
{
    public static class GlobalGfx
    {
        //all global graphics vars should be placed here
        public static Texture2D gridLargeTest;
        public static Texture2D btnHover, btnDef, btnActive;
        public static Texture2D fieldCursor;
        public static Texture2D dialogTexture;
        private static Texture2D customerBase;
        private static List<FileInfo> iconFiles = new List<FileInfo>();
        public static List<Texture2D> iconTextures = new List<Texture2D>();
        private static Random r;

        public static readonly int CHARACTER_SIDE_LOLOLOL = 16;

        //shader stuff
        public static Effect colorSwapEffect, e2;
        private static Texture2D palette;

        //load in all global gfx vars
        public static void initGfx(ContentManager cm, GraphicsDevice gd)
        {
            r = new Random();

            btnHover = cm.Load<Texture2D>(@"MiscGfx\GridSpace");
            btnDef = cm.Load<Texture2D>(@"MiscGfx\GridSpacedefaulttest");
            btnActive = cm.Load<Texture2D>(@"MiscGfx\GridSpaceactivetest");
            //console msg
            Program.gameConsole.AddLine("Button Textures Loaded", Microsoft.Xna.Framework.Color.Green);

            fieldCursor = cm.Load<Texture2D>(@"Text\Cursor");
            //console msg
            Program.gameConsole.AddLine("Text Field Textures Loaded", Microsoft.Xna.Framework.Color.Green);

            gridLargeTest = cm.Load<Texture2D>(@"MiscGfx\GridLarge");
            //console msg
            Program.gameConsole.AddLine("Grid Textures Loaded", Microsoft.Xna.Framework.Color.Green);

            dialogTexture = cm.Load<Texture2D>(@"ControlAssets\dialogTest");
            //console msg
            Program.gameConsole.AddLine("Dialog Textures Loaded", Microsoft.Xna.Framework.Color.Green);

            //setup Icon Sprite Sheets
            getIconsInDirectory(cm.RootDirectory);
            Stream str;
            foreach (FileInfo fi in iconFiles)
            {
                str = new FileStream(fi.Directory.ToString() + @"\" + fi.Name, FileMode.Open, FileAccess.Read);
                iconTextures.Add(Texture2D.FromStream(gd, str));

                //console msg
                Program.gameConsole.AddLine(fi.Name + " Icon Sheet Loaded", Microsoft.Xna.Framework.Color.Green);
            }


            //load shaders
            palette = cm.Load<Texture2D>(@"Characters\palette");
            //colorSwapEffect = cm.Load<Effect>(@"Shaders\PaletteCycle");
#if DIRECTX
            BinaryReader Reader = new BinaryReader(File.Open(@"Content\shaders\DXpaletteCycle.mgfx", FileMode.Open));
            colorSwapEffect = new Effect(gd, Reader.ReadBytes((int)Reader.BaseStream.Length));
            Reader.Close();
#else
            BinaryReader Reader = new BinaryReader(File.Open(@"Content\shaders\PaletteCycle2.mgfxo", FileMode.Open));
            colorSwapEffect = new Effect(gd, Reader.ReadBytes((int)Reader.BaseStream.Length));
            Reader.Close();
#endif
            
            //customer base texture
            customerBase = cm.Load<Texture2D>(@"Characters\customerTemp");
        }

        /// <summary>
        /// Reads in the texture files found in the icons folder.
        /// Will allow users to make custom icons.
        /// </summary>
        /// <param name="rootDir">The root directory where content is located.</param>
        public static void getIconsInDirectory(string rootDir)
        {
            //get the directory of map files
            DirectoryInfo iconDir = new DirectoryInfo(rootDir + @"\Icons");
            try
            {
                //search through the directory and add in all .qxm files
                foreach (FileInfo IconFile in iconDir.GetFiles("?.png", SearchOption.TopDirectoryOnly))
                {
                    iconFiles.Add(IconFile);
                }
            }
            catch //return if there is an error finding directory
            {
                //console msg
                Program.gameConsole.AddLine(iconDir.FullName + " could not be accessed!", Microsoft.Xna.Framework.Color.Red);
                return;
            }
        }

        public static Texture2D createCustomerTexture(GraphicsDevice gd, SpriteBatch sb)
        {
            #region Shader Code
            //get a random number to pass into the shader.
            float seed = (float)r.Next(0, 32);
            float seed2 = (float)r.Next(0, 32);
            float seed3 = (float)r.Next(0, 32);
            float seed4 = (float)r.Next(0, 32);
            //create the texture to copy color data into
            Texture2D customerTex = new Texture2D(gd, CHARACTER_SIDE_LOLOLOL, CHARACTER_SIDE_LOLOLOL);
            //create a render target to draw a character to.
            RenderTarget2D rendTarget = new RenderTarget2D(gd, CHARACTER_SIDE_LOLOLOL, CHARACTER_SIDE_LOLOLOL,
                false, gd.PresentationParameters.BackBufferFormat, DepthFormat.None);
            gd.SetRenderTarget(rendTarget);
            //set background of new render target to transparent.
            gd.Clear(Microsoft.Xna.Framework.Color.Black);
            //start drawing to the new render target
            sb.Begin(SpriteSortMode.Immediate, BlendState.Opaque,
                    SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            //set the current technique of the shader
            colorSwapEffect.CurrentTechnique = colorSwapEffect.Techniques["ColorSwap"];
            //send the random value to the shader.
            Graphics.GlobalGfx.colorSwapEffect.Parameters["seed"].SetValue(seed);
            Graphics.GlobalGfx.colorSwapEffect.Parameters["seed2"].SetValue(seed2);
            Graphics.GlobalGfx.colorSwapEffect.Parameters["seed3"].SetValue(seed3);
            Graphics.GlobalGfx.colorSwapEffect.Parameters["seed4"].SetValue(seed4);
            //send the palette texture to the shader.
            Graphics.GlobalGfx.colorSwapEffect.Parameters["colorTable"].SetValue(Graphics.GlobalGfx.palette);
            //apply the effect
            //Graphics.GlobalGfx.colorSwapEffect.CurrentTechnique.Passes[0].Apply();
            //draw the texture (now with color!)
            sb.Draw(customerBase, new Microsoft.Xna.Framework.Vector2(0, 0), Microsoft.Xna.Framework.Color.White);
            //end drawing
            sb.End();
            //reset rendertarget
            gd.SetRenderTarget(null);
            //copy the drawn and colored customer to a non-volitile texture (instead of render target)
            //create the color array the size of the texture.
            Color[] cs = new Color[CHARACTER_SIDE_LOLOLOL * CHARACTER_SIDE_LOLOLOL];
            //get all color data from the render target
            rendTarget.GetData<Color>(cs);
            //move the color data into the texture.
            customerTex.SetData<Color>(cs);
            //return the finished texture.
            return customerTex;
            #endregion

            ////get a random number to pass into the shader.
            //float seed = (float)r.Next(0, 32);
            //float seed2 = (float)r.Next(0, 32);
            //float seed3 = (float)r.Next(0, 32);
            //float seed4 = (float)r.Next(0, 32);
            ////create the texture to copy color data into
            //Texture2D customerTex = new Texture2D(gd, CHARACTER_SIDE_LOLOLOL, CHARACTER_SIDE_LOLOLOL);
            ////create a render target to draw a character to.
            //RenderTarget2D rendTarget = new RenderTarget2D(gd, CHARACTER_SIDE_LOLOLOL, CHARACTER_SIDE_LOLOLOL,
            //    false, gd.PresentationParameters.BackBufferFormat, DepthFormat.None);
            //gd.SetRenderTarget(rendTarget);
            ////set background of new render target to transparent.
            //gd.Clear(Microsoft.Xna.Framework.Color.Black);
            ////create the color array the size of the texture.
            //Color[] cs = new Color[CHARACTER_SIDE_LOLOLOL * CHARACTER_SIDE_LOLOLOL];
            ////get all color data from the render target
            //rendTarget.GetData<Color>(cs);
            ////move the color data into the texture.
            //for (int c = 
            ////start drawing to the new render target
            //sb.Begin(SpriteSortMode.Immediate, BlendState.Opaque,
            //        SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            ////draw the texture (now with color!)
            //sb.Draw(customerBase, new Microsoft.Xna.Framework.Vector2(0, 0), Microsoft.Xna.Framework.Color.White);
            ////end drawing
            //sb.End();
            ////reset rendertarget
            //gd.SetRenderTarget(null);
            ////return the finished texture.
            //return customerTex;
        }
    }
}

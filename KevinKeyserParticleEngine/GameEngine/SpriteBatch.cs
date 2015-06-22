using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameEngine
{
    public enum SpriteEffect
    {
        None,
        FlipHorizontally,
        FlipVertically,
        FlipBoth
    }

    public class SpriteBatch
    {
        PictureBox image;
        Bitmap canvas;
        Graphics graphics;
        public Graphics Graphics
        {
            get
            {
                return graphics;
            }
        }

        bool ended = true;

        public SpriteBatch(Size ClientSize, PictureBox image)
        {
            this.image = image;
            canvas = new Bitmap(ClientSize.Width, ClientSize.Height);
            graphics = Graphics.FromImage(canvas);
        }

        private Bitmap TintBitmap(Bitmap bitmap, Color color)
        {
            Bitmap b2 = new Bitmap(bitmap.Width, bitmap.Height);
            ImageAttributes ia = new ImageAttributes();
            float c = 255 - color.R;
            float m = 255 - color.G;
            float y = 255 - color.B;
            float k = 255 - Math.Min(c, Math.Min(m, y));
            float r = color.R / 255f;
            float g = color.G / 255f;
            float b = color.B / 255f;
            float a = color.A / 255f;
            ColorMatrix matrix = new ColorMatrix(
                new Single[][]
                {
                    new Single[] {r, 0, 0, 0, 0},
                    new Single[] {0, g, 0, 0, 0},
                    new Single[] {0, 0, b, 0, 0},
                    new Single[] {0, 0, 0, a, 0},
                    new Single[] {0, 0, 0, 0, 1}
                }
            );

            //ia.SetOutputChannel(ColorChannelFlag.ColorChannelY, ColorAdjustType.Bitmap);
            ia.SetColorMatrix(matrix);
            Graphics graphics = Graphics.FromImage(b2);
            graphics.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, ia);
            graphics.Dispose();
            ia.Dispose();
            return b2;
            
        }

        public void Update(Size ClientSize)
        {
            canvas = new Bitmap(ClientSize.Width, ClientSize.Height);
            graphics = Graphics.FromImage(canvas);
        }

        public void Begin()
        {
            if(ended == false)
            {
                throw new Exception("Must end SpriteBatch before beginning");
            }
            ended = false;
        }

        public void Clear(Color color)
        {
            if(ended == false)
            {
                throw new Exception("SpriteBatch must be closed before clearing");
            }
            graphics.FillRectangle(new SolidBrush(color), 0, 0, canvas.Width, canvas.Height);
        }

        public void Draw(Bitmap texture, PointF position, Color tint)
        {
            this.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), tint, 0, new PointF(0, 0), new PointF(1, 1), SpriteEffect.None);
        }

        public void Draw(Bitmap texture, RectangleF destinationRectangle, Color tint)
        {
            this.Draw(texture, new PointF(destinationRectangle.X, destinationRectangle.Y), new Rectangle(0, 0, texture.Width, texture.Height), tint, 0, new PointF(0, 0), new PointF(destinationRectangle.Width / texture.Width, destinationRectangle.Height/texture.Height), SpriteEffect.None);
        }

        public void Draw(Bitmap texture, PointF position, Rectangle? sourceRectangle, Color tint, float rotation, PointF origin, float scale, SpriteEffect effect)
        {
            this.Draw(texture, position, sourceRectangle, tint, rotation, origin, new PointF(scale, scale), effect);
        }

        public void Draw(Bitmap texture, PointF position, Rectangle? sourceRectangle, Color tint, float rotation, PointF origin, PointF scale, SpriteEffect effect)
        {
            if (ended == true)
            {
                throw new Exception("SpriteBatch must begin before drawing");
            }
            switch (effect)
            {
                case SpriteEffect.FlipHorizontally:
                    texture.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    break;
                case SpriteEffect.FlipVertically:
                    texture.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    break;
                case SpriteEffect.FlipBoth:
                    texture.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                    break;
            }
            texture = TintBitmap(texture, tint);

            Graphics gfx = Graphics.FromImage(canvas);
            
            //position
            gfx.TranslateTransform(position.X, position.Y);
            //rotate
            gfx.RotateTransform(rotation);
            //scale
            gfx.ScaleTransform(scale.X, scale.Y);
            //origin
            gfx.TranslateTransform(-origin.X, -origin.Y);

            gfx.SmoothingMode = SmoothingMode.HighQuality;
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
            if(sourceRectangle.HasValue == false)
            {
                sourceRectangle = new Rectangle(0, 0, image.Width, image.Height);
            }

            gfx.DrawImage(texture, 0, 0, sourceRectangle.Value, GraphicsUnit.Pixel);

            gfx.Dispose();
        }

        public void DrawString(Font font, string text, PointF position, Color color)
        {
            graphics.DrawString(text, font, new SolidBrush(color), position);
        }

        public void End()
        {
            if(ended == true)
            {
                throw new Exception("Must begin spriteBatch before ending");
            }
            ended = true;
            image.Image = canvas;
        }
    }
}

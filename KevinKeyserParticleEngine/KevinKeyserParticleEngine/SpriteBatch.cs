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

namespace KevinKeyserParticleEngine
{
    public enum SpriteEffect
    {
        None,
        FlipHorizontally,
        FlipVertically
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

        private Bitmap ColorTint(Bitmap sourceBitmap, float redTint, float greenTint, float blueTint, float alphaTint)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                               sourceBitmap.Width, sourceBitmap.Height),
                                               ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);

            float blue = 0;
            float green = 0;
            float red = 0;
            float alpha = 0;

            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
            {
                blue = pixelBuffer[k] + (255 - pixelBuffer[k]) * blueTint;
                green = pixelBuffer[k + 1] + (255 - pixelBuffer[k + 1]) * greenTint;
                red = pixelBuffer[k + 2] + (255 - pixelBuffer[k + 2]) * redTint;
                red = pixelBuffer[k + 3] + (255 - pixelBuffer[k + 3]) * alphaTint;

                if (blue > 255)
                { blue = 255; }

                if (green > 255)
                { green = 255; }

                if (red > 255)
                { red = 255; }

                if (alpha > 255)
                { alpha = 255; }

                pixelBuffer[k] = (byte)blue;
                pixelBuffer[k + 1] = (byte)green;
                pixelBuffer[k + 2] = (byte)red;
                //pixelBuffer[k + 3] = (byte)alpha;
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }

        private Bitmap TintBitmap(Bitmap bitmap, Color color, Single intensity)
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
                    new Single[] {-1, 0, 0, 0, 0},
                    new Single[] {0, -1, 0, 0, 0},
                    new Single[] {0, 0, -1, 0, 0},
                    new Single[] {0, 0, 0, a, 0},
                    new Single[] {r, g, b, 1, 1}
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

        public void Draw(Bitmap texture, PointF position, Rectangle sourceRectangle, Color tint, float rotation, PointF origin, PointF scale, SpriteEffect effect)
        {
            if (ended == true)
            {
                throw new Exception("SpriteBatch must begin before drawing");
            }
            texture = TintBitmap(texture, tint, 1f);
            //texture = ColorTint(texture, tint.R, tint.G, tint.B, 255);

            Graphics gfx = Graphics.FromImage(canvas);
            
            //position
            gfx.TranslateTransform(position.X, position.Y);
            //rotate
            gfx.RotateTransform(rotation);
            //origin
            gfx.TranslateTransform(-origin.X, -origin.Y);
            //scale
            gfx.ScaleTransform(scale.X, scale.Y);

            gfx.SmoothingMode = SmoothingMode.HighQuality;
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            gfx.DrawImage(texture, 0, 0, sourceRectangle, GraphicsUnit.Pixel);

            gfx.Dispose();
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

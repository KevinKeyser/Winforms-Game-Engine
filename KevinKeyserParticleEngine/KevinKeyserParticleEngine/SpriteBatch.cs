﻿using System;
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
                alpha = pixelBuffer[k + 3] + (255 - pixelBuffer[k + 3]) * alphaTint;

                blue.Clamp(0, 255);
                red.Clamp(0, 255);
                green.Clamp(0, 255);
                alpha.Clamp(0, 255);

                pixelBuffer[k] = (byte)blue;
                pixelBuffer[k + 1] = (byte)green;
                pixelBuffer[k + 2] = (byte)red;
                pixelBuffer[k + 3] = (byte)alpha;

            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
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

            texture = ColorTint(texture, tint.R, tint.G, tint.B, tint.A);

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
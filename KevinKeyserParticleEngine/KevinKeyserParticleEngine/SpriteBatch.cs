using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KevinKeyserParticleEngine
{
    public class SpriteBatch
    {
        PictureBox image;
        Bitmap canvas;
        Graphics graphics;
        bool ended = true;

        public SpriteBatch(Size ClientSize, PictureBox image)
        {
            this.image = image;
            canvas = new Bitmap(ClientSize.Width, ClientSize.Height);
            graphics = Graphics.FromImage(canvas);
        }

        private Bitmap ColorTint(Bitmap sourceBitmap, float redTint, float greenTint, float blueTint)
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

            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
            {
                blue = pixelBuffer[k] + (255 - pixelBuffer[k]) * blueTint;
                green = pixelBuffer[k + 1] + (255 - pixelBuffer[k + 1]) * greenTint;
                red = pixelBuffer[k + 2] + (255 - pixelBuffer[k + 2]) * redTint;

                if (blue > 255)
                { blue = 255; }

                if (green > 255)
                { green = 255; }

                if (red > 255)
                { red = 255; }

                pixelBuffer[k] = (byte)blue;
                pixelBuffer[k + 1] = (byte)green;
                pixelBuffer[k + 2] = (byte)red;

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
                throw new Exception("Must End SpriteBatch Before Beginning");
            }
            ended = false;
        }

        public void Clear(Color color)
        {
            graphics.FillRectangle(new SolidBrush(color), 0, 0, canvas.Width, canvas.Height);
        }

        public void Draw(Bitmap image, PointF position, Color tint)
        {
            graphics.DrawImage(ColorTint(image, tint.R, tint.G, tint.B), position.X, position.Y);
        }

        public void End()
        {
            if(ended == true)
            {
                throw new Exception("Must Begin SpriteBatch Before Ending");
            }
            ended = true;
            image.Image = canvas;
        }
    }
}

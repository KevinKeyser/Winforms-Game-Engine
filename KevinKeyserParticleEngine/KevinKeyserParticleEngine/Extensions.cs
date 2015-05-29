using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KevinKeyserParticleEngine
{
    public static class Extensions
    {
        public static Color Lerp(Color color1, Color color2, float amount)
        {
            amount.Clamp(0, 1);
            int a = color1.A - color2.A;
            int r = color1.R - color2.R;
            int g = color1.G - color2.G;
            int b = color1.B - color2.B;
            return Color.FromArgb((int)(color1.A - a * amount), (int)(color1.R - r * amount), (int)(color1.G - g * amount), (int)(color1.B - b * amount));
        }

        public static Color nextColor(this Random randomGenerator)
        {
            return nextColor(randomGenerator, true);
        }

        public static Color nextColor(this Random randomGenerator, bool alpha)
        {
            return Color.FromArgb( alpha ? randomGenerator.Next(256) : 255, randomGenerator.Next(256), randomGenerator.Next(256), randomGenerator.Next(256));
        }

        public static void Draw(this Graphics graphics, Image image, PointF position)
        {
            graphics.DrawImage(image, position.X, position.Y);
        }

        public static float Clamp(this float amount, float min, float max)
        {
            if (amount > max)
                amount = max;
            else if (amount < min)
                amount = min;
            return amount;
        }

        public static int Clamp(this int amount, int min, int max)
        {
            if (amount > max)
                amount = max;
            else if (amount < min)
                amount = min;
            return amount;
        }
    }
}

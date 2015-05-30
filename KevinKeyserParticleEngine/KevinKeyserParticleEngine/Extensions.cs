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
            amount.Clamp(0f, 1f);
            int a = color1.A - color2.A;
            int r = color1.R - color2.R;
            int g = color1.G - color2.G;
            int b = color1.B - color2.B;
            return Color.FromArgb((int)(color1.A - a * amount), (int)(color1.R - r * amount), (int)(color1.G - g * amount), (int)(color1.B - b * amount));
        }

        public static float Lerp(float number1, float number2, float amount)
        {
            amount.Clamp(0f, 1f);
            float difference = number1 - number2;
            return number1 - difference * amount;
        }

        public static Color NextColor(this Random randomGenerator)
        {
            return NextColor(randomGenerator, true);
        }

        public static Color NextColor(this Random randomGenerator, bool alpha)
        {
            return Color.FromArgb( alpha ? randomGenerator.Next(256) : 255, randomGenerator.Next(256), randomGenerator.Next(256), randomGenerator.Next(256));
        }

        public static IComparable Clamp(this IComparable amount, IComparable min, IComparable max)
        {
            if (amount.CompareTo(max) > 0)
            {
                amount = max;
            }
            else if (amount.CompareTo(min) < 0)
            {
                amount = min;
            }
            return amount;
        }

        public static float Clamp(this float amount, float min, float max)
        {
            if (amount > max)     
            {
                amount = max;
            }
            else if (amount < min)  
            {
                amount = min;
            }
            return amount;
        }

        public static int Clamp(this int amount, int min, int max)
        {
            if (amount > max)     
            {
                amount = max;
            }
            else if (amount < min)  
            {
                amount = min;
            }
            return amount;
        }
    }
}

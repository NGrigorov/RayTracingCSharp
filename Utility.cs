using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template
{
    public static class Utility
    {
        //Standart clamp function
        public static float Clamp(float min, float max, float value)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        //Transforming floats RGB to Int32
        public static int RGBtoHex(float r, float g, float b)
        {
            string hex = ((int)Clamp(0, 255, r * 255)).ToString("X2") + ((int)Clamp(0, 255, g * 255)).ToString("X2") + ((int)Clamp(0, 255, b * 255)).ToString("X2");
            int col = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return col;
        }

        public static float[] HextoRGB(int hex)
        {
            Color c = Color.FromArgb(hex);

            float[] temp = new float[3];
            temp[0] = c.R / 255;
            temp[1] = c.G / 255;
            temp[2] = c.B / 255;
            return temp;
        }

        //Since I will be using this for colors only I can get away with this method.
        public static float[] MulColor(float[] a, float[] b)
        {
            a[0] *= b[0];
            a[1] *= b[1];
            a[2] *= b[2];
            return a;
        }

        public static float[] MulColor(float[] a, float b)
        {
            a[0] *= b;
            a[1] *= b;
            a[2] *= b;
            return a;
        }

        public static float Vec3Distance(Vector3 a, Vector3 b)
        {
            return (float)Math.Sqrt(((a.X-b.X)* (a.X - b.X)) + ((a.Y-b.Y)* (a.Y - b.Y)) + ((a.Z - b.Z)* (a.Z - b.Z)));
        }
    }
}

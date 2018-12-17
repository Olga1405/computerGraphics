using System;
using System.Drawing;


namespace Converter
{
    public class YCbCr
    {
        double Y;
        double Cb;
        double Cr;

        public YCbCr(double Y, double Cb, double Cr)
        {
            this.Y = Y;
            this.Cb = Cb;
            this.Cr = Cr;
        }
    }

    public static class YCbCrConverter
    {
        private static double[,] directMatrix = new double[3, 3]{
           {0.299, 0.587, 0.114},
           {0.5, -0.4187, -0.0813},
           {0.1687, -0.3313, 0.5}
        };

        private static double[,] invertMatrix = new double[3, 3]{
           {1, 0, 1.402},
           {1, -0.34414, -0.71414},
           {1, 1.772, 0}
        };

        public static YCbCr fromRGB(Color color)
        {
            return fromRGB(color.R, color.G, color.B);
        }

        public static YCbCr fromRGB(int red, int green, int blue)
        {
            double Y = directMatrix[0, 0] * red + directMatrix[0, 1] * green + directMatrix[0, 2] * blue;
            double Cr = 128 + directMatrix[1, 0] * red + directMatrix[1, 1] * green + directMatrix[1, 2] * blue;
            double Cb = 128 + directMatrix[2, 0] * red + directMatrix[2, 1] * green + directMatrix[2, 2] * blue;

            return new YCbCr(Y, Cb, Cr);
        }

        public static Color toRGB(int Y, int Cb, int Cr)
        {
            int R = (int)(invertMatrix[0, 0] * Y + invertMatrix[0, 1] * Cb + invertMatrix[0, 2] * Cr);
            int G = (int)(-128 + invertMatrix[1, 0] * Y + invertMatrix[1, 1] * Cb + invertMatrix[1, 2] * Cr);
            int B = (int)(-128 + invertMatrix[2, 0] * Y + invertMatrix[2, 1] * Cb + invertMatrix[2, 2] * Cr);

            return Color.FromArgb(R, G, B);
        }
    }
}
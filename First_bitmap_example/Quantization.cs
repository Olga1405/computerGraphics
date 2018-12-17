using System;

namespace BasicQuantizer
{
    public static class BasicQuantizerMatrix
    {
        public static int DIMENSION = 8;
        public static int[,] DefaultMatix = new int[8,8] {
            {16, 11, 10, 16, 24, 40, 51, 61 },
            {12, 12, 14, 19, 26, 58, 60, 55 },
            {14, 13, 16, 24, 40, 57, 69, 56},
            {14, 17, 22, 29, 51, 87, 80, 62},
            {18, 22, 37, 29, 51, 87, 80, 62},
            {24, 35, 55, 64, 81, 109, 103, 77},
            {49, 64, 78, 87, 103, 121, 120, 101},
            {72, 92, 95, 98, 112, 100, 103, 99}
        };

        public static int[,] GetMatrix(double quality)
        {
            int[,] quantizeMatrix = BasicQuantizerMatrix.DefaultMatix;
            double scalingFactor = 0;

            if (quality < 50)
            {
                scalingFactor = 5000 / quality;
            }
            else
            {
                scalingFactor = 200 - 2 * quality;
            }

            for (int i = 0; i < BasicQuantizerMatrix.DIMENSION; i++) {
                for (int j = 0; j < BasicQuantizerMatrix.DIMENSION; j++)
                {
                    quantizeMatrix[i, j] = (int) Math.Floor((scalingFactor * quantizeMatrix[i, j] + 50) / 100);

                    if (quantizeMatrix[i, j] == 0)
                    {
                        quantizeMatrix[i, j] = 1;
                    }
                }
            }

            return quantizeMatrix;
        }

        public static double[,] DivideDCT(double[,] DCTCoeffInput, double quality)
        {
            if (DCTCoeffInput.GetLength(0) != BasicQuantizerMatrix.DIMENSION)
            {
                throw new Exception();
            }

            int[,] quantizeMatrix = BasicQuantizerMatrix.GetMatrix(quality);

            for (int i = 0; i < BasicQuantizerMatrix.DIMENSION; i++)
            {
                for (int j = 0; j < BasicQuantizerMatrix.DIMENSION; j++)
                {
                    DCTCoeffInput[i, j] = Math.Round(DCTCoeffInput[i, j] / quantizeMatrix[i, j]);
                }
            }

            return DCTCoeffInput;
        }

    }
}
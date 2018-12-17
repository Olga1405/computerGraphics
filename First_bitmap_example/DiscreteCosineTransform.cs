using System;
using System.Collections.Generic;
using System.Drawing;
using Converter;
using BasicQuantizer;


namespace DiscreteCosineTransform
{
    public static class GlobalVar
    {
        public const int SIZE = 8;
        public const int QUALITY = 80;
    }

    class QuotedBlock
    {
        public Block block;
        public double[,] RQuotedDCTarray;
        public double[,] GQuotedDCTarray;
        public double[,] BQuotedDCTarray;

        public double[,] RQuotedIDCTarray;
        public double[,] GQuotedIDCTarray;
        public double[,] BQuotedIDCTarray;

        public QuotedBlock(Block block)
        {
            this.block = block;
            RQuotedDCTarray = BasicQuantizerMatrix.DivideDCT(block.RDCTarray, GlobalVar.QUALITY);
            GQuotedDCTarray = BasicQuantizerMatrix.DivideDCT(block.GDCTarray, GlobalVar.QUALITY);
            BQuotedDCTarray = BasicQuantizerMatrix.DivideDCT(block.BDCTarray, GlobalVar.QUALITY);
        }

        public void convertToRGB()
        {
            this.RQuotedIDCTarray = block.idct(RQuotedDCTarray, RQuotedDCTarray.GetLength(0), RQuotedDCTarray.GetLength(0));
            this.GQuotedIDCTarray = block.idct(GQuotedDCTarray, GQuotedDCTarray.GetLength(0), GQuotedDCTarray.GetLength(0));
            this.BQuotedIDCTarray = block.idct(BQuotedDCTarray, BQuotedDCTarray.GetLength(0), BQuotedDCTarray.GetLength(0));
        }
    }

    class Block
    {
        public int start;
        public int end;
        public Color[,] colors;
        public YCbCr[,] yCbCrs;

        public double[,] RDCTarray;
        public double[,] GDCTarray;
        public double[,] BDCTarray;

        public double[,] RIDCTarray;
        public double[,] GIDCTarray;
        public double[,] BIDCTarray;

        public Block(Bitmap image, int startI, int startJ)
        {
            start = startI;
            end = startJ;
            colors = new Color[GlobalVar.SIZE, GlobalVar.SIZE];
            yCbCrs = new YCbCr[GlobalVar.SIZE, GlobalVar.SIZE];
            RDCTarray = new double[GlobalVar.SIZE, GlobalVar.SIZE];
            GDCTarray = new double[GlobalVar.SIZE, GlobalVar.SIZE];
            BDCTarray = new double[GlobalVar.SIZE, GlobalVar.SIZE];



            for (int i = 0; i < GlobalVar.SIZE; i++)
                for (int j = 0; j < GlobalVar.SIZE; j++)
                {
                    colors[i, j] = image.GetPixel(startI + i, startJ + j);
                    yCbCrs[i, j] = YCbCrConverter.fromRGB(colors[i, j]);
                }

        }

        public void convertDct()
        {
            int i, j, u, v;
            for (u = 0; u < GlobalVar.SIZE; ++u)
            {
                for (v = 0; v < GlobalVar.SIZE; ++v)
                {
                    RDCTarray[u, v] = 0;
                    GDCTarray[u, v] = 0;
                    BDCTarray[u, v] = 0;
                    for (i = 0; i < GlobalVar.SIZE; i++)
                    {
                        for (j = 0; j < GlobalVar.SIZE; j++)
                        {
                            RDCTarray[u, v] += colors[i, j].R * Math.Cos(Math.PI / ((float)GlobalVar.SIZE) * (i + 1.0 / 2.0) * u) * Math.Cos(Math.PI / ((float)GlobalVar.SIZE) * (j + 1.0 / 2.0) * v);
                            GDCTarray[u, v] += colors[i, j].G * Math.Cos(Math.PI / ((float)GlobalVar.SIZE) * (i + 1.0 / 2.0) * u) * Math.Cos(Math.PI / ((float)GlobalVar.SIZE) * (j + 1.0 / 2.0) * v);
                            BDCTarray[u, v] += colors[i, j].B * Math.Cos(Math.PI / ((float)GlobalVar.SIZE) * (i + 1.0 / 2.0) * u) * Math.Cos(Math.PI / ((float)GlobalVar.SIZE) * (j + 1.0 / 2.0) * v);
                        }
                    }
                }
            }
        }

        public void blockIdct()
        {
            this.RIDCTarray = idct(RDCTarray, RDCTarray.GetLength(0), RDCTarray.GetLength(0));
            this.GIDCTarray = idct(GDCTarray, GDCTarray.GetLength(0), GDCTarray.GetLength(0));
            this.BIDCTarray = idct(BDCTarray, BDCTarray.GetLength(0), BDCTarray.GetLength(0));
        }

        public double[,] idct(double[,] DCTMatrix, int N, int M)
        {

            double[,] result = new double[N, M];
            int i, j, u, v;

            for (u = 0; u < N; ++u)
            {
                for (v = 0; v < M; ++v)
                {
                    result[u, v] = 1 / 4.0 * DCTMatrix[0, 0];
                    for (i = 1; i < N; i++)
                    {
                        result[u, v] += 1 / 2.0 * DCTMatrix[i, 0];
                    }
                    for (j = 1; j < M; j++)
                    {
                        result[u, v] += 1 / 2.0 * DCTMatrix[0, j];
                    }

                    for (i = 1; i < N; i++)
                    {
                        for (j = 1; j < M; j++)
                        {
                            result[u, v] += DCTMatrix[i, j] * Math.Cos(Math.PI / ((float)N) * (u + 1.0 / 2.0) * i) * Math.Cos(Math.PI / ((float)M) * (v + 1.0 / 2.0) * j);
                        }
                    }
                    result[u, v] *= 2.0 / ((float)N) * 2.0 / ((float)M);
                }
            }
            return result;
        }
    }

    class HelperMatricDevider
    {
        public Bitmap rawImage;
        public List<QuotedBlock> blocks;

        public HelperMatricDevider(Bitmap image)
        {
            rawImage = image;
            blocks = convert(image);
        }

        private static List<QuotedBlock> convert(Bitmap image)
        {
            if (image.Width % 8 != 0)
                throw new Exception();

            if (image.Height % 8 != 0)
                throw new Exception();

            List<QuotedBlock> result = new List<QuotedBlock>();

            for (int i = 0; i < image.Width; i += 8)
            {
                for (int j = 0; j < image.Width; j += 8)
                {
                    Block block = new Block(image, i, j);
                    block.convertDct();
                    result.Add(new QuotedBlock(block));
                }
            }

            return result;
        }


        public Bitmap iconvert(List<QuotedBlock> image)
        {
            int size = (int)Math.Sqrt(image.Count) * 8;
            Bitmap result = new Bitmap(size, size);

            foreach (QuotedBlock block in image)
            {
                block.block.blockIdct();
                for (int i = 0; i < GlobalVar.SIZE; i++)
                    for (int j = 0; j < GlobalVar.SIZE; j++) { 
                        int r = DoubleToInt(block.block.RIDCTarray[i, j]);
                        int g = DoubleToInt(block.block.GIDCTarray[i, j]);
                        int b = DoubleToInt(block.block.BIDCTarray[i, j]);

                        result.SetPixel(block.block.start + i, block.block.end + j, Color.FromArgb(r, g, b));
                }
            }
            return result;
        }

    public Bitmap iconvertQuoted(List<QuotedBlock> image)
    {
        int size = (int)Math.Sqrt(image.Count) * 8;
        Bitmap result = new Bitmap(size, size);

        foreach (QuotedBlock block in image)
        {
            block.convertToRGB();
            for (int i = 0; i < GlobalVar.SIZE; i++)
            {
                for (int j = 0; j < GlobalVar.SIZE; j++)
                {
                    int r = DoubleToInt(block.RQuotedIDCTarray[i, j]);
                    int g = DoubleToInt(block.GQuotedIDCTarray[i, j]);
                    int b = DoubleToInt(block.BQuotedIDCTarray[i, j]);

                    result.SetPixel(block.block.start + i, block.block.end + j, Color.FromArgb(r, g, b));
                }
            }
        }

        return result;
    }

    private int DoubleToInt(double d)
        {
            if (d < 0)
                return 0;
            if (d > 255)
                return 255;
            return (int) d;
        }
    }
}

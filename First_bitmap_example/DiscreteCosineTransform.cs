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
        public const int QUALITY = 100;
    }

    class QuotedBlock
    {
        public Block block;
        public double[,] RQuotedDCTarray;
        public double[,] GQuotedDCTarray;
        public double[,] BQuotedDCTarray;

        public QuotedBlock(Block block)
        {
            this.block = block;
            RQuotedDCTarray = BasicQuantizerMatrix.DivideDCT(block.RDCTarray, GlobalVar.QUALITY);
            GQuotedDCTarray = BasicQuantizerMatrix.DivideDCT(block.GDCTarray, GlobalVar.QUALITY);
            BQuotedDCTarray = BasicQuantizerMatrix.DivideDCT(block.BDCTarray, GlobalVar.QUALITY);
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
                            RDCTarray[u, v] += colors[i, j].R * Math.Cos(Math.PI / ((float)GlobalVar.SIZE) * (i + 1.0 / 2.0) * u) * Math.Cos(Math.PI / ((float)GlobalVar.SIZE) * (j + 1.0/2.0) * v);
                            GDCTarray[u, v] += colors[i, j].G * Math.Cos(Math.PI / ((float)GlobalVar.SIZE) * (i + 1.0 / 2.0) * u) * Math.Cos(Math.PI / ((float)GlobalVar.SIZE) * (j + 1.0 / 2.0) * v);
                            BDCTarray[u, v] += colors[i, j].B * Math.Cos(Math.PI / ((float)GlobalVar.SIZE) * (i + 1.0 / 2.0) * u) * Math.Cos(Math.PI / ((float)GlobalVar.SIZE) * (j + 1.0 / 2.0) * v);
                        }
                    }
                }
            }
        }
    } 
    

    class HelperMatricDevider
    {
        Bitmap rawImage;
        List<QuotedBlock> blocks;

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
                    Block block = new Block(image, i,j);
                    block.convertDct();
                    result.Add(new QuotedBlock(block));
                }
            }

            return result;
        }
    }

}

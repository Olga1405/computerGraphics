using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace First_bitmap_example
{
    public partial class Form1 : Form
    {
        #region fields
        Bitmap img;
        int P,x1,x2,x3,type=0;
        bool b = false;
        #endregion

        public Form1()
        {
            InitializeComponent();
        }
        #region buttons
    
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();           
            if (of.ShowDialog() == DialogResult.OK)
            {
                img = new Bitmap(of.FileName);              
                pictureBox1.Image = img;
                pictureBox1.Refresh();
            }
            if (pictureBox1.Image.Width > pictureBox1.Width && pictureBox1.Image.Height > pictureBox1.Height)
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;            
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog of = new SaveFileDialog();
            of.Filter = "Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif |JPEG Image (.jpeg)|*.jpeg |Png Image (.png)|*.png |Tiff Image (.tiff)|*.tiff |Wmf Image (.wmf)|*.wmf|All Files (*.*)|*.*|JPG Image (.jpg)|*.jpg";
            of.FileName = "*";
            of.DefaultExt = "bmp";
            of.ValidateNames = true;
            if (of.ShowDialog() == DialogResult.OK)
            {               
                pictureBox1.Image.Save(of.FileName);
            }
        }      

        private void levelToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }       
        
        private void yCbCrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //by = 0.0722; ry = 0.2126

            double c1, c2, c3;
            Bitmap res2 = new Bitmap(img), res3 = new Bitmap(img), res4 = new Bitmap(img);
            Color c;
            b = true;
            type = 2;

            for (int i = 0; i < img.Width; ++i)
                for (int j = 0; j < img.Height; ++j)
                {
                    c = img.GetPixel(i, j);
                    c1 = Convert.ToDouble(c.R);
                    c2 = Convert.ToDouble(c.G);
                    c3 = Convert.ToDouble(c.B);

                    RGBtoYCbCr(c, out c1, out c2, out c3);
                    YCbCrtoRGB(c1, 0, 0);
                    res2.SetPixel(i, j, Color.FromArgb(x1, x2, x3));                   
                    YCbCrtoRGB(0.5, c2, 0);
                    res3.SetPixel(i, j, Color.FromArgb(x1, x2, x3));                    
                    YCbCrtoRGB(0.5, 0, c3);
                    res4.SetPixel(i, j, Color.FromArgb(x1, x2, x3));

                }
            pictureBox2.Image = res2; pictureBox3.Image = res3; pictureBox4.Image = res4;
            pictureBox2.Refresh(); pictureBox3.Refresh(); pictureBox4.Refresh();
        }

        #endregion

        #region convert func
        //YCbCr
        public void RGBtoYCbCr(Color color, out double Y, out double Cb, out double Cr)
        {
            double Kry = 0.2126, Kby = 0.0722, Kgy = 1 - Kry - Kby;

            double r = (double)color.R / 255.0;
            double g = (double)color.G / 255.0;
            double b = (double)color.B / 255.0;

            Y = Kry * r + Kgy * g + Kby * b;
            Cb = b - Y;
            Cr = r - Y;
        }

        public Color YCbCrtoRGB(double Y, double Cb, double Cr)
        {
            double Kry = 0.2126, Kby = 0.0722, Kgy = 1 - Kry - Kby;

            double r = 0;
            double g = 0;
            double b = 0; 

            r = Y + Cr;           
            g = Y - (Kby / Kgy) * Cb - (Kry / Kgy) * Cr;          
            b = Y + Cb;

            x1 = Convert.ToInt32(r * 255.0);
            x1 = (0 > x1) ? 0 : ((255 < x1) ? 255 : x1); 
            x2 = Convert.ToInt32(g * 255.0);
            x2 = (0 > x2) ? 0 : ((255 < x2) ? 255 : x2); 
            x3 = Convert.ToInt32(b * 255.0);
            x3 = (0 > x3) ? 0 : ((255 < x3) ? 255 : x3); 
            return Color.FromArgb(x1, x2, x3);
        }

        #endregion

        #region mouse click
        
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (b)
            {
                Color color;
                int x = pictureBox1.Width / 2 - pictureBox1.Image.Width / 2, y = pictureBox1.Height / 2 - pictureBox1.Image.Height / 2;
                color = img.GetPixel(e.X-x, e.Y-y);

                switch(type)
                {
                    case 1:
                        {
                            double c1 = Convert.ToDouble(color.R);
                            double c2 = Convert.ToDouble(color.G);
                            double c3 = Convert.ToDouble(color.B);
                            RGBtoYCbCr(color, out c1, out c2, out c3);
                            break;
                        }
                }

                b = false;            
            }
        }
        #endregion  

       

       

        
       

        






       
    }
}

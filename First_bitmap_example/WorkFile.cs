private void filterRobertsToolStripMenuItem_Click(object sender, EventArgs e)
{
if (pictureBox1.Image != null)
{
double tmp, tmp1, tmp2;
Bitmap res = new Bitmap(img);
for (int i = 0; i < res.Width-1; ++i)
for (int j = 0; j < res.Height-1; ++j)
{
Color c1 = res.GetPixel(i,j), 
      c2 = res.GetPixel(i+1,j+1),
      c3 = res.GetPixel(i+1,j),
      c4 = res.GetPixel(i,j+1);
tmp1 = Math.Abs(c2.R + c2.G + c2.B - c1.R - c2.G - c2.B);
tmp2 = Math.Abs(c4.R + c4.G + c4.B - c3.R - c3.G - c3.B);
tmp = Math.Sqrt(Math.Pow((tmp1 / 3.0), 2.0) + Math.Pow((tmp2 / 3.0), 2.0));
// черные границы на белои фоне
tmp = 255 - tmp;
if (tmp > 255) tmp = 255;
res.SetPixel(i, j, Color.FromArgb((int)tmp, (int)tmp, (int)tmp));
}
pictureBox5.Image = res;
pictureBox5.Refresh();
}
else return;
}

// Морфологическое расширение
private void Dilation(int[,] srcR, int[,] srcG, int[,] srcB, bool[,] mask, int[,] ResR, int[,] ResG, int[,] ResB, int width, int height)
{
int MW = 4, MH = 4;// Размеры инструмента
for (int y = MH / 2; y < height - MH / 2; y++)// От радиуса инструмента
for (int x = MW / 2; x < width - MW / 2; x++)
{
int maxR = 0;
int maxG = 0;
int maxB = 0;
// Поиск максимума в окрестности текущего пиксела и заполнение матриц ResR, ResG, ResB
for (int j = -MH / 2; j <= MH / 2; j++)// Рассмотрение окр-ти текущего пиксела
for (int i = -MW / 2; i <= MW / 2; i++)
{
if ((mask[i+2, j+2]) && (srcR[x + i, y + j] > maxR))
maxR = srcR[x + i, y + j];
if ((mask[i+2, j+2]) && (srcG[x + i, y + j] > maxG))
maxG = srcG[x + i, y + j];
if ((mask[i+2, j+2]) && (srcB[x + i, y + j] > maxB))
maxB = srcB[x + i, y + j];

}
ResR[x, y] = maxR;
ResG[x, y] = maxG;
ResB[x, y] = maxB;
}
}
// Морфологическое сужение
void Erosion(int[,] srcR, int[,] srcG, int[,] srcB, bool[,] mask, int[,] ResR, int[,] ResG, int[,] ResB, int width, int height)
{
int MW = 4, MH = 4;
for (int y = MH / 2; y < height - MH / 2; y++)
for (int x = MW / 2; x < width - MW / 2; x++)
{
int minR = 255;
int minG = 255;
int minB = 255;
for (int j = -MH / 2; j <= MH / 2; j++)
for (int i = -MW / 2; i <= MW / 2; i++)
{
if ((mask[i+2, j+2]) && (srcR[x + i, y + j] < minR))
minR = srcR[x + i, y + j];
if ((mask[i+2, j+2]) && (srcG[x + i, y + j] < minG))
minG = srcG[x + i, y + j];
if ((mask[i+2, j+2]) && (srcB[x + i, y + j] < minB))
minB = srcB[x + i, y + j];

}
ResR[x, y] = minR;
ResG[x, y] = minG;
ResB[x, y] = minB;
}
}

//Это херачишь в Form1.Designer.c после всяких private

//А сам фильтр:

private void morphologicalGradientToolStripMenuItem_Click(object sender, EventArgs e)
{
if (pictureBox1.Image != null)
{
Bitmap res = new Bitmap(img);
int width = img.Width;
int height = img.Height;
bool[,] Mask = new bool[,] { { false, true, true, true, false },
{ true, true, true, true, true },
{ true, true, true, true, true },
{ true, true, true, true, true },
{ false, true, true, true, false } };
int[,] allPixR = new int[width, height];// Матрица со всеми значениями R, которые есть на изображении
int[,] allPixG = new int[width, height];// -//-
int[,] allPixB = new int[width, height];// -//-
int[,] resPixR1 = new int[width, height];
int[,] resPixG1 = new int[width, height];
int[,] resPixB1 = new int[width, height];
// Заполнение матриц
for (int i = 0; i < width; i++)
for (int j = 0; j < height; j++)
{
allPixR[i, j] = img.GetPixel(i, j).R;
allPixG[i, j] = img.GetPixel(i, j).G;
allPixB[i, j] = img.GetPixel(i, j).B;
}
// Расширение
Dilation(allPixR, allPixG, allPixB, Mask, resPixR, resPixG, resPixB, img.Width, img.Height);
// Сужение
Erosion(resPixR, resPixG, resPixB, Mask, allPixR, allPixG, allPixB, img.Width, img.Height);
for (int i = 0; i < width; i++)
for (int j = 0; j < height; j++)
res.SetPixel(i, j, Color.FromArgb(allPixR[i,j], allPixG[i,j], allPixB[i,j],));
pictureBox2.Image = res;
pictureBox2.Refresh();
}
}



int[,] resPixR1 = new int[width, height];
int[,] resPixG1 = new int[width, height];
int[,] resPixB1 = new int[width, height]; 

//Вот здеся убери единички



 private void filterPrevittesToolStripMenuItem_Click_1(object sender, EventArgs e)   
    {
            
        }




int l = 128 * 128;
            if (levelToolStripMenuItem1.Text != "Level")//считываем уровень выделения границ
            {
                l = Convert.ToInt32(levelToolStripMenuItem1.Text);
            }
            Bitmap res = new Bitmap(img);
            int width = img.Width;
            int height = img.Height;
            //создаем матричное преобразование 
            int[,] Sx = new int[,] { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 } };
            int[,] Sy = new int[,] { { -1, -1, -1 }, { 0, 0, 0 }, { 1, 1, 1 } };
            int[,] R_all = new int[width, height];
            int[,] G_all = new int[width, height];
            int[,] B_all = new int[width, height];
            int density = l;  
            //суммируем каналы всех пикселей
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    R_all[i, j] = res.GetPixel(i, j).R;
                    G_all[i, j] = res.GetPixel(i, j).G;
                    B_all[i, j] = res.GetPixel(i, j).B;
                }        
            int R_x = 0, R_y = 0, G_x = 0, G_y = 0, B_x = 0, B_y = 0;
            int R_c, G_c, B_c;  
            for (int i = 1; i < img.Width - 1; i++)
            {
                for (int j = 1; j < img.Height - 1; j++)
                {
                    R_x = 0; R_y = 0; G_x = 0; G_y = 0; B_x = 0; B_y = 0; R_c = 0; G_c = 0; B_c = 0;                    
                    for (int wi = -1; wi < 2; wi++)
                    {
                        for (int hw = -1; hw < 2; hw++)
                        {
                            //пробегая окрестности всех пикселей применяем преобразования Первитта 
                            R_c = R_all[i + hw, j + wi];
                            R_x += Sx[wi + 1, hw + 1] * R_c;
                            R_y += Sy[wi + 1, hw + 1] * R_c;

                            G_c = G_all[i + hw, j + wi];
                            G_x += Sx[wi + 1, hw + 1] * G_c;
                            G_y += Sy[wi + 1, hw + 1] * G_c;

                            B_c = B_all[i + hw, j + wi];
                            B_x += Sx[wi + 1, hw + 1] * B_c;
                            B_y += Sy[wi + 1, hw + 1] * B_c;
                        }
                    }
                    if (Math.Pow(R_x, 2) + Math.Pow(R_y, 2) > density
                        || Math.Pow(G_x, 2) + Math.Pow(G_y, 2) > density
                        || Math.Pow(B_x, 2) + Math.Pow(B_y, 2) > density)
                    {
                        res.SetPixel(i, j, Color.Black); // какие области пикселей имеют больше переход чем считаный порог - те и есть контур
                    }

                    else
                    {                        
                        res.SetPixel(i, j, Color.White); // иначе фон
                    }
                        
                }
            }
            pictureBox1.Image = res;
            pictureBox1.Refresh();
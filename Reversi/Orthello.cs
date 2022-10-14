using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reversi
{
    public partial class Form1 : Form
    {
        //Declaratie van variabelen
        int n = 6;
        float textboxlengte;
        float a;
        int[,] speelbord;
        bool RoodAanZet;
        int t = 1;
        int rood = 2;
        int blauw = 2;

        public Form1()
        {
            InitializeComponent();

            panel1.Paint += scherm;
            panel2.Paint += scherm2;
            panel1.MouseClick += klikken;
            textboxlengte = 600/ n;
            speelbord = new int[n, n];
            RoodAanZet = false;
            textBox1.Text = "2";
            textBox2.Text = "2";
            textBox3.Text = "Blauw begint";



        }
        public void scherm(object o, PaintEventArgs pea)
        {
            // leeg = 0, blauw = 1, rood = 2
            speelbord[2,2] = 1;
            speelbord[3,3] = 1;
            speelbord[2,3] = 2;
            speelbord[3,2] = 2;

            Pen ZwartePen = new Pen(Color.Black);
            Brush Rood = Brushes.Red;
            Brush Blauw = Brushes.Blue;


            if (t % 2 == 0)
                RoodAanZet = true;
            else
                RoodAanZet = false;

            for (int i = 0; i < n + 1; i++)
            {
                a = textboxlengte * i;
                pea.Graphics.DrawLine(ZwartePen, a, 0, a, 600);
                pea.Graphics.DrawLine(ZwartePen, 0, a, 600, a);
            }
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (speelbord[i, j] == 1)
                    {
                        pea.Graphics.FillEllipse(Blauw, textboxlengte * (i), textboxlengte * (j), 600 / n, 600 / n);
                    }
                    else if (speelbord[i, j] == 2)
                    {
                        pea.Graphics.FillEllipse(Rood, textboxlengte * (i), textboxlengte * (j), 600 / n, 600 / n);
                    }
        }
        public void scherm2(object o, PaintEventArgs pea)
        {
            pea.Graphics.FillEllipse(Brushes.Red, 1,1,40,40);
            pea.Graphics.FillEllipse(Brushes.Blue, 1,41,40,40);
        }


        public void klikken(object o, MouseEventArgs mea)
        {
            try
            {
                int Horizontaal = (mea.X / Convert.ToInt32(textboxlengte));
                int Vertikaal = (mea.Y / Convert.ToInt32(textboxlengte));

                if (RoodAanZet == false && speelbord[Horizontaal, Vertikaal] == 0)
                {
                    speelbord[Horizontaal, Vertikaal] = 1;
                    textBox3.Text = "Rood is aan zet";
                    blauw += 1;
                    string s2 = blauw.ToString();
                    textBox1.Text = s2;
                }
                else if (RoodAanZet == true && speelbord[Horizontaal, Vertikaal] == 0)
                {
                    speelbord[Horizontaal, Vertikaal] = 2;
                    textBox3.Text = "Blauw is aan zet";
                    rood += 1;
                    string s1 = rood.ToString();
                    textBox2.Text = s1;
                }
                t += 1;
                panel1.Invalidate();
            }
            catch
            {
                Console.WriteLine("Error, Klik in het speelveld!");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

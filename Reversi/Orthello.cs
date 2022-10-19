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
using System.Xml.Linq;

namespace Reversi
{
    public partial class Form1 : Form
    {
        //Declaratie van variabelen
        int n;
        float rbox;
        string[,] speelbord;
        bool RoodAanZet;
        List<int[]> moves;

        public Form1()
        {
            InitializeComponent();

            defaults();

            boardpanel.Paint += drawboard;
            scorepanel.Paint += drawscore;

            boardpanel.MouseClick += click;
        }

        public void defaults()
        {
            n = 6;
            rbox = 600 / n;
            speelbord = new string[n, n];

            RoodAanZet = false;

            for (int i = 0; i < n; i++) { for (int j = 0; j < n; j++) { speelbord[i, j] = "O"; } }
            speelbord[n / 2 - 1, n / 2 - 1] = speelbord[n / 2, n / 2] = "B";
            speelbord[n / 2 - 1, n / 2] = speelbord[n / 2, n / 2 - 1] = "R";

            redscorelabel.Text   = "2";
            bluescorelabel.Text  = "2";
            gamestatus.Text = "Blauw begint";

            updatemoves();
        }
        public void drawboard(object o, PaintEventArgs pea)
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    Brush Col = Brushes.White;
                    if      (speelbord[i, j] == "B")  Col = Brushes.Blue;
                    else if (speelbord[i, j] == "R")  Col = Brushes.Red;

                    pea.Graphics.FillEllipse(Col, rbox * i, rbox * j, rbox, rbox);
                    pea.Graphics.DrawRectangle(new Pen(Color.Black), rbox * i, rbox * j, rbox, rbox);
                }

            for (int index = 0; index < moves.Count; index++)
            {
                int[] square = moves[index];
                pea.Graphics.DrawEllipse(new Pen(Color.Black), rbox * square[0], rbox * square[1], rbox, rbox);
            }
        }

        public void updatescore()
        {
            int redscore  = 0;
            int bluescore = 0;

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    if (speelbord[i, j] == "B") bluescore++;
                    else if (speelbord[i, j] == "R") redscore++;
                }

            bluescorelabel.Text = $"{bluescore}";
            redscorelabel.Text  = $"{redscore}";

        }
        public void drawscore(object o, PaintEventArgs pea)
        {
            pea.Graphics.FillEllipse(Brushes.Red, 1,1,40,40);
            pea.Graphics.FillEllipse(Brushes.Blue, 1,41,40,40);
        }


        public void click(object o, MouseEventArgs mea)
        {
            try
            {
                int boardx = (mea.X / Convert.ToInt32(rbox));
                int boardy = (mea.Y / Convert.ToInt32(rbox));

                if (RoodAanZet == false && speelbord[boardx, boardy] == "O")
                {
                    speelbord[boardx, boardy] = "B";
                    gamestatus.Text = "Rood is aan zet";
                }
                else if (RoodAanZet == true && speelbord[boardx, boardy] == "O")
                {
                    speelbord[boardx, boardy] = "R";
                    gamestatus.Text = "Blauw is aan zet";
                }
                RoodAanZet = !RoodAanZet;
                boardpanel.Invalidate();

                updatemoves();
                updatescore();
            }
            catch
            {
                Console.WriteLine("Error, Klik in het speelveld!");
            }
        }

        public void updatemoves()
        {
            moves = new List<int[]> { }; 
            string neighbor;
            string neighborneigbor;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {

                    int[,] surrounds = { { 1,   0 }, {   0, - 1 },
                                         { 1,   1 }, { - 1,   0 },
                                         { 1, - 1 }, { - 1,   1 },
                                         { 0,   1 }, { - 1, - 1 } };

                    for (int index = 0; index < 8; index ++)
                    {
                        try
                        {
                            neighbor        = speelbord[i +     surrounds[index,0], j +     surrounds[index,1]];
                            neighborneigbor = speelbord[i + 2 * surrounds[index,0], j + 2 * surrounds[index,1]];

                            if ((RoodAanZet && neighbor == "B" && neighborneigbor == "R") || 
                               (!RoodAanZet && neighbor == "R" && neighborneigbor == "B"))
                            {
                                moves.Add(new int[] { i, j });
                            }
                        }
                        catch { /* INDEX OUT OF RANGE */ }
                    }
                }
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

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
        Board board;

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

            board = new Board(n);
            board.updatescore(bluescorelabel, redscorelabel);

            gamestatus.Text = "Blauw begint";
        }


        public void drawboard(object o, PaintEventArgs pea)
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    Brush piecebrush = board.getsquarecolor(i, j, this.BackColor);

                    pea.Graphics.FillEllipse  (piecebrush,           rbox * i, rbox * j, rbox, rbox);
                    pea.Graphics.DrawRectangle(new Pen(Color.Black), rbox * i, rbox * j, rbox, rbox);
                }

            for (int index = 0; index < board.moves.Count; index++)
            {
                int[] square = board.moves[index];
                pea.Graphics.DrawEllipse(new Pen(Color.Black), rbox * square[0], rbox * square[1], rbox, rbox);
            }
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

                if (board.player == "BLUE" && board.grid[boardx, boardy] == "O")
                    board.grid[boardx, boardy] = "B";
                else if (board.player == "RED" && board.grid[boardx, boardy] == "O")
                    board.grid[boardx, boardy] = "R";

                board.switchplayer(gamestatus);
                board.updatemoves();
                board.updatescore(bluescorelabel, redscorelabel);

                boardpanel.Invalidate();
            }
            catch
            {
                Console.WriteLine("Error, Klik in het speelveld!");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
    }


    public class Board
    {
        public int         dimension;
        public string[,]   grid;
        public List<int[]> moves;
        public string      player;

        public Board( int n )
        {
            this.dimension = n;

            this.grid = new string[n, n];
            for (int i = 0; i < n; i++) for (int j = 0; j < n; j++) this.grid[i, j] = "O";
            this.grid[n / 2 - 1, n / 2 - 1] = this.grid[n / 2, n / 2] = "B";
            this.grid[n / 2 - 1, n / 2] = this.grid[n / 2, n / 2 - 1] = "R";

            this.player = "BLUE";

            this.updatemoves();
        }

        public Brush getsquarecolor(int i, int j, Color background)
        {
            Color col = background;
            if (this.grid[i, j] == "R")      col = Color.Red;
            else if (this.grid[i, j] == "B") col = Color.Blue;

            Brush br = new SolidBrush(col);
            return br;
        }

        public void updatemoves()
        {
            moves = new List<int[]> { };
            string neighbor, neighborneigbor;

            for (int i = 0; i < this.dimension; i++)
                for (int j = 0; j < this.dimension; j++)
                    if ( (this.grid[i, j] == "B" && this.player == "BLUE") || (this.grid[i, j] == "R" && this.player == "RED"))
                    {
                        int[,] surrounds = { { 1,   0 }, {   0, - 1 },
                                             { 1,   1 }, { - 1,   0 },
                                             { 1, - 1 }, { - 1,   1 },
                                             { 0,   1 }, { - 1, - 1 } };

                        for (int index = 0; index < 8; index++)
                        {
                            try
                            {
                                neighbor        = this.grid[i +     surrounds[index, 0], j +     surrounds[index, 1]];
                                neighborneigbor = this.grid[i + 2 * surrounds[index, 0], j + 2 * surrounds[index, 1]];

                                if (neighbor != this.grid[i, j] && neighbor != "O" && neighborneigbor == "O")
                                {
                                    moves.Add(new int[] { i + 2 * surrounds[index, 0], j + 2 * surrounds[index, 1]  });
                                }
                            }
                            catch { /* INDEX OUT OF RANGE */ }
                        }
                    }
        }

        public void updatescore(Label bluescorelabel, Label redscorelabel)
        {
            int redscore = 0;
            int bluescore = 0;

            for (int i = 0; i < this.dimension; i++)
                for (int j = 0; j < this.dimension; j++)
                {
                    if (this.grid[i, j] == "B") bluescore++;
                    else if (this.grid[i, j] == "R") redscore++;
                }

            bluescorelabel.Text = $"{bluescore}";
            redscorelabel.Text  = $"{redscore}";
        }

        public void switchplayer(Label gamestatus)
        {
            if (this.player == "BLUE")
            {
                this.player = "RED";
                gamestatus.Text = "Rood is aan zet";
            }
            else
            {
                this.player = "BLUE";
                gamestatus.Text = "Blauw is aan zet";
            }
        }
    }
}

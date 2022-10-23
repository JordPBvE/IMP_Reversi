using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Reversi
{
    public partial class Reversi : Form
    {
        //Declaratie van variabelen
        int n;
        float rbox;
        Board board;

        public Reversi()
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
                    Color squarecolor    = board.getsquarecolor(new Pt(i, j), this.BackColor);
                    Brush piecebrush     = new SolidBrush(squarecolor);
                    Brush lightpiecebrush = new SolidBrush(ControlPaint.LightLight(squarecolor));

                    if (squarecolor == this.BackColor)
                        lightpiecebrush = new SolidBrush(this.BackColor);

                    pea.Graphics.FillEllipse  (lightpiecebrush,     rbox * i,                  rbox * j,                    rbox,                    rbox);
                    pea.Graphics.DrawRectangle(new Pen(Color.Gray), rbox * i,                  rbox * j,                    rbox,                    rbox);
                    pea.Graphics.FillEllipse  (piecebrush,          rbox * i + (int)(rbox/40), rbox * j + (int)(rbox / 40), rbox - (int)(rbox / 20), rbox - (int)(rbox / 20));
                }

            for (int index = 0; index < board.moves.Count; index++)
            {
                Pt square = board.moves[index].point;
                pea.Graphics.DrawEllipse(new Pen(Color.Black), rbox * square.x, rbox * square.y, rbox, rbox);
            }
        }


        public void drawscore(object o, PaintEventArgs pea)
        {
            pea.Graphics.FillEllipse(Brushes.Red,  1, 1,  40, 40);
            pea.Graphics.FillEllipse(Brushes.Blue, 1, 41, 40, 40);
        }


        public void click(object o, MouseEventArgs mea)
        {
            try
            {
                Pt boardpos = new Pt(mea.X / Convert.ToInt32(rbox), mea.Y / Convert.ToInt32(rbox));

                if (board.movepossible(boardpos))
                {
                    board.placepiece(boardpos);
                    board.switchplayer(gamestatus);
                    board.updatemoves();
                    board.updatescore(bluescorelabel, redscorelabel);

                    boardpanel.Invalidate();
                }
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
        public int dimension;
        public GridWrapper grid;
        public List<Move> moves;
        public string player;

        public Board(int n)
        {
            this.dimension = n;
            
            this.player = "BLUE";
            this.grid = new GridWrapper(n, n);

            for (int i = 0; i < n; i++) 
                for (int j = 0; j < n; j++) 
                    this.grid[i, j] = "O";

            this.grid[n / 2 - 1, n / 2 - 1] = this.grid[n / 2, n / 2]     = "B";
            this.grid[n / 2 - 1, n / 2]     = this.grid[n / 2, n / 2 - 1] = "R";

            this.updatemoves();
        }

        public Color getsquarecolor(Pt point, Color background)
        {
            if      (this.grid[point] == "R") return Color.Red;
            else if (this.grid[point] == "B") return Color.Blue;

            return background;
        }

        public bool movepossible(Pt point)
        {
            bool ispossible = false;

            for (int i = 0; i < this.moves.Count; i++)
                if (point == this.moves[i].point)
                    ispossible = true;

            return ispossible;
        }

        public void placepiece(Pt point)
        {
            this.grid[point] = Char.ToString(this.player[0]);

            for (int i = 0; i < moves.Count; i++)
                if (point == moves[i].point)
                    for (int k = 0; k < this.moves[i].steps.Count; k++)
                    {
                        this.grid[moves[i].steps[k]] = Char.ToString(this.player[0]);
                    }
        }

        public void updatemoves()
        {
            moves = new List<Move> { };

            Pt startpoint;
            Pt neighbor;
            string neighborvalue;
            List<Pt> steps;

            for (int i = 0; i < this.dimension; i++)
                for (int j = 0; j < this.dimension; j++)
                    if (this.grid[i, j] == Char.ToString(this.player[0]))
                    {
                        startpoint = new Pt(i, j);
                        List<Pt> directions = new List<Pt>
                        {
                            new Pt(   1,   0 ), new Pt(   1,   1 ), new Pt(   1, - 1 ),
                            new Pt( - 1,   0 ), new Pt( - 1,   1 ), new Pt( - 1, - 1 ),
                            new Pt(   0,   1 ), new Pt(   0, - 1 )
                        };

                        foreach (Pt direction in directions)
                        {
                            try
                            {
                                steps = new List<Pt> { };

                                neighbor = startpoint + direction;
                                neighborvalue = this.grid[neighbor];

                                while (neighborvalue != Char.ToString(this.player[0]) && neighborvalue != "O")
                                {
                                    steps.Add(neighbor);
                                    neighbor += direction;

                                    neighborvalue = this.grid[neighbor];
                                }

                                if (neighborvalue == "O" && steps.Count > 0)                             
                                    moves.Add(new Move(neighbor, steps));
                                
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
            redscorelabel.Text = $"{redscore}";
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

    public class GridWrapper : Tuple<int, int>
    {
        public string[,] Grid { set; get; }

        public string this[Pt point]
        {
            get => Grid [point.x, point.y];
            set => Grid [point.x, point.y] = value;
        }
        public string this[int x, int y]
        {
            get => Grid[x, y];
            set => Grid[x, y] = value;
        }

        public GridWrapper(int dim1, int dim2) : base(dim1, dim2)
        {
            Grid = new string[dim1, dim2];
        }
    }

    public struct Move
    {
        public List<Pt> steps;
        public Pt       point;

        public Move(Pt point, List<Pt> steps)
        {
            this.steps = steps;
            this.point = point;
        }
    }

    public class Pt
    {
        public int x;
        public int y;

        public Pt(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Pt operator +(Pt a, Pt b)
        => new Pt(a.x + b.x, a.y + b.y);


        public static bool operator ==(Pt a, Pt b)
        {
            return (a.x == b.x && a.y == b.y);
        }
        public static bool operator !=(Pt a, Pt b)
        {
            return !(a.x == b.x && a.y == b.y);
        }
    }
}

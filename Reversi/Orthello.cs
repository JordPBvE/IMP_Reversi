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
                    Point point = new Point(i, j);

                    Brush piecebrush = board.getsquarecolor(point, this.BackColor);
                    pea.Graphics.FillEllipse(piecebrush, rbox * i, rbox * j, rbox, rbox);
                    pea.Graphics.DrawRectangle(new Pen(Color.Black), rbox * i, rbox * j, rbox, rbox);
                }

            for (int index = 0; index < board.moves.Count; index++)
            {
                Point square = board.moves[index].point;
                pea.Graphics.DrawEllipse(new Pen(Color.Black), rbox * square.x, rbox * square.y, rbox, rbox);
            }
        }


        public void drawscore(object o, PaintEventArgs pea)
        {
            pea.Graphics.FillEllipse(Brushes.Red, 1, 1, 40, 40);
            pea.Graphics.FillEllipse(Brushes.Blue, 1, 41, 40, 40);
        }


        public void click(object o, MouseEventArgs mea)
        {
            try
            {
                int boardx = (mea.X / Convert.ToInt32(rbox));
                int boardy = (mea.Y / Convert.ToInt32(rbox));

                if (board.movepossible(boardx, boardy))
                {
                    board.placepiece(boardx, boardy);
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

            this.grid = new GridWrapper(n, n);

            for (int i = 0; i < n; i++) for (int j = 0; j < n; j++) this.grid[i, j] = "O";
            this.grid[n / 2 - 1, n / 2 - 1] = this.grid[n / 2, n / 2] = "B";
            this.grid[n / 2 - 1, n / 2] = this.grid[n / 2, n / 2 - 1] = "R";

            this.player = "BLUE";

            this.updatemoves();
        }

        public Brush getsquarecolor(Point point, Color background)
        {
            Color col = background;
            if      (this.grid[point] == "R") col = Color.Red;
            else if (this.grid[point] == "B") col = Color.Blue;

            Brush br = new SolidBrush(col);
            return br;
        }

        public bool movepossible(int x, int y)
        {
            bool ispossible = false;

            for (int i = 0; i < this.moves.Count; i++)
                if (x == this.moves[i].point.x && y == this.moves[i].point.y)
                    ispossible = true;

            return ispossible;
        }

        public void placepiece(int a, int b)
        {
            Point point = new Point(a, b);

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
            Point neighbor;
            string neighborvalue;
            List<Point> steps;

            for (int i = 0; i < this.dimension; i++)
                for (int j = 0; j < this.dimension; j++)
                    if ((this.grid[i, j] == "B" && this.player == "BLUE") || (this.grid[i, j] == "R" && this.player == "RED"))
                    {
                        Point pt = new Point(i, j);

                        List<Point> surrounds = new List<Point>
                        {
                            new Point(   1,   0 ), new Point(   1,   1 ), new Point(   1, - 1 ),
                            new Point( - 1,   0 ), new Point( - 1,   1 ), new Point( - 1, - 1 ),
                            new Point(   0,   1 ), new Point(   0, - 1 )
                        };

                        for (int index = 0; index < 8; index++)
                        {
                            try
                            {
                                steps = new List<Point> { };

                                neighbor = pt + surrounds[index];

                                neighborvalue = this.grid[neighbor.x, neighbor.y];

                                while (neighborvalue != Char.ToString(this.player[0]) && neighborvalue != "O")
                                {
                                    neighbor += surrounds[index];
                                    neighborvalue = this.grid[neighbor.x, neighbor.y];

                                    steps.Add(neighbor);
                                }

                                if (neighborvalue == "O" && steps.Count > 0)
                                {
                                    Move newmove = new Move(neighbor, steps);
                                    moves.Add(newmove);
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

        public string this[Point point]
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

    public class Move
    {
        public List<Point> steps;
        public Point       point;

        public Move(Point point, List<Point> steps)
        {
            this.steps = steps;
            this.point = point;
        }
    }

    public class Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Point operator +(Point a, Point b)
        => new Point(a.x + b.x, a.y + b.y);


        public static bool operator ==(Point a, Point b)
        {
            return (a.x == b.x && a.y == b.y);
        }
        public static bool operator !=(Point a, Point b)
        {
            return !(a.x == b.x && a.y == b.y);
        }
    }
}

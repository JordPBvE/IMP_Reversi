using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Reversi
{
    public partial class Reversi : Form
    {
        //Declaratie van variabelen
        int   n;
        bool  hint;
        float rbox;

        Board board;

        Player P1;
        Player P2;

        public Reversi()
        {
            InitializeComponent();

            player2.Items.Add(new Player("ROOD", "P2", Color.Red));
            player2.Items.Add(new Player("ORANJE", "P2", Color.Orange));
            player2.Items.Add(new Player("GEEL", "P2", Color.Yellow));

            player1.Items.Add(new Player("BLAUW", "P1", Color.Blue));
            player1.Items.Add(new Player("PAARS", "P1", Color.Purple));
            player1.Items.Add(new Player("TEAL", "P1", Color.Teal));

            player1.SelectedIndex = player2.SelectedIndex = 0;

            P1 = (Player)player1.SelectedItem;
            P2 = (Player)player2.SelectedItem;

            defaults(null, null);

            boardpanel.Paint += drawboard;
            scorepanel.Paint += drawscore;

            boardpanel.MouseClick += click;

            hintbutton.Click       += help;
            resetbutton.Click      += defaults;
            nTrackbar.ValueChanged += defaults;

            player1.SelectedIndexChanged += playerchanged;
            player2.SelectedIndexChanged += playerchanged;
        }

        public void defaults(object o, EventArgs ea)
        {

            n = 2* nTrackbar.Value;
            hint = false;
            rbox = boardpanel.Size.Height / n;

            board = new Board(n, P1, P2);
            board.updatescore(bluescorelabel, redscorelabel);

            boardpanel.Invalidate();

            gamestatus.Text = P1.name + " BEGINT";
        }


        public void drawboard(object o, PaintEventArgs pea)
        {
            if (board.moves.Count == 0){drawwinner(o, pea); return;}

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    Color squarecolor     = board.getsquarecolor(new Pt(i, j), this.BackColor);
                    Brush piecebrush      = new SolidBrush(squarecolor);
                    Brush lightpiecebrush = new SolidBrush(ControlPaint.LightLight(squarecolor));

                    if (squarecolor == this.BackColor)
                        lightpiecebrush = new SolidBrush(this.BackColor);

                    pea.Graphics.FillEllipse  (lightpiecebrush,     rbox * i,                  rbox * j,                    rbox,                    rbox);
                    pea.Graphics.DrawRectangle(new Pen(Color.Gray), rbox * i,                  rbox * j,                    rbox,                    rbox);
                    pea.Graphics.FillEllipse  (piecebrush,          rbox * i + (int)(rbox/40), rbox * j + (int)(rbox / 40), rbox - (int)(rbox / 20), rbox - (int)(rbox / 20));
                }
            if (hint)
                for (int index = 0; index < board.moves.Count; index++)
                {
                    Pt square = board.moves[index].point;
                    pea.Graphics.DrawEllipse(new Pen(Color.Black), rbox * square.x, rbox * square.y, rbox, rbox);
                }
            hint = false;
        }


        public void drawscore(object o, PaintEventArgs pea)
        {
            pea.Graphics.FillEllipse(new SolidBrush(P1.color),  1, 1,  40, 40);
            pea.Graphics.FillEllipse(new SolidBrush(P2.color), 1, 41, 40, 40);
        }

        public void drawwinner(object o, PaintEventArgs pea)
        {
            gamestatus.Text = "We have a winner!";

            int redscore  = int.Parse(redscorelabel.Text);
            int bluescore = int.Parse(bluescorelabel.Text);

            Brush br;
            string message;
            if (redscore > bluescore)
            {
                br = new SolidBrush(ControlPaint.Light(Color.Red));
                message = P1.name + " WINT";
            }
            else if (redscore < bluescore)
            {
                br = new SolidBrush(ControlPaint.Light(Color.Blue));
                message = "BLAUW WINT";
            }
            else
            {
                br = new SolidBrush(Color.LightGray);
                message = "REMISE";
            }
            pea.Graphics.FillRectangle(br, 0, 0, 600, 600);
            pea.Graphics.DrawString(message, 
                                    new Font("Arial", 20), 
                                    new SolidBrush(Color.Black), 
                                    new Rectangle(160, 230, 200, 50), 
                                    new StringFormat() { Alignment = StringAlignment.Center });
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

        public void help(object o, EventArgs ea)
        {
            hint = true;
            boardpanel.Invalidate();
        }

        public void playerchanged(object o, EventArgs ea)
        {
            this.P1 = (Player)player1.SelectedItem;
            this.P2 = (Player)player2.SelectedItem;

            if (board.player.sign == "P1")
            {
                board.player = P1;
                board.waiting = P2;
            } else
            {
                board.player = P2;
                board.waiting = P1;
            }

            boardpanel.Invalidate();
            scorepanel.Invalidate();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void sizelabel_Click(object sender, EventArgs e) { }
    }


    public class Board
    {
        public int dimension;
        public GridWrapper grid;
        public List<Move> moves;
        public Player player;
        public Player waiting;

        public Board(int n, Player p1, Player p2)
        {
            this.dimension = n;
            
            this.player  = p1;
            this.waiting = p2;

            this.grid = new GridWrapper(n, n);

            for (int i = 0; i < n; i++) 
                for (int j = 0; j < n; j++) 
                    this.grid[i, j] = "O";

            this.grid[n / 2 - 1, n / 2 - 1] = this.grid[n / 2, n / 2]     = "P1";
            this.grid[n / 2 - 1, n / 2]     = this.grid[n / 2, n / 2 - 1] = "P2";

            this.updatemoves();
        }

        public Color getsquarecolor(Pt point, Color background)
        {
            if      (this.grid[point] == this.player.sign)  return this.player.color;
            else if (this.grid[point] == this.waiting.sign) return this.waiting.color;

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
            this.grid[point] = this.player.sign;

            for (int i = 0; i < moves.Count; i++)
                if (point == moves[i].point)
                    for (int k = 0; k < this.moves[i].steps.Count; k++)
                    {
                        this.grid[moves[i].steps[k]] = this.player.sign;
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
                    if (this.grid[i, j] == this.player.sign)
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

                                while (neighborvalue != this.player.sign && neighborvalue != "O")
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
        public void updatescore(Label p1scorelabel, Label p2scorelabel)
        {
            int p1score = 0;
            int p2score = 0;

            for (int i = 0; i < this.dimension; i++)
                for (int j = 0; j < this.dimension; j++)
                {
                    if (this.grid[i, j] == "P1") p1score++;
                    else if (this.grid[i, j] == "P2") p2score++;
                }

            p1scorelabel.Text = $"{p1score}";
            p2scorelabel.Text = $"{p2score}";
        }

        public void switchplayer(Label gamestatus)
        {
            Player buffer = this.waiting;

            this.waiting = this.player;
            this.player  = buffer;

            gamestatus.Text = this.player.name + " IS AAN ZET";
        }
    }

    public class Player
    {
        public string name;
        public string sign;
        public Color color;

        public Player(string name, string sign, Color color)
        {
            this.name = name;
            this.sign = sign;
            this.color = color;
        }

        public override string ToString()
            => name;
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

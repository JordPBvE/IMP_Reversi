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
        // declaration of global variables
        int   n;
        bool  helping;
        float rbox;

        Board board;

        Player P1;
        Player P2;

        // initializing the form. defining basic properties and defining eventhandlers
        public Reversi()
        {
            InitializeComponent();

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

        // function defining default settings for the game
        public void defaults(object o, EventArgs ea)
        {

            n = 2* nTrackbar.Value;
            helping = false;
            rbox = boardpanel.Size.Height / n;

            board = new Board(n, P1, P2);
            board.updatescore(p1scorelabel, p2scorelabel);

            boardpanel.Invalidate();

            gamestatus.Text = P1.name + " BEGINT";
        }

        // draw the board by looping through the grid, and for each cell, drawing the cell borders and the possible piece in the cell
        public void drawboard(object o, PaintEventArgs pea)
        {
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
            if (helping)
                for (int index = 0; index < board.moves.Count; index++)
                {
                    Pt square = board.moves[index].point;
                    pea.Graphics.DrawEllipse(new Pen(Color.Black), rbox * square.x, rbox * square.y, rbox, rbox);
                }
            if (board.moves.Count == 0){drawwinner(o, pea);}
        }

        // draw the colored circles on the scireboard
        public void drawscore(object o, PaintEventArgs pea)
        {
            pea.Graphics.FillEllipse(new SolidBrush(P1.color),  1, 1,  40, 40);
            pea.Graphics.FillEllipse(new SolidBrush(P2.color), 1, 41, 40, 40);
        }

        // if the game is finished, display text saying who won and color the board to the color of the winner
        public void drawwinner(object o, PaintEventArgs pea)
        {
            gamestatus.Text = "FINISHED!";

            int p1score = int.Parse(p1scorelabel.Text);
            int p2score  = int.Parse(p2scorelabel.Text);

            Brush br;
            string message;

            if (p1score > p2score)
            {
                br = new SolidBrush(Color.FromArgb(100, P1.color));
                message = P1.name + " WINT";
            }
            else if (p1score < p2score)
            {
                br = new SolidBrush(Color.FromArgb(100, P2.color));
                message = P2.name + " WINT";
            }
            else
            {
                br = new SolidBrush(Color.FromArgb(100, Color.LightGray));
                message = "REMISE";
            }
            pea.Graphics.FillRectangle(br, 0, 0, board.dimension * (rbox), board.dimension * (rbox));
            pea.Graphics.DrawString(message, 
                                    new Font("Arial", 20), 
                                    new SolidBrush(Color.Black), 
                                    new Rectangle(160, 230, 200, 50), 
                                    new StringFormat() { Alignment = StringAlignment.Center });
        }

        // eventhandler for clicking on the board
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
                    board.updatescore(p1scorelabel, p2scorelabel);

                    boardpanel.Invalidate();
                }
                Console.WriteLine("clicked");
            }
            catch { /* clicked outside of the playing field */ }
        }

        // pressing help button toggles showing the moves
        public void help(object o, EventArgs ea)
        {
            helping = !helping;
            boardpanel.Invalidate();
        }

        // when changing one of the comboboxes, the players are updated to the selected presets. 
        // both in the global variable a in the player and waiting variable in the board.
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

            board.switchplayer(gamestatus);
            board.switchplayer(gamestatus);
        }
    }

    // board class containing the grid and methods analysing and or changing the grid
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

            // set starting values for a new grid
            for (int i = 0; i < n; i++) 
                for (int j = 0; j < n; j++) 
                    this.grid[i, j] = "O";

            this.grid[n / 2 - 1, n / 2 - 1] = this.grid[n / 2, n / 2]     = "P1";
            this.grid[n / 2 - 1, n / 2]     = this.grid[n / 2, n / 2 - 1] = "P2";

            this.updatemoves();
        }

        // get the color of a squarre at given point, if its empty, return the background color
        public Color getsquarecolor(Pt point, Color background)
        {
            if      (this.grid[point] == this.player.sign)  return this.player.color;
            else if (this.grid[point] == this.waiting.sign) return this.waiting.color;

            return background;
        }

        // return if a point is in the move list
        public bool movepossible(Pt point)
        {
            for (int i = 0; i < this.moves.Count; i++)
                if (point == this.moves[i].point)
                    return true;

            return false;
        }

        // put down a piece of the active player at a given point in the grid.
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

        // the following function creates a list of all available moves to the player whos turn it is. it works by going through
        // the grid, and for each of the pieces of the active player; check all directions to see if there is a series of pieces
        // of the opposing player followed by an empty square.
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
                        // list of 'vectors' corresponding to all neighboring cells
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

                                // while loop goes one step in the selected direction, checks for a piece of the opposing player,
                                // if its there, repeat until we come to an empty square or one of our own pieces.
                                while (neighborvalue != this.player.sign && neighborvalue != "O")
                                {
                                    steps.Add(neighbor);
                                    neighbor += direction;

                                    neighborvalue = this.grid[neighbor];
                                }

                                // if the piece after a series of opposing pieces is empty, add it to the move list.
                                if (neighborvalue == "O" && steps.Count > 0)                             
                                    moves.Add(new Move(neighbor, steps));
                                
                            }
                            catch { /* INDEX OUT OF RANGE */ }
                        }
                    }
        }

        // function that counts the amount of pieces each of the player has on the board and displays it to the scoreoard
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

        // this function switches which player is to move next
        public void switchplayer(Label gamestatus)
        {
            Player buffer = this.waiting;

            this.waiting = this.player;
            this.player  = buffer;

            gamestatus.Text = this.player.name + " IS AAN ZET";
        }
    }

    // this player class represents the two players, the purpose of this class is to make the code more readeable and modular. 
    // statements like gamestatus.Text = player.name + "IS AAN ZET" are valid whichever color the player happens to be.
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

        // defining how to display the player class as a string so that in the comboboxes, the names are displayerd properly
        public override string ToString()
            => name;
    }


    // this is a wrapper for the board grid. the purpose of this class is to make it possible to input points into the grid:
    // 
    // Pt point = new Pt(i,j)
    // grid[point] == grid[i,j] is true
    //
    // this makes the code more readable in instances like: this.grid[moves[i].steps[k]] instead of this.grid[ moves[i].steps[k][0], this.grid[moves[i].steps[k][1] ]
    public class GridWrapper : Tuple<int, int>
    {
        public string[,] Grid { set; get; }

        public GridWrapper(int dim1, int dim2) : base(dim1, dim2)
        {
            Grid = new string[dim1, dim2];
        }

        // defining what grid[point] means
        public string this[Pt point]
        {
            get => Grid [point.x, point.y];
            set => Grid [point.x, point.y] = value;
        }

        // defining what grid[x, y] means
        public string this[int x, int y]
        {
            get => Grid[x, y];
            set => Grid[x, y] = value;
        }

    }


    // the move class represents moves in the game. each move consists of the point where the piece is placed and the pieces of the other color it then encloses
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


    // Pt class represents points in the game grid
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

        // define Pt equality and inequality
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThinkLib;
using System.IO;
using System.Diagnostics;
using System.Windows.Threading;



namespace TickTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int Human_score,AI_score,Draw_score;//variables used to keep score of the games
        Turtle Draw;//turtle used to draw the strike throughs
        Logic AI;//main AI of the game
        bool running;//variable used to manage whether is the players turn or not
        DispatcherTimer computer_AI = new DispatcherTimer();
        static int[,] game { get; set; }//the array used to keep track of the game
        public MainWindow()
        {
            InitializeComponent();
            play.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/TickTacToe;component/Images/board.jpg")));  
            play.MouseUp += new MouseButtonEventHandler(MouseClick);//checks for any input from mouse 
            computer_AI.Tick += new EventHandler(comp);//the event handel that allows the pc to play the game
            computer_AI.Interval = new TimeSpan(0, 0, 0,0,260);//set time interval takes for computer to make a coice so that it isnt instant and gives player a better visual experience 
            Draw = new Turtle(play);//creates turtle on the grid
            Draw.Visible = false;
            Draw.BrushWidth = 10;
            game = new int[3, 3];//creates the array
        }
        public void BeginGame()//begins a new match
        {
            if (btnE.IsChecked == true)
                AI = new Logic(1, game);//creates a new instance of the Logic class that computer can use as an AI
            else if (btnH.IsChecked == true)
                AI = new Logic(2, game);
            Draw.Heading = 0;
            if (Shared_Variables.Player_goingFirst == 'Y')//decides the trun depedning on we the player chose to play first
            {
                running = true;
                computer_AI.IsEnabled = false;
                Shared_Variables.Player_goingFirst = 'N';
            }
            else if(Shared_Variables.Player_goingFirst == 'N')
            {  
                running = false;
                computer_AI.IsEnabled = true;
                Shared_Variables.Player_goingFirst = 'Y';
            }
        }
        public void DrawO(double bot, double left)//Draws a "O" at a given position
        {
            Image O = new Image();
            O.Source = new BitmapImage(new Uri("pack://application:,,,/TickTacToe;component/Images/O.png"));
            O.Height = 60;
            O.Width = 60;
            Canvas.SetTop(O, bot);
            Canvas.SetLeft(O, left);
            play.Children.Add(O);
        }
        public void DrawX(double bot, double left)//Draws an "X" at a given position
        {
            Image X = new Image();
            X.Source = new BitmapImage(new Uri("pack://application:,,,/TickTacToe;component/Images/X.png"));
            X.Height = 60;
            X.Width = 60;
            Canvas.SetTop(X, bot);
            Canvas.SetLeft(X, left);
            play.Children.Add(X);
        }
        public Point Get_Grid_coord(Point Location)//gets the location of where the shape will be drawn on the grind given the equivalent point in the array
        {
            double Row = Location.Y, Col = Location.X;
            int Y_coord, X_coord;
            switch (Row)//maps the equivalent array points given the grid points
            {
                case 0: Y_coord = 50; break;
                case 1: Y_coord = 150; break;
                case 2: Y_coord = 250; break;
                default: Y_coord = 0; break;
            }
            switch (Col)
            {
                case 0: X_coord = 225; break;
                case 1: X_coord = 365; break;
                case 2: X_coord = 500; break;
                default: X_coord = 0; break;
            }
            return new Point(X_coord, Y_coord);

        }
        private void Get_Array_coord(Point coordinate,bool Player)//gets the location of the shape in the array given the equivalent points on the grid
        {
            int X_pos = coordinate.X <= 325 ? 225 : coordinate.X < 460 ? 365 : 500;
            int Y_pos = coordinate.Y <= 135 ? 50 : coordinate.Y < 230 ? 150 : 250;
            int rPos, cPos;
            switch (Y_pos)//switch statement to get the position of the array relevant to the postion of shape is displayed
            {
                case 50: rPos = 0; break;
                case 150: rPos = 1; break;
                case 250: rPos = 2; break;
                default: rPos = 0; break;
            }
            switch (X_pos)
            {
                case 225: cPos = 0; break;
                case 365: cPos = 1; break;
                case 500: cPos = 2; break;
                default: cPos = 0; break;
            } 
            if (game[rPos, cPos] != 1 && game[rPos, cPos] != 2)//qualifier so that the player cannot select a square that has already been filled in
            {                                                  //A 1 signifies the postion that the player has entered a shape whilist a 2 is for the AI
                if(Player==true)
                {
                    game[rPos, cPos] = 2;
                    if (Shared_Variables.playerShape == 'X') 
                        DrawX(Y_pos, X_pos);
                    else
                        DrawO(Y_pos, X_pos);

                }
                else
                {
                    game[rPos, cPos] = 1;
                    if (Shared_Variables.ComputerShape == 'X')
                        DrawX(Y_pos, X_pos);
                    else
                        DrawO(Y_pos, X_pos);
                }
                  
                StrikeThrough(rPos, cPos);
            }
        }
        private void Strike(int num=0)//Displays who won and ends the current game session
        {
            game = new int[3, 3];//clears the current array of game so that its state is that of a new game
            computer_AI.IsEnabled = false;//stops the event handler so that ther computer doesnt keep trying tofigure out where to play as the game is over this makes it so there isnt an infinite loop
            running = false;//makes it so the player cannot play as it isn't the players turn
            if (num == 2)//2 represents the players number in the array so if the last shape to be played is 2 the player wins
            {
                MessageBox.Show("You win");
                win.Content = Convert.ToString(++Human_score);

            }
            else if (num == 1)//1 repesents the computers number
            {
                MessageBox.Show("Computer wins");
                lose.Content = Convert.ToString(++AI_score);
            }
            else// number 0 indicates a draw meaning that neither player nor the computer has won
            {
                MessageBox.Show("Draw");
                drawz.Content = Convert.ToString(++Draw_score);
            }
            Draw.Clear();
            play.Children.Clear();
            BeginGame();
        }
        private void Diaonal_Collsions(int num, int row, int col)
        {

            if (game[0, 0] == num && game[1, 1] == num && game[2, 2] == num)
            {
                Draw.Heading = 36;
                Draw.WarpTo(190,30);
                Draw.Forward(530);
                Strike(num);

            }
            else if (game[2, 0] == num && game[1, 1] == num && game[0, 2] == num)
            {
                Draw.Heading = 142;
                Draw.WarpTo(590,30);
                Draw.Forward(510);
                Strike(num);

            }

        }//checks to see if a single shape has filled out an entire diagonal
        private bool Row_Collisions(int num, int col)
        {
            for (int r = 0; r < 3; r++)
                if (game[r, col] != num)
                    return false;
            return true;
        }//checks to see if a single shape has filled out an entire row
        private bool Col_Collisions(int num, int row)
        {
            for (int c = 0; c < 3; c++)
                if (game[row, c] != num)
                    return false;

            return true;
        }//checks to see if a single shape has filled out an entire column 
        private void StrikeThrough(int row, int col)//checks to see if player or computer has won
        {
            int num = game[row, col];
            if (Col_Collisions(num, row) == true)//strikes through the relevant column that have resulted in a win in
            {
                Draw.Heading = 0;
                Draw.WarpTo(190, Get_Grid_coord(new Point(col, row)).Y+30);
                Draw.Forward(400);
                Strike(num);
                return;
            }

            else if (Row_Collisions(num, col) == true)//strikes through the relevant row that have resulted in a win in
            {
                Draw.Heading = 90;
                Draw.WarpTo(Get_Grid_coord(new Point(col, row)).X+30, 30);
                Draw.Forward(300);
                Strike(num);
                return;
            }
            else
            {
                Diaonal_Collsions(num, row, col);//strikes through the relevant diagonal that have resulted in a win in
            }           
            for (int r = 0; r < 3; r++)//qualfier to check we the game is drawn game this prevents an infinite loop from occuring when the state of game is thought to be ongoing while it is not as all squares are filed up
                for (int c = 0; c < 3; c++)
                    if (game[r, c] == 0)
                        return;//if all the squares are not filled up then this ends execution here 
            Strike();//only runs if all the squares are filled up thus runnning this function ends game in a draw
        }
        private void comp(object sender, EventArgs e)//event that gets the computers decision for where to place the shape
        {
           // MessageBox.Show("HERE running= " + running);
            Get_Array_coord(Get_Grid_coord(AI.Choice()),false);
            running = true;
            computer_AI.IsEnabled = false;
        }
        private void MouseClick(object sender, MouseButtonEventArgs e)//event for player clicking mouse
        {
            if (running==true)//only executes if it is the players turn
            {
                Point point = Mouse.GetPosition(play);//gets the players click postion
                computer_AI.IsEnabled = true;//sets the event handler for computer to true so that computer can decide on its next move
                running = false;//sets so player cannot make a move if the computer hasnt made a move
                Get_Array_coord(point,true);//gets players click coordinates in the array and checks if player has won
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)//starts the game
        {
            BeginGame();
            BtnStart.IsEnabled = false;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)//Goes to the main menu
        {
            Draw.Clear();
            game = new int[3, 3];
            Intro_screen intro = new Intro_screen();
            intro.Show();
            this.Close();
        }
    }
}

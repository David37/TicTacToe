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
using System.Windows.Shapes;

namespace TickTacToe
{
    /// <summary>
    /// Interaction logic for Intro_screen.xaml
    /// </summary>
    public static class Shared_Variables//variables used throughout the whole program 
    {
        public static char Player_goingFirst;
        public static char playerShape;
        public static char ComputerShape;
    }
    public partial class Intro_screen : Window
    {
        int click_Num = 1;//variable used check which intial click of user of either button
        public Intro_screen()//sets up intro screen
        {
            this.Background= new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/TickTacToe;component/Images/tic tac toe.jpg", UriKind.Absolute)));
            InitializeComponent();
            Option1.Visibility = Visibility.Hidden;
            Option2.Visibility = Visibility.Hidden;
        }
        private void intro()//changes the function of button 1 and 2 on intial click of user of either button
        {
            lblQuestions.Content = "Do you want to play first?";
            Option1.Content = "Yes";
            Option2.Content = "No";
            play.IsEnabled = false;
            click_Num = 0;
        }
        private void Button_Click(object sender, RoutedEventArgs e)//play button that intiates the game
        {
            lblQuestions.Content = "Choose what shape you want to play as";
            Option1.Visibility = Visibility.Visible;
            Option2.Visibility = Visibility.Visible;
            play.Visibility = Visibility.Hidden;
            play.IsEnabled = true;
            Option1.IsEnabled = true;
            Option2.IsEnabled = true;
            quit.Visibility= Visibility.Hidden; 

        }
        private void playGame()//goes into a new game instance
        {
            MainWindow win = new MainWindow();
            win.Show();//launches the main window where the game is going to b played
            this.Close();
        }
        private void Option1_Click(object sender, RoutedEventArgs e)
        {
            if (click_Num == 1)//checks to see if it is user clikcing to select the shape or whether they want to play first
            {
                intro();
                Shared_Variables.playerShape = 'X';
                Shared_Variables.ComputerShape = 'O';
            }
            else
            {
                Shared_Variables.Player_goingFirst = 'Y';
                playGame();
            }      
        }
        private void Option2_Click(object sender, RoutedEventArgs e)//same as option1 button but instead used "O" shape and "No" for player_goignFirst is playing option
        {
            if (click_Num == 1)
            {
                intro();
                Shared_Variables.playerShape = 'O';
                Shared_Variables.ComputerShape = 'X';//Decides shape computer takes depending on players shape choice
            }
            else
            {
                Shared_Variables.Player_goingFirst = 'N';
                playGame();
            }
        }
        private void Button2_Click(object sender, RoutedEventArgs e)//exits the game 
        {
            Environment.Exit(0);
        }
    }
}

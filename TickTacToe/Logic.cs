using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;




namespace TickTacToe
{
    class Logic
    {
        private int Difficulty { get; set; }
        public char Shape { get; set; }
        public int[,] CurrentGame { get; set; }
        public Logic(int Difficulty,int[,] CurrentGame)//constructor for the the logic class
        {
            this.CurrentGame = CurrentGame;
            this.Difficulty = Difficulty;//Sets difficulty player has chosen       
        }
        public Point Choice()//Difficulty that is run depending the players difficulty choice chosen
        {
            if (Difficulty == 1)
                return Easy();
            else
                return Hard();
        }
        private Point Easy()//Ai just randomly chooses any avaliable postition to insert a shape
        {
            while (true)
            {
                Random Rng = new Random();
                Point Array_Location = new Point(Rng.Next(0, 3), Rng.Next(0, 3));//chooses a random position in the array
                if (CurrentGame[(int)Array_Location.Y, (int)Array_Location.X]!=1 && CurrentGame[(int)Array_Location.Y, (int)Array_Location.X] != 2)                                                                                   
                    return new Point(Array_Location.X, Array_Location.Y); //if the chosen positon isnt occupied by either a computer or AI shape then those coordinates in the array are returned
            }
        }
        private bool DiagWin(int num)//checks for diagonal win
        {
            if (CurrentGame[0, 0] == num && CurrentGame[1, 1] == num && CurrentGame[2, 2] == num)
                return true;

            else if (CurrentGame[2, 0] == num && CurrentGame[1, 1] == num && CurrentGame[0, 2] == num)
                return true;
            else return false; 
        }
        private bool ColWin(int num)//checks for a column win
        {
            for (int r = 0; r < 3; r++)
                if (CurrentGame[0, r] == num && CurrentGame[1, r] == num && CurrentGame[2, r] == num)
                    return true;
            return false;
        }
        private bool RowWin(int num)//checks for a row win
        {
            for (int c = 0; c < 3; c++)
                if (CurrentGame[c, 0] == num && CurrentGame[c, 1] == num && CurrentGame[c, 2] == num)
                    return true; 
            return false;
        }
        public struct AiMove//keeps track of the score
        {
            public int x;
            public int y;
            public int score;
            public AiMove(int Score) : this() { score = Score; }
        }
        private int CheckForWin(int player)//checks to see if a win is possible
        {
            if (DiagWin(player) || RowWin(player) || ColWin(player))
            {
                return player;
            }
            for (int r = 0; r < 3; r++)//qualfier to check we the game is drawn game this prevents an infinite loop from occuring when the state of game is thought to be ongoing while it is not as all squares are filed up
                for (int c = 0; c < 3; c++)
                    if (CurrentGame[r, c] == 0)
                        return 0;
            return -1;
        }
        private AiMove BestMove(int[,] board,int player)//Gets the best move for the AI
        {   
            int _AI=1;
            int _human=2;
            if (CheckForWin(_AI)==1)
                return new AiMove(10);
            else if (CheckForWin(_human)==2)
                return new AiMove(-10);
            else if(CheckForWin(1)==-1)
                return new AiMove(0);
            List<AiMove> moves = new List<AiMove>();
            for (int y=0;y<3;y++)
                for(int x=0;x<3;x++)
                {
                    if (board[y, x] == 0)
                    {
                        AiMove move;
                        move.x = x;
                        move.y = y;
                        board[y,x] = player;
                        if (player == 1)
                            move.score = BestMove(board, _human).score;
                        else
                            move.score = BestMove(board, _AI).score;
                        moves.Add(move);
                        board[y,x] = 0;
                    }
                }
            int bestMove = 0;
            if (player == 1)
            {
                int bestscore = -1000000;
                for(int i= 0; i<moves.Count;i++)
                    if(moves[i].score>bestscore)
                    {
                        bestMove = i;
                        bestscore = moves[i].score;
                    }
            }
            else
            {
                int bestscore = 1000000;
                for (int i = 0; i < moves.Count; i++)
                    if (moves[i].score < bestscore)
                    {
                        bestMove = i;
                        bestscore = moves[i].score;
                    }
            }
            return moves[bestMove];
        }
        private Point Hard()//AI choses the best location for its shape that will either garuantee a win or a draw 
        {
            AiMove bestmovee = BestMove(CurrentGame, 1);
            return new Point(bestmovee.x, bestmovee.y);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//https://www.neverstopbuilding.com/blog/minimax

namespace Connect4_Console
{
    class Program
    {

        static int[,] Board = new int[6,7];
        
        static readonly int PLAYER_1 = 1;
        static readonly int PLAYER_2 = 2;

        static void Main(string[] args)
        {
            Console.WriteLine("\nWinning moves: " + GetWinningMoveCount(Board, PLAYER_2));


            Board[2, 3] = 1;
            Board[3, 4] = 1;
            Board[4, 5] = 1;
            Board[5, 6] = 1;
            PrintBoard(Board);



            int input = 0;


            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out input))
                {
                    if (!PlacePiece(input, Board, PLAYER_1)) continue;
                    PrintBoard(Board);

                    int maxScore = -10000, tempIndex = 0;
                    for (int i = 0; i < 7; i++)
                    {
                        int aiScore = GetWinningMoveCount(Board, PLAYER_2);
                        int p1Score = GetWinningMoveCount(Board, PLAYER_1);

                        int subScore = aiScore - p1Score;

                        Console.WriteLine("subScore : " + subScore);
                        if (subScore > maxScore)
                        {
                            maxScore = subScore;
                            tempIndex = i;
                        }
                    }

                    PlacePiece(tempIndex+1, Board, PLAYER_2);
                    Console.WriteLine("P2s move");
                    PrintBoard(Board);

                }
                else
                {
                    Console.WriteLine("Please enter a column between  1-7, as well as unoccupied");
                }
            }


            //Console.WriteLine("\nWinning moves: " + GetWinningMoveCount(Board, PLAYER_1));
        }


        //https://www.geeksforgeeks.org/minimax-algorithm-in-game-theory-set-3-tic-tac-toe-ai-finding-optimal-move/
        static int Minmax(int depth, bool isMaximizer)
        {
            int score = Evaluate(Board);

            // If Maximizer has won the game return his/her 
            // evaluated score 
            if (score == 1000) return score;

            // If Minimizer has won the game return his/her 
            // evaluated score 
            if (score == -1000) return score;

            if (!isMovesLeft()) return 0;

            // If this maximizer's move 
            if (isMaximizer)
            {
                int best = -1000;

            }


                return 0;
        }



        static bool PlacePiece(int columnIndex, int[,]board, int player) //columnIndex should be between 0 and 6
        {
            columnIndex -= 1; //player inputs a column from 1-7, but in array we need to subtract 1 to index
            if (columnIndex < 0 || columnIndex > board.GetLength(0)) return false;

            for (int row=board.GetLength(0)-1;row>=0;row--)
            {
                if (board[row, columnIndex] == 0)
                {
                    board[row,columnIndex] = player;
                    return true;
                }
            }



            return true;
        }

        static void PrintBoard(int[,] board)
        {
            for (int row=0;row<board.GetLength(0);row++)
            {
                Console.Write("[");
                for(int col = 0; col < board.GetLength(1); col++)
                {
                    if (col == board.GetLength(1)-1)
                    {
                        Console.WriteLine(board[row, col] + "]");
                        break;
                    }
                    Console.Write(board[row,col] + ", ");
                }
            }

            Console.WriteLine("\n");
        }

        static int Score(int depth)
        {
            if (Win(PLAYER_1)) return 1000 - depth;
            else if (Win(PLAYER_2)) return depth - 1000;
            else
                return 0;

        }

        //https://stackoverflow.com/questions/15457796/four-in-a-row-logic/15457826#15457826
        static int GetWinningMoveCount(int[,] board, int player)
        {
            //There should be 69 possible ways to win on an empty board.
            int winningMoves = 0;
            //1 - look at rows first (24 ways to win)
            for(int row = 0; row < board.GetLength(0); row++)
                for(int col = 0; col < 4; col++) //only need to look at 4 ways to win per  
                    if((board[row,col] == 0 || board[row, col] == player)      &&
                       (board[row, col+1] == 0 || board[row, col+1] == player) &&
                       (board[row, col+2] == 0 || board[row, col+2] == player) &&
                       (board[row, col+3] == 0 || board[row, col+3] == player))    
                        winningMoves++;
                
            //2 - look at vertical (3 * 7 = 21 ways)
            for (int row = 0; row < 3; row++)
                for (int col = 0; col < board.GetLength(1); col++) //only need to look at 4 ways to win per  
                    if ((board[row, col] == 0 || board[row, col] == player)        &&
                       (board[row + 1, col] == 0 || board[row + 1, col] == player) &&
                       (board[row + 1, col] == 0 || board[row + 1, col] == player) &&
                       (board[row + 1, col] == 0 || board[row + 1, col] == player))
                        winningMoves++;

            //3 - look at diagonal right and down (12), right to up (12)
            for(int col = 0; col< 4; col++)
            {
                for(int row = 0; row < 3; row++)
                {
                    if ((board[row,col]==0||board[row,col] == player)       &&
                        (board[row+1, col+1] == 0 || board[row+1, col+1] == player) &&
                        (board[row+2, col+2] == 0 || board[row+2, col+2] == player) &&
                        (board[row+3, col+3] == 0 || board[row+3, col+3] == player))
                        winningMoves++;
                }

                for(int row = 3; row < 6; row++)
                {
                    if ((board[row, col] == 0 || board[row, col] == player) &&
                        (board[row-1, col+1] == 0 || board[row-1, col+1] == player) &&
                        (board[row-2, col+2] == 0 || board[row-2, col+2] == player) &&
                        (board[row-3, col+3] == 0 || board[row-3, col+3] == player))
                        winningMoves++;
                }
            }

            return winningMoves;
        }

        static bool isMovesLeft()
        {
            for (int row = 0; row < Board.GetLength(0); row++)
                for (int col = 0; col < Board.GetLength(1); col++)
                    if (Board[row, col] == 0) return true;

            return false;
        }

        static int Evaluate(int[,] board) //Scoring function - temporary hackjob
        {
            //There should be 69 possible ways to win on an empty board.
            //1 - look at rows first (24 ways to win)
            for (int row = 0; row < Board.GetLength(0); row++)
                for (int col = 0; col < 4; col++) //only need to look at 4 ways to win per  
                    if ((Board[row, col] == Board[row, col + 1]) &&
                       (Board[row, col] == Board[row, col + 2]) &&
                       (Board[row, col] == Board[row, col + 3]))
                            if (Board[row, col] == PLAYER_1)
                                return 1000;
                            else if (Board[row, col] == PLAYER_2)
                                return -1000;

            //2 - look at vertical (3 * 7 = 21 ways)
            for (int row = 0; row < 3; row++)
                for (int col = 0; col < Board.GetLength(1); col++) //only need to look at 4 ways to win per  
                    if ((Board[row, col] == Board[row + 1, col]) &&
                       (Board[row, col] == Board[row + 1, col]) &&
                       (Board[row, col] == Board[row + 1, col]))
                            if (Board[row, col] == PLAYER_1)
                                return 1000;
                            else if (Board[row, col] == PLAYER_2)
                                return -1000;

            //3 - look at diagonal right and down (12), right to up (12)
            for (int col = 0; col < 4; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    if ((Board[row, col] == Board[row + 1, col + 1]) &&
                        (Board[row, col] == Board[row + 2, col + 2]) &&
                        (Board[row, col] == Board[row + 3, col + 3]))
                            if (Board[row, col] == PLAYER_1)
                                return 1000;
                            else if (Board[row, col] == PLAYER_2)
                                return -1000;
                }

                for (int row = 3; row < 6; row++)
                {
                    if ((Board[row, col] == Board[row - 1, col + 1]) &&
                        (Board[row, col] == Board[row - 2, col + 2]) &&
                        (Board[row, col] == Board[row - 3, col + 3]))
                            if (Board[row, col] == PLAYER_1)
                                return 1000;
                            else if (Board[row, col] == PLAYER_2)
                                return -1000;
                }
            }

            return 0; //dumbest ai return 0 if neither has won.
        }



        static bool Win(int player)
        {
            //There should be 69 possible ways to win on an empty board.
            //1 - look at rows first (24 ways to win)
            for (int row = 0; row < Board.GetLength(0); row++)
                for (int col = 0; col < 4; col++) //only need to look at 4 ways to win per  
                    if ((Board[row, col] != 0 && Board[row, col] == player) &&
                       (Board[row, col] == Board[row, col + 1]) &&
                       (Board[row, col] == Board[row, col + 2]) &&
                       (Board[row, col] == Board[row, col + 3]))
                        return true;

            //2 - look at vertical (3 * 7 = 21 ways)
            for (int row = 0; row < 3; row++)
                for (int col = 0; col < Board.GetLength(1); col++) //only need to look at 4 ways to win per  
                    if ((Board[row, col] != 0 && Board[row, col] == player) &&
                       (Board[row, col] == Board[row + 1, col]) &&
                       (Board[row, col] == Board[row + 1, col]) &&
                       (Board[row, col] == Board[row + 1, col]))
                        return true;

            //3 - look at diagonal right and down (12), right to up (12)
            for (int col = 0; col < 4; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    if ((Board[row, col] != 0 && Board[row, col] == player) &&
                        (Board[row, col] == Board[row + 1, col + 1]) &&
                        (Board[row, col] == Board[row + 2, col + 2]) &&
                        (Board[row, col] == Board[row + 3, col + 3]))
                        return true;
                }

                for (int row = 3; row < 6; row++)
                {
                    if ((Board[row, col] != 0 && Board[row, col] == player) &&
                        (Board[row, col] == Board[row - 1, col + 1]) &&
                        (Board[row, col] == Board[row - 2, col + 2]) &&
                        (Board[row, col] == Board[row - 3, col + 3]))
                        return true;
                }
            }

            return false;
        }
    }
}

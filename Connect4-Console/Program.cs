using System;
/**
 * @Description: Ai assignment 5, connect 4 ai implementation using on minimax algorithm
 * 
 * 
 * @Author: Daniel Tian (A00736794)
 * @Date:   November 21, 2018
 * 
 * */

namespace Connect4_Console
{
    class Program
    {
        static int[,] Board = new int[6,7];
        static readonly int MAXIMIZER = 1;
        static readonly int MINIMIZER = 2;
        static int MaxDepth = 5;

        static void Main(string[] args)
        {
            //Console.WriteLine("\nWinning moves: " + GetWinningMoveCount(Board, PLAYER_2));
            int input = 0;

            while (true)
            {
                Console.WriteLine("Player turn: ");
                if (int.TryParse(Console.ReadLine(), out input))
                {
                    if (!PlacePiece(input - 1, Board, MINIMIZER)) //player inputs a column from 1-7, but in array we need to subtract 1 to index
                    {
                        Console.WriteLine("Please enter a column between  1-7, as well as unoccupied");
                        continue;
                    }

                    PrintBoard(Board);

                    if (Win(MINIMIZER))
                    {
                        Console.WriteLine("Player has won!");
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a valid integer between 1 and 7");
                    continue;
                }

                Console.WriteLine("AI turn: ");
                int colIndex = FindBestMove(Board, MaxDepth);

                PlacePiece(colIndex, Board, MAXIMIZER);
                PrintBoard(Board);

                if (Win(MAXIMIZER))
                {
                    Console.WriteLine("AI has won!");
                    break;
                }
            }
        }

        static int Minimax(int[,] board, int maxDepth, int currentDepth, bool isMax)
        {
            int score = Evaluate(board);
            if (score == 1000) return score;
            if (score == -1000) return score; //minimizer won
            if (!IsMovesLeft() || currentDepth >= maxDepth) return score; //might be a tie? 
            
            if (isMax)
            {
                int best = int.MinValue;
                //traverse all cells
                for (int col = 0; col < Board.GetLength(1); col++)
                    if (Board[0, col] == 0) //check if empty
                    {
                        //make the move
                        int[,] tempBoard = CopyBoard(board);
                        PlacePiece(col, tempBoard, MAXIMIZER);
                        //call minimax recursively and choose the max value
                        int result = Minimax(tempBoard, maxDepth, currentDepth + 1, !isMax);
                        if (result > best) best = result;
                    }
                return best;
            }
            else// If this minimizer's move 
            {
                int best = int.MaxValue;

                //traverse all cells
                for (int col = 0; col < Board.GetLength(1); col++)
                    if (Board[0, col] == 0) //check if empty
                    {
                        int[,] tempBoard = CopyBoard(board); //make the move
                        PlacePiece(col, tempBoard, MINIMIZER);
                        int result = Minimax(tempBoard, maxDepth, currentDepth + 1, !isMax); //call minimax recursively and choose the mininum value
                        if (result < best) best = result;
                    }
                return best;
            }
        }
            
        static int FindBestMove(int[,] board, int maxDepth)
        {
            int bestVal = int.MinValue;
            int bestMoveColumnIndex = 0;

            for(int col = 0; col < board.GetLength(1); col++)
            {
                    
                if (Board[0, col] == 0) //check if board empty
            {
                    //make the move
                    int[,] tempBoard = CopyBoard(board);
                    PlacePiece(col, tempBoard, MAXIMIZER);
                    int moveVal = Minimax(tempBoard, maxDepth, 0, false);
                    if(moveVal > bestVal)
                    {
                        bestMoveColumnIndex = col;
                        bestVal = moveVal;
                    }
                }
            }
            //Console.WriteLine("The value of the best Move is : " + bestVal);
            return bestMoveColumnIndex;
        }

        static int[,] CopyBoard(int[,] board)
        {
            int[,] newArr = new int[6, 7];

            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    newArr[row, col] = board[row, col];
                }
            }

            return newArr;
        }

        static bool PlacePiece(int columnIndex, int[,]board, int player) //columnIndex should be between 0 and 6
        {
            if (columnIndex < 0 || columnIndex > board.GetLength(0)) return false;

            if (board[0, columnIndex] != 0) return false; //check if top spot is ok
           

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

        static char ConvertToPlayer(int player)
        {
            if (player == MAXIMIZER) return 'X';
            if (player == MINIMIZER) return 'O';
            return ' ';
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
            if (Win(MAXIMIZER)) return 1000 - depth;
            else if (Win(MINIMIZER)) return depth - 1000;
            else
                return 0;

        }

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
                       (board[row + 1, col] == 0 || board[row + 2, col] == player) &&
                       (board[row + 1, col] == 0 || board[row + 3, col] == player))
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

        static bool IsMovesLeft()
        {
            for (int row = 0; row < Board.GetLength(0); row++)
                for (int col = 0; col < Board.GetLength(1); col++)
                    if (Board[row, col] == 0) return true;

            return false;
        }

        //https://stackoverflow.com/questions/15457796/four-in-a-row-logic/15457826#15457826
        static int Evaluate(int[,] board) //Scoring function - There should be 69 possible ways to win on an empty board.
        {
            //1 - look at rows first (24 ways to win)
            for (int row = 0; row < board.GetLength(0); row++)
                for (int col = 0; col < 4; col++) //only need to look at 4 ways to win per  
                    if ((board[row, col] == board[row, col + 1]) &&
                       (board[row, col] == board[row, col + 2]) &&
                       (board[row, col] == board[row, col + 3]))
                            if (board[row, col] == MAXIMIZER) return 1000;
                            else if (board[row, col] == MINIMIZER) return -1000;

            //2 - look at vertical (3 * 7 = 21 ways)
            for (int row = 0; row < 3; row++)
                for (int col = 0; col < board.GetLength(1); col++) //only need to look at 4 ways to win per  
                    if ((board[row, col] == board[row + 1, col]) &&
                       (board[row, col] == board[row + 2, col]) &&
                       (board[row, col] == board[row + 3, col]))
                            if (board[row, col] == MAXIMIZER) return 1000;
                            else if (board[row, col] == MINIMIZER) return -1000;

            //3 - look at diagonal right and down (12), right to up (12)
            for (int col = 0; col < 4; col++)
            {
                for (int row = 0; row < 3; row++)
                    if ((board[row, col] == board[row + 1, col + 1]) &&
                        (board[row, col] == board[row + 2, col + 2]) &&
                        (board[row, col] == board[row + 3, col + 3]))
                            if (board[row, col] == MAXIMIZER) return 1000;
                            else if (board[row, col] == MINIMIZER) return -1000;

                for (int row = 3; row < 6; row++)
                    if ((board[row, col] == board[row - 1, col + 1]) &&
                        (board[row, col] == board[row - 2, col + 2]) &&
                        (board[row, col] == board[row - 3, col + 3]))
                            if (board[row, col] == MAXIMIZER)
                                return 1000;
                            else if (board[row, col] == MINIMIZER)
                                return -1000;
            }
            return 0; //should be improved
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
                       (Board[row, col] == Board[row + 2, col]) &&
                       (Board[row, col] == Board[row + 3, col]))
                        return true;

            //3 - look at diagonal right and down (12), right to up (12)
            for (int col = 0; col < 4; col++)
            {
                for (int row = 0; row < 3; row++)
                    if ((Board[row, col] != 0 && Board[row, col] == player) &&
                        (Board[row, col] == Board[row + 1, col + 1]) &&
                        (Board[row, col] == Board[row + 2, col + 2]) &&
                        (Board[row, col] == Board[row + 3, col + 3]))
                        return true;
                
                for (int row = 3; row < 6; row++)
                    if ((Board[row, col] != 0 && Board[row, col] == player) &&
                        (Board[row, col] == Board[row - 1, col + 1]) &&
                        (Board[row, col] == Board[row - 2, col + 2]) &&
                        (Board[row, col] == Board[row - 3, col + 3]))
                        return true;
            }
            return false;
        }
    }
}

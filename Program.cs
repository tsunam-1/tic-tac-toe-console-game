using System.Diagnostics.Metrics;

namespace TicTacToe
{
    internal class Program
    {
        // Created by: Michael Ha
        // Note: x ~ row, y ~ column
        static void Main(string[] args)
        {
            int boardSize;
            int squaresInBoard;
            int playAgain = 0;
            
            while (playAgain == 0)
            {
                Console.WriteLine("******************************");
                Console.WriteLine("Tic Tac Toe game against an AI");
                Console.WriteLine("******************************");
                Console.WriteLine();
                Console.WriteLine("What is your name?");
                Console.WriteLine("\nHello {0} !! How large of a board would you like to play on today?", Console.ReadLine());
                while (!int.TryParse(Console.ReadLine(), out boardSize) || boardSize < 3)
                {
                    Console.WriteLine("Invalid input. Please enter a board size equal or larger than 3.");
                }
                squaresInBoard = boardSize * boardSize;
                Console.WriteLine();
                Console.WriteLine("Board generated !!! Are you ready to play ?");
                Console.WriteLine("You are the x and the AI is the o.");
                if (boardSize == 3)
                {
                    Console.WriteLine("Be the first to get 3 marks in a row to win.");
                }
                else
                {
                    Console.WriteLine("Be the first to get 4 marks in a row to win.");
                }
                char[,] board = createBoard(boardSize);
                displayBoard(board);

                char result = ' ';
                while (squaresInBoard > 0 && result.Equals(' '))
                {
                    Console.WriteLine("************************************************************************");
                    Console.WriteLine();
                    Console.WriteLine("Please enter your move.");
                    getUserMove(board);
                    displayBoard(board);
                    result = checkForWinner(board);
                    squaresInBoard--;

                    if (squaresInBoard > 0 && result.Equals(' '))
                    {
                        Console.WriteLine("************************************************************************");
                        Console.WriteLine();
                        Console.WriteLine("It's the AI's turn:");
                        getAIMove(board);
                        displayBoard(board);
                        result = checkForWinner(board);
                        squaresInBoard--;
                    }
                }

                if (result.Equals(' '))
                {
                    Console.WriteLine("DRAW");
                }
                else
                {
                    Console.WriteLine("WINNER: {0}", result);
                }
                Console.WriteLine();
                Console.WriteLine("Enter 0 if you want to play again or enter 1 if you want to leave.");
                while(!int.TryParse(Console.ReadLine(), out playAgain) || (playAgain != 0 && playAgain != 1))
                {
                    Console.WriteLine("Invalid input. Please enter either 0 (play again) or 1 (leave).");
                }
                Console.WriteLine();
            }
        } 
        public static char[,] createBoard(int n)
        {
            char[,] emptyBoard = new char[n, n];
            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    emptyBoard[x, y] = ' ';
                }
            }
            return emptyBoard;
        }
        public static void displayBoard(char[,] board)
        {
            int displaySize = 1 + 2 * board.GetLength(0);
            Console.WriteLine();
            for (int x = 0; x < displaySize; x++)
            {
                for (int y = 0; y < displaySize; y++)
                {
                    if (x % 2 == 0) // If the row is odd
                    {
                        if (y % 2 == 0) // If the column is odd
                        {
                            Console.Write("+");
                        }
                        else // If the column is even
                        {
                            Console.Write("-");
                        }
                    }
                    else // If the row is even
                    {
                        if (y % 2 == 0) // If the column is odd
                        {
                            Console.Write("|");
                        }
                        else // If the column is even
                        {
                            Console.Write(board[x / 2, y / 2]);
                        }
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public static bool writeOnBoard(char[,] board, char letter, int x, int y)
        {
            if (x >= board.GetLength(0) || y >= board.GetLength(1))
            {
                Console.WriteLine("({0}, {1}) is an out of bound exception.", x + 1, y + 1);
                return false;
            }
            else if (!board[x, y].Equals(' '))
            {
                Console.WriteLine("({0}, {1}) is already taken by {2}.", x + 1, y + 1, board[x, y]);
                return false;
            }
            else
            {
                board[x, y] = letter;
                return true;
            }
        }
        public static void getUserMove(char[,] board)
        {
            int x;
            int y;
            
            Console.WriteLine();
            Console.WriteLine("What row ('1' represents the first row)?");
            while (!int.TryParse(Console.ReadLine(), out x) || x < 1)
            {
                Console.WriteLine("Invalid input. Please enter a positive integer larger than 0.");
            }
            Console.WriteLine("What column ('1' represents the first column)?");
            while (!int.TryParse(Console.ReadLine(), out y) || y < 1)
            {
                Console.WriteLine("Invalid input. Please enter a positive integer larger than 0.");
            }
            while (!writeOnBoard(board, 'x', x - 1, y - 1))
            {
                Console.WriteLine();
                Console.WriteLine("What row?");
                while (!int.TryParse(Console.ReadLine(), out x) || x < 1)
                {
                    Console.WriteLine("Invalid input. Please enter a positive integer larger than 0.");
                }
                Console.WriteLine("What column?");
                while (!int.TryParse(Console.ReadLine(), out y) || x < 1)
                {
                    Console.WriteLine("Invalid input. Please enter a positive integer larger than 0.");
                }
            }
        }
        // I didn't bother doing a method for boards larger than 3x3 (requiring 4 marks in a row to win) because there are too many combinations to cover through bruteforcing.
        // However, this method still somewhat functions for 4x4 and larger because attempting to block off 4 marks in a row can be done by blocking off the player from doing 3 marks in a row.
        // The same goes for the AI to win (although it's very unlikely).
        public static bool checkForObviousMove(char[,] board)
        {
            // Written in order of priority
            if (checkRows('o', board) || checkColumns('o', board) || checkDiagonals('o', board) || checkRows('x', board) || checkColumns('x', board) || checkDiagonals('x', board))
            {
                Console.WriteLine("The AI is thinking very hard...");
                Thread.Sleep(2000);
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool checkRows(char letter, char[,] board)
        {
            int boardSize = board.GetLength(0);
            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize - 2; y++)
                {
                    if (board[x, y].Equals(' ') && board[x, y + 1].Equals(letter) && board[x, y + 2].Equals(letter))
                    {
                        writeOnBoard(board, 'o', x, y);
                        return true;
                    }
                    else if (board[x, y].Equals(letter) && board[x, y + 1].Equals(' ') && board[x, y + 2].Equals(letter))
                    {
                        writeOnBoard(board, 'o', x, y + 1);
                        return true;
                    }
                    else if (board[x, y].Equals(letter) && board[x, y + 1].Equals(letter) && board[x, y + 2].Equals(' '))
                    {
                        writeOnBoard(board, 'o', x, y + 2);
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool checkColumns(char letter, char[,] board)
        {
            int boardSize = board.GetLength(0);
            for (int y = 0; y < boardSize; y++)
            {
                for (int x = 0; x < boardSize - 2; x++)
                {
                    if (board[x, y].Equals(' ') && board[x + 1, y].Equals(letter) && board[x + 2, y].Equals(letter))
                    {
                        writeOnBoard(board, 'o', x, y);
                        return true;
                    }
                    else if (board[x, y].Equals(letter) && board[x + 1, y].Equals(' ') && board[x + 2, y].Equals(letter))
                    {
                        writeOnBoard(board, 'o', x + 1, y);
                        return true;
                    }
                    else if (board[x, y].Equals(letter) && board[x + 1, y].Equals(letter) && board[x + 2, y].Equals(' '))
                    {
                        writeOnBoard(board, 'o', x + 2, y);
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool checkDiagonals(char letter, char[,] board)
        {
            int boardSize = board.GetLength(0);
            
            for (int x = 0; x < boardSize - 2; x++)
            {
                for (int y = 0; y < boardSize - 2; y++)
                {
                    if (board[x, y].Equals(' ') && board[x + 1, y + 1].Equals(letter) && board[x + 2, y + 2].Equals(letter))
                    {
                        writeOnBoard(board, 'o', x, y);
                        return true;
                    }
                    else if (board[x, y].Equals(letter) && board[x + 1, y + 1].Equals(' ') && board[x + 2, y + 2].Equals(letter))
                    {
                        writeOnBoard(board, 'o', x + 1, y + 1);
                        return true;
                    }
                    else if (board[x, y].Equals(letter) && board[x + 1, y + 1].Equals(letter) && board[x + 2, y + 2].Equals(' '))
                    {
                        writeOnBoard(board, 'o', x + 2, y + 2);
                        return true;
                    }
                    else if (board[x, y + 2].Equals(' ') && board[x + 1, y + 1].Equals(letter) && board[x + 2, y].Equals(letter))
                    {
                        writeOnBoard(board, 'o', x, y + 2);
                        return true;
                    }
                    else if (board[x, y + 2].Equals(letter) && board[x + 1, y + 1].Equals(' ') && board[x + 2, y].Equals(letter))
                    {
                        writeOnBoard(board, 'o', x + 1, y + 1);
                        return true;
                    }
                    else if (board[x, y + 2].Equals(letter) && board[x + 1, y + 1].Equals(letter) && board[x + 2, y].Equals(' '))
                    {
                        writeOnBoard(board, 'o', x + 2, y);
                        return true;
                    }
                }
            }
            return false;
        }
        public static void getAIMove(char[,] board)
        {
            if (!checkForObviousMove(board))
            {
                Random random = new Random();
                int boardSize = board.GetLength(0);
                int randomRow = random.Next(0, boardSize);
                int randomColumn = random.Next(0, boardSize);

                while (!writeOnBoard(board, 'o', randomRow, randomColumn))
                {
                    randomRow = random.Next(0, boardSize);
                    randomColumn = random.Next(0, boardSize);
                }
            }
        }
        public static char checkForWinner(char[,] board)
        {
            int boardSize = board.GetLength(0);

            if (boardSize == 3) // Requires 3 in a row to win
            {
                for (int x = 0; x < boardSize; x++) // Check rows
                {
                    if (board[x, 0].Equals('x') && board[x, 1].Equals('x') && board[x, 2].Equals('x'))
                    {
                        return 'x';
                    }
                    else if (board[x, 0].Equals('o') && board[x, 1].Equals('o') && board[x, 2].Equals('o'))
                    {
                        return 'o';
                    }
                }
                for (int y = 0; y < boardSize; y++) // Check columns
                {
                    if (board[0, y].Equals('x') && board[1, y].Equals('x') && board[2, y].Equals('x'))
                    {
                        return 'x';
                    }
                    else if (board[0, y].Equals('o') && board[1, y].Equals('o') && board[2, y].Equals('o'))
                    {
                        return 'o';
                    }
                }
                if (board[0, 0].Equals('x') && board[1, 1].Equals('x') && board[2, 2].Equals('x')) // Check diagonals
                {
                    return 'x';
                }
                else if (board[0, 0].Equals('o') && board[1, 1].Equals('o') && board[2, 2].Equals('o'))
                {
                    return 'o';
                }
                else if (board[0, 2].Equals('x') && board[1, 1].Equals('x') && board[2, 0].Equals('x'))
                {
                    return 'x';
                }
                else if (board[0, 2].Equals('o') && board[1, 1].Equals('o') && board[2, 0].Equals('o'))
                {
                    return 'o';
                }
            }
            else // Requires 4 in a row to win (if it was 3 to win for any board size larger than 3x3, the game would be entirely dependant on who starts first)
            {
                for (int x = 0; x < boardSize; x++) // Checks rows
                {
                    for (int y = 0; y < boardSize - 3; y++)
                    {
                        if (board[x, y].Equals('x') && board[x, y + 1].Equals('x') && board[x, y + 2].Equals('x') && board[x, y + 3].Equals('x'))
                        {
                            return 'x';
                        }
                        else if (board[x, y].Equals('o') && board[x, y + 1].Equals('o') && board[x, y + 2].Equals('o') && board[x, y + 3].Equals('o'))
                        {
                            return 'o';
                        }
                    }
                }
                for (int y = 0; y < boardSize; y++) // Checks columns
                {
                    for (int x = 0; x < boardSize - 3; x++)
                    {
                        if (board[x, y].Equals('x') && board[x + 1, y].Equals('x') && board[x + 2, y].Equals('x') && board[x + 3, y].Equals('x'))
                        {
                            return 'x';
                        }
                        else if (board[x, y].Equals('o') && board[x + 1, y].Equals('o') && board[x + 2, y].Equals('o') && board[x + 3, y].Equals('o'))
                        {
                            return 'o';
                        }
                    }
                }
                for (int x = 0; x < boardSize - 3; x++) // Checks diagonals
                {
                    for (int y = 0; y < boardSize - 3; y++)
                    {
                        if (board[x, y].Equals('x') && board[x + 1, y + 1].Equals('x') && board[x + 2, y + 2].Equals('x') && board[x + 3, y + 3].Equals('x'))
                        {
                            return 'x';
                        }
                        else if (board[x, y].Equals('o') && board[x + 1, y + 1].Equals('o') && board[x + 2, y + 2].Equals('o') && board[x + 3, y + 3].Equals('o'))
                        {
                            return 'o';
                        }
                    }
                }
            }
            return ' ';
        }
        /* I tried generalizing a method for the AI that would check obvious moves instead of bruteforcing through every possible combinations, but it turned out too long and too complicated.
         * Here are the vestiges:
        public static bool checkForObviousMove2(char[,] board)
        {
            int boardSize = board.GetLength(0);

            for (int x = 0; x < boardSize; x++)
            {
                int xHoriCounter = 0;
                int oHoriCounter = 0;
                int xBlankHoriCounter = 0;
                int oBlankHoriCounter = 0;
                int xVertiCounter = 0;
                int oVertiCounter = 0;
                int xBlankVertiCounter = 0;
                int oBlankVertiCounter = 0;

                for (int y = 0; y < boardSize; y++)
                {
                    if (board[x, y] == 'x')
                    {
                        xHoriCounter++;
                        oHoriCounter = 0;
                        oBlankHoriCounter = 0;
                    }
                    else if (board[x, y] == 'o')
                    {
                        oHoriCounter++;
                        xHoriCounter = 0;
                        xBlankHoriCounter = 0;
                    }
                    else // When the square is blank (' ')
                    {
                        xBlankHoriCounter++;
                        oBlankHoriCounter++;
                    }

                    if (board[y, x] == 'x')
                    {
                        xVertiCounter++;
                        oVertiCounter = 0;
                        oBlankVertiCounter = 0;
                    }
                    else if (board[y, x] == 'o')
                    {
                        oVertiCounter++;
                        xVertiCounter = 0;
                        xBlankVertiCounter = 0;
                    }
                    else // When the square is blank (' ')
                    {
                        xBlankVertiCounter++;
                        oBlankVertiCounter++;
                    }

                    if (boardSize == 3) // Requires 3 marks in a row to win
                    {
                        // Written in order of priority
                        if (oHoriCounter == 2 && oBlankHoriCounter == 1)
                        {
                            writeOnBoard(board, 'o', x, findBlankSquare("horizontal", 2, x, y, board));
                            return true;
                        }
                        else if (oVertiCounter == 2 && oBlankVertiCounter == 1)
                        {
                            writeOnBoard(board, 'o', findBlankSquare("vertical", 2, x, y, board), y);
                            return true;
                        }
                        else if (xVertiCounter == 2 && xBlankVertiCounter == 1)
                        {
                            writeOnBoard(board, 'o', x, findBlankSquare("horizontal", 2, x, y, board));
                            Console.WriteLine("There is xVerti");
                            return true;
                        }
                        else if (xVertiCounter == 2 && xBlankVertiCounter == 1)
                        {
                            writeOnBoard(board, 'o', findBlankSquare("vertical", 2, x, y, board), y);
                            return true;
                        }
                    }
                    else // Requires 4 marks in a row to win
                    {
                        // Written in order of priority
                        if (oHoriCounter == 3 && oBlankHoriCounter == 1)
                        {
                            writeOnBoard(board, 'o', x, findBlankSquare("horizontal", 3, x, y, board));
                            return true;
                        }
                        else if (oVertiCounter == 3 && oBlankVertiCounter == 1)
                        {
                            writeOnBoard(board, 'o', findBlankSquare("vertical", 3, x, y, board), y);
                            return true;
                        }
                        else if (xVertiCounter == 3 && xBlankVertiCounter == 1)
                        {
                            writeOnBoard(board, 'o', x, findBlankSquare("horizontal", 3, x, y, board));
                            return true;
                        }
                        else if (xVertiCounter == 3 && xBlankVertiCounter == 1)
                        {
                            writeOnBoard(board, 'o', findBlankSquare("vertical", 3, x, y, board), y);
                            return true;
                        }
                        else if (oHoriCounter == 2 && oBlankHoriCounter == 2)
                        {
                            writeOnBoard(board, 'o', x, findBlankSquare("horizontal", 2, x, y, board));
                            return true;
                        }
                        else if (oVertiCounter == 2 && oBlankVertiCounter == 2)
                        {
                            writeOnBoard(board, 'o', findBlankSquare("vertical", 2, x, y, board), y);
                            return true;
                        }
                        else if (xVertiCounter == 2 && xBlankVertiCounter == 2)
                        {
                            writeOnBoard(board, 'o', x, findBlankSquare("horizontal", 2, x, y, board));
                            return true;
                        }
                        else if (xVertiCounter == 2 && xBlankVertiCounter == 2)
                        {
                            writeOnBoard(board, 'o', findBlankSquare("vertical", 2, x, y, board), y);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public static int findBlankSquare(string type, int length, int x, int y, char[,] board)
        {
            int boardSize = board.GetLength(0);

            if (type.Equals("horizontal"))
            {
                for (int i = y; i >= y - length; i--)
                {
                    if (board[x, i].Equals(' '))
                    {
                        return i;
                    }
                }
            }
            else // vertical
            {
                for (int i = x; i >= x - length; i--)
                {
                    if (board[i, y].Equals(' '))
                    {
                        return i;
                    }
                }
            }
            return 0;
        }
        */
    }
}

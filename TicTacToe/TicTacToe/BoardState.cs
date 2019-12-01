using System;
using System.Collections.Generic;

namespace TicTacToe
{
    public class BoardState
    {
        public readonly int SIZE = 3;

        public char[,] Board { get; set; }

        public Player Player { get; set; }

        public BoardState()
        {
            Player = new Player();

            Board = new char[SIZE, SIZE];

            for (int row = 0; row < SIZE; row++)
            {
                for (int column = 0; column < SIZE; column++)
                {
                    Board[row, column] = '_';
                }
            }
        }

        //checks if the state is winning one and returns tha score
        public int EvaluateWinningScore(int depth)
        {
            //check if there is a diagonal win
            if ((Board[0, 0] == Board[1, 1] && Board[1, 1] == Board[2, 2]) ||
                (Board[0, 2] == Board[1, 1] && Board[1, 1] == Board[2, 0]))
            {
                if (Board[0, 0] == Player.X)
                {
                    return 10 - depth;
                }
                else if (Board[0, 0] == Player.O)
                {
                    return depth - 10;
                }
            }

            //check if there is a row win
            for (int row = 0; row < SIZE; row++)
            {
                if (Board[row, 0] == Board[row, 1] && Board[row, 1] == Board[row, 2])
                {
                    if (Board[0, 0] == Player.X)
                    {
                        return 10 - depth;
                    }
                    else if (Board[0, 0] == Player.O)
                    {
                        return -10 + depth;
                    }
                }
            }

            //check if there is a column win
            for (int column = 0; column < SIZE; column++)
            {
                if (Board[0, column] == Board[1, column] && Board[1, column] == Board[2, column])
                {
                    if (Board[0, 0] == Player.X)
                    {
                        return 10 - depth;
                    }
                    else if (Board[0, 0] == Player.O)
                    {
                        return depth - 10;
                    }
                }
            }

            //if there is no winning, it is a draw and the score is 0
            return 0;
        }

        public bool IsFull()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int column = 0; column < SIZE; column++)
                {
                    if (IsCellEmpty(row, column))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool IsGameOver()
        {
            if (((Board[0, 0] == Board[1, 1] && Board[1, 1] == Board[2, 2]) ||
                (Board[0, 2] == Board[1, 1] && Board[1, 1] == Board[2, 0])) && (Board[0, 0] != '_'))
            {
                return true;
            }

            for (int row = 0; row < SIZE; row++)
            {
                if ((Board[row, 0] == Board[row, 1] && Board[row, 1] == Board[row, 2]) &&
                    (Board[0, 0] != '_'))
                {
                    return true;
                }
            }

            for (int column = 0; column < SIZE; column++)
            {
                if ((Board[0, column] == Board[1, column] && Board[1, column] == Board[2, column]) &&
                    (Board[0, 0] != '_'))
                {
                    return true;
                }
            }

            if (IsFull())
            {
                return true;
            }

            return false;
        }

        public List<Tuple<int, int>> GetAvailablePositions()
        {
            var availablePositions = new List<Tuple<int, int>>();

            for (int row = 0; row < SIZE; row++)
            {
                for (int column = 0; column < SIZE; column++)
                {
                    if (Board[row, column] == '_')
                    {
                        availablePositions.Add(new Tuple<int, int>(row, column));
                    }
                }
            }

            return availablePositions;
        }

        public bool IsCellEmpty(int row, int column)
        {
            return Board[row, column] == '_';
        }

        public bool IsValidCell(int row, int column)
        {
            return row >= 0 && row <= 2 && column >= 0 && column <= 2;
        }

        public void Print()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int column = 0; column < SIZE; column++)
                {
                    Console.Write($"{Board[row, column]} ");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public BoardState Copy()
        {
            BoardState copy = new BoardState
            {
                Board = new char[SIZE, SIZE]
            };

            for (var row = 0; row < SIZE; row++)
            {
                for (var column = 0; column < SIZE; column++)
                {
                    copy.Board[row, column] = Board[row, column];
                }
            }

            return copy;
        }
    }
}

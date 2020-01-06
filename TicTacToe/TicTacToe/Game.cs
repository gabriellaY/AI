using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    public class Game
    {
        public BoardState TicTacToeBoard { get; set; }

        public Tuple<int, int> MovePosition { get; set; }

        public Tuple<int, int> BestMove;

        public char MaxPlayer { get; set; }

        public char MinPlayer { get; set; }

        public List<char[,]> PossibleMoves { get; set; }

        public Game()
        {
            TicTacToeBoard = new BoardState();
            MovePosition = new Tuple<int, int>(-1, -1);
            PossibleMoves = new List<char[,]>();
        }

        public int Minimax(bool isMax, int depth)
        {
            if (TicTacToeBoard.IsGameOver())
            {
                return TicTacToeBoard.EvaluateWinningScore(depth);
            }

            var Scores = new List<int>();

            var availableMoves = TicTacToeBoard.GetAvailablePositions();

            //Calculate MIN and MAX scores
            if (isMax)
            {
                foreach (var move in availableMoves)
                {
                    // Make a move from the available
                    // user made move, now compo makes
                    TicTacToeBoard.Board[move.Item1, move.Item2] = MaxPlayer;

                    // Add the score of the that board
                    Scores.Add(Minimax(!isMax, depth++));
                    TicTacToeBoard.Board[move.Item1, move.Item2] = TicTacToeBoard.Player.E;
                }

                int maxScore = Scores.Max();

                return maxScore;
            }
            else
            {
                foreach (var move in availableMoves)
                {
                    // Make a move from the available
                    // user made move, now compo makes
                    TicTacToeBoard.Board[move.Item1, move.Item2] = MinPlayer;
                    // Add the score of the that board
                    Scores.Add(Minimax(!isMax, depth++));

                    TicTacToeBoard.Board[move.Item1, move.Item2] = TicTacToeBoard.Player.E;
                }

                int minScore = Scores.Min();

                return minScore;
            }
        }

        public void Play()
        {
            Console.WriteLine("Starting Tic-Tac-Toe game...");
            Console.WriteLine("Choose a position on the board");

            // The scores for each round

            // Hard coded
            MaxPlayer = TicTacToeBoard.Player.O;
            MinPlayer = TicTacToeBoard.Player.X;

            while (!TicTacToeBoard.IsFull())
            {
                var scores = new List<int>();

                string humanMove = Console.ReadLine();
                int row = Convert.ToInt32(humanMove.Split(',')[0]);
                int column = Convert.ToInt32(humanMove.Split(',')[1]);

                if (!TicTacToeBoard.IsValidCell(row, column) || !TicTacToeBoard.IsCellEmpty(row, column))
                {
                    throw new ArgumentOutOfRangeException("Invalid positions.");
                }

                Console.WriteLine("\n");
                TicTacToeBoard.Board[row, column] = TicTacToeBoard.Player.X;
                TicTacToeBoard.Print();

                Console.WriteLine("\n");
                var availableMoves = TicTacToeBoard.GetAvailablePositions();

                foreach (var currMove in availableMoves)
                {
                    TicTacToeBoard.Board[currMove.Item1, currMove.Item2] = MaxPlayer;

                    int minimaxScore = Minimax(false, 0);
                    scores.Add(minimaxScore);

                    TicTacToeBoard.Board[currMove.Item1, currMove.Item2] = TicTacToeBoard.Player.E;
                }
                Console.WriteLine();

                // User moved (min) -> Its computers move (should be max)
                // For each avaiable move, call minimax
                var index = scores.FindIndex(score => score == scores.Max());

                if (index != -1)
                {
                    var move = availableMoves[index];

                    TicTacToeBoard.Board[move.Item1, move.Item2] = TicTacToeBoard.Player.O;
                    TicTacToeBoard.Print();
                }

                string winner = TicTacToeBoard.HasWinner();

                if (winner == "X")
                {
                    Console.WriteLine("You win!");
                    break;
                }
                else if (winner == "O")
                {
                    Console.WriteLine("Computer wins!");
                    break;
                }
                else if (winner == "-")
                {
                    Console.WriteLine("It is a draw game!");
                    break;
                }
            }

        }
    }
}

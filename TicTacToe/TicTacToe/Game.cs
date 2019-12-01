using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    public class Game
    {
        public BoardState TicTacToeBoard { get; set; }
        public int Depth { get; set; }
        public Tuple<int, int> MovePosition { get; set; }
        public List<int> Scores { get; set; }
        public List<Tuple<int, int>> Moves { get; set; }

        List<char[,]> PossibleMoves { get; set; }

        public Game()
        {
            TicTacToeBoard = new BoardState();
            Depth = 0;
            MovePosition = new Tuple<int, int>(-1, -1);
            PossibleMoves = new List<char[,]>();
        }

        public void GenerateNewStates(Tuple<int, int> move)
        {
            TicTacToeBoard.Board[move.Item1, move.Item2] = TicTacToeBoard.Player.X;

            for (int row = 0; row < TicTacToeBoard.SIZE; row++)
            {
                for (int column = 0; column < TicTacToeBoard.SIZE; column++)
                {
                    if (TicTacToeBoard.IsValidCell(row, column) &&
                        TicTacToeBoard.IsCellEmpty(row, column))
                    {
                        var newState = TicTacToeBoard.Copy();

                        newState.Board[row, column] = TicTacToeBoard.Player.O;
                        PossibleMoves.Add(newState.Board);
                    }
                }
            }
        }

        public int Minimax(bool isMax)
        {
            if (TicTacToeBoard.IsGameOver())
            {
                return TicTacToeBoard.EvaluateWinningScore(Depth);
            }

            Depth++;
            Scores = new List<int>();
            Moves = new List<Tuple<int, int>>();

            var availableMoves = TicTacToeBoard.GetAvailablePositions();

            foreach (var move in availableMoves)
            {
                GenerateNewStates(move);
                Scores.Add(Minimax(!isMax));
                Moves.Add(move);
            }

            //Calculate MIN and MAX scores
            if (isMax)
            {
                int maxScore = Scores.Max();
                int maxScoreIndex = Scores.FindIndex(score => score == maxScore);
                var move = Moves[maxScoreIndex];

                //??
                return Scores[maxScoreIndex];
            }
            else
            {
                int minScore = Scores.Max();
                int minScoreIndex = Scores.FindIndex(score => score == minScore);
                var move = Moves[minScoreIndex];

                //??
                return Scores[minScoreIndex];
            }
        }

        public void Play()
        {
            Console.WriteLine("Starting Tic-Tac-Toe game...");
            Console.WriteLine("Choose a position on the board");

            while (!TicTacToeBoard.IsFull())
            {
                string humanMove = Console.ReadLine();
                int row = Convert.ToInt32(humanMove.Split(',')[0]);
                int column = Convert.ToInt32(humanMove.Split(',')[1]);

                if (!TicTacToeBoard.IsValidCell(row, column) || !TicTacToeBoard.IsCellEmpty(row, column))
                {
                    throw new ArgumentOutOfRangeException("Invalid positions.");
                }

                Console.WriteLine("Your move");
                TicTacToeBoard.Board[row, column] = TicTacToeBoard.Player.X;
                TicTacToeBoard.Print();

                Console.WriteLine("Computer's move");

                int minimaxScore = Minimax(false);
                int index = Scores.FindIndex(score => score == minimaxScore);
                var move = Moves[index];
                TicTacToeBoard.Board[move.Item1, move.Item2] = TicTacToeBoard.Player.O;
                TicTacToeBoard.Print();
            }
        }
    }
}

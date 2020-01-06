using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    public class Game
    {
        public BoardState TicTacToeBoard { get; set; }
        //public int Depth { get; set; }
        public Tuple<int, int> MovePosition { get; set; }
        //public List<int> Scores { get; set; }
        //public List<Tuple<int, int>> Moves { get; set; }
        // You need to track who is who
        public Tuple<int, int> BestMove;
        public char MaxPlayer { get; set; }
        public char MinPlayer { get; set; }


        List<char[,]> PossibleMoves { get; set; }

        public Game()
        {
            TicTacToeBoard = new BoardState();
            //Scores = new List<int>();
            //Depth = 0;
            MovePosition = new Tuple<int, int>(-1, -1);
            PossibleMoves = new List<char[,]>();
            //Moves = new List<Tuple<int, int>>();
        }

        // Thinkin about it we dont need this
        //public void GenerateNewStates(Tuple<int, int> move)
        //{
        //    // compo is O
        //    TicTacToeBoard.Board[move.Item1, move.Item2] = TicTacToeBoard.Player.O;

        //    for (int row = 0; row < TicTacToeBoard.SIZE; row++)
        //    {
        //        for (int column = 0; column < TicTacToeBoard.SIZE; column++)
        //        {
        //            if (TicTacToeBoard.IsValidCell(row, column) &&
        //                TicTacToeBoard.IsCellEmpty(row, column))
        //            {
        //                var newState = TicTacToeBoard.Copy();

        //                newState.Board[row, column] = TicTacToeBoard.Player.O;
        //                PossibleMoves.Add(newState.Board);
        //                newState.Board[row, column] = TicTacToeBoard.Player.E;
        //            }
        //        }
        //    }
        //}

        public int Minimax(bool isMax, int depth)
        {
            if (TicTacToeBoard.IsGameOver())
            {
                return TicTacToeBoard.EvaluateWinningScore(depth);
            }

            //Moves = new List<Tuple<int, int>>();
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
                    //GenerateNewStates(move);
                    // Add the score of the that board
                    Scores.Add(Minimax(isMax, depth++));
                    //Moves.Add(move);
                    TicTacToeBoard.Board[move.Item1, move.Item2] = TicTacToeBoard.Player.E;
                }

                int maxScore = Scores.Max();
                //int maxScoreIndex = Scores.FindIndex(score => score == maxScore);



                //??
                return maxScore;
            }
            else
            {
                foreach (var move in availableMoves)
                {
                    // Make a move from the available
                    // user made move, now compo makes
                    TicTacToeBoard.Board[move.Item1, move.Item2] = MinPlayer;
                    //GenerateNewStates(move);
                    // Add the score of the that board
                    Scores.Add(Minimax(!isMax, depth++));
                    //Moves.Add(move);
                    TicTacToeBoard.Board[move.Item1, move.Item2] = TicTacToeBoard.Player.E;
                }

                // tuka beshe         Max be kifte
                int minScore = Scores.Min();
                //int minScoreIndex = Scores.FindIndex(score => score == minScore);
                //BestMove = Moves[minScoreIndex];


                //??
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

                Console.WriteLine("Your move");
                TicTacToeBoard.Board[row, column] = TicTacToeBoard.Player.X;
                TicTacToeBoard.Print();

                Console.WriteLine("Computer's move");
                var availableMoves = TicTacToeBoard.GetAvailablePositions();

                foreach (var currMove in availableMoves)
                {
                    TicTacToeBoard.Board[currMove.Item1, currMove.Item2] = MaxPlayer;
                    int minimaxScore = Minimax(false, 0);                  
                    // Keep the move scores in something
                    scores.Add(minimaxScore);
                    TicTacToeBoard.Board[currMove.Item1, currMove.Item2] = TicTacToeBoard.Player.E;
                }

                // User moved (min) -> Its computers move (should be max)
                // For each avaiable move, call minimax
                var index = scores.FindIndex(score => score == scores.Max());

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

                if (index != -1)
                {
                    var move = availableMoves[index];

                    TicTacToeBoard.Board[move.Item1, move.Item2] = TicTacToeBoard.Player.O;
                    TicTacToeBoard.Print();
                }
                
            }
            Console.WriteLine("It is a draw.");
        }
    }
}

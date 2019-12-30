using System;
using System.Collections.Generic;

namespace SlidingPuzzleIDAStar
{
    public class State
    {
        private int[,] _board = new int[,] { };
        private int[,] _finalState = new int[,] { };
        private int _heuristic;
        private int _weight;

        public int[,] Board { set; get; }

        public int BoardSize { get; set; }

        public int Distance { set; get; }

        public int Heuristic
        {
            get => _heuristic;

            set
            {
                _heuristic = CalculateHeuristic();
            }
        }

        public int Weight
        {
            get => CalculateHeuristic() + Distance;

            set => _weight = value;
        }

        public List<Move> PathDirections { set; get; }


        public State()
        {

        }

        public State(int size, int[,] board)
        {
            BoardSize = size;

            Board = new int[BoardSize, BoardSize];

            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    Board[i, j] = board[i, j];
                }
            }

            PathDirections = new List<Move>();

            InitializeFinalState();
            //_emptyTileIndex = emptyTileIndex;
        }

        public State(State other)
        {
            BoardSize = other.BoardSize;
            Board = other.Board;
            Weight = other.Weight;
            PathDirections = other.PathDirections;
            Distance = other.Distance;
        }

        public Tuple<int, int> FindEmptyTile()
        {
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    if (Board[i, j] == 0)
                    {
                        return new Tuple<int, int>(i, j);
                    }
                }
            }

            return new Tuple<int, int>(-1, -1);
        }

        public Tuple<int, int> FindIndexInFinalState(int tile)
        {
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    if (_finalState[i, j] == tile)
                    {
                        return new Tuple<int, int>(i, j);
                    }
                }
            }

            return new Tuple<int, int>(-1, -1);
        }

        public void InitializeFinalState()
        {
            int tileValue = 1;

            _finalState = new int[BoardSize, BoardSize];

            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    if (i == j && i == BoardSize - 1)
                    {
                        tileValue = 0;
                    }
                    _finalState[i, j] = tileValue++;
                }
            }
        }

        public int CalculateHeuristic()
        {
            int heuristic = 0;

            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    Tuple<int, int> goalPosition = FindIndexInFinalState(Board[i, j]);

                    int manhhattanDistance = Math.Abs(goalPosition.Item1 - i) + Math.Abs(goalPosition.Item2 - j);

                    heuristic += manhhattanDistance;
                }
            }

            return heuristic;
        }

        public void PrintState()
        {
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    Console.Write(_finalState[i, j] + " ");
                }

                Console.WriteLine();
            }
        }

    }
}

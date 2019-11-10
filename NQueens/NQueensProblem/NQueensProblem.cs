using System;
using System.Collections.Generic;
using System.Text;

namespace NQueensProblem
{
    public class NQueensProblem
    {
        private readonly int _boardSize;
        private readonly int _diagonalSize;
        private readonly Random _randomIndex;
        private  int[] _queens; //queens' positions

        private int[] _conflictsInColumns;
        private int[] _conflictsInD1; //primary diagonal
        private int[] _conflictsInD2; //secondary diagonal

        public NQueensProblem(int N)
        {
            this._boardSize = N;
            _diagonalSize = 2 * _boardSize - 1;
            _queens = new int[_boardSize];
            _conflictsInColumns = new int[_boardSize];
            _conflictsInD1 = new int[_diagonalSize];
            _conflictsInD2 = new int[_diagonalSize];
            _randomIndex = new Random();

            GenerateBoard();
        }

        public void SolveProblem()
        {
            int moves = 0;
            int move = 0;
            int countRestarts = 0;

            while (true)
            {
                int maxConflicts = GetQueenWithMaxConflicts();

                if (maxConflicts == -1)
                {
                    Console.WriteLine($"Board size -> {_boardSize}\nMoves -> {moves}\nRestarts of the board -> {countRestarts}");

                    return;
                }

                if (moves == 100)
                {
                    Restart();

                    moves = 0;
                    move = 0;
                    countRestarts++;

                    continue;
                }

                int minConflicts = GetQueenWithMinConflicts(maxConflicts);

                int currentPosition = _queens[maxConflicts];
                int nextPosition = minConflicts;

                if (currentPosition != nextPosition)
                {
                    _queens[maxConflicts] = nextPosition;

                    _conflictsInColumns[currentPosition]--;
                    _conflictsInColumns[nextPosition]++;

                    int currentIndexD1 = GetIndexInD1(maxConflicts, currentPosition);
                    int nextIndexD1 = GetIndexInD1(maxConflicts, nextPosition);

                    _conflictsInD1[currentIndexD1]--;            
                    _conflictsInD1[nextIndexD1]++;
                    
                    _conflictsInD2[maxConflicts + currentPosition]--;
                    _conflictsInD2[maxConflicts + nextPosition ]++;

                    moves++;
                }

                move++;
            }
        }

        public string GetSolution(int rows)
        {
            StringBuilder solutionBoard = new StringBuilder();

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < rows; col++)
                {
                    if (_queens[row] == col)
                    {
                        solutionBoard.Append(col == rows - 1 ? "*\n" : "* ");
                    }
                    else
                    {
                        solutionBoard.Append(col == rows - 1 ? "_\n" : "_ ");
                    }
                }
            }

            return solutionBoard.ToString();
        }

        private int GetIndexInD1(int row, int column)
        {
            int indexInD1;

            if (row <= column)
            {
                indexInD1 = _boardSize - 1 - Math.Abs(row - column);
            }
            else
            {
                indexInD1 = _boardSize - 1 + Math.Abs(row - column);
            }

            return indexInD1;
        }

        //get row with min conflicts
        private int GetQueenWithMinConflicts(int row)
        {
            List<int> minConflictsQueens = new List<int>();

            int minConflicts = _boardSize;

            for (int column = 0; column < _boardSize; column++)
            {
                if (column == _queens[row])
                {
                    continue;
                }

                int numberOfConflicts = CalculateConflictsWithQueens(row, column);

                if (numberOfConflicts < minConflicts)
                {
                    minConflicts = numberOfConflicts;
                    minConflictsQueens.Clear();
                    minConflictsQueens.Add(column);
                }
                else
                {
                    minConflictsQueens.Add(column);
                }
            }

            return minConflictsQueens[_randomIndex.Next(0, minConflictsQueens.Count)];
        }

        //get column with max conflicts
        private int GetQueenWithMaxConflicts()
        {
            List<int> maxConflictsQueens = new List<int>();

            int maxConflicts = 0;

            for (int i = 0; i < _boardSize; i++)
            {
                int numberOfConflicts = CalculateConflictsWithQueens(i, _queens[i]);

                if (numberOfConflicts > maxConflicts)
                {
                    maxConflicts = numberOfConflicts;
                    maxConflictsQueens.Clear();
                    maxConflictsQueens.Add(i);
                }
                else
                {
                    maxConflictsQueens.Add(i);
                }
            }

            if (maxConflicts == 0)
            {
                return -1;
            }
            else
            {
                return maxConflictsQueens[_randomIndex.Next(0, maxConflictsQueens.Count)];
            }
        }

        private void TrackConflicts(int row, int column)
        {
            int indexInD1 = GetIndexInD1(row, column);

            _conflictsInColumns[column]++;
            _conflictsInD1[indexInD1]++;
            _conflictsInD2[row + column]++;
        }

        private int CalculateConflictsWithQueens(int row, int column)
        {
            int indexInD1 = GetIndexInD1(row, column);

            if (_queens[row] == column)
            {
                return _conflictsInD1[indexInD1] + _conflictsInD2[Math.Abs(row + column)] + _conflictsInColumns[column] - 3;
            }
            else
            {
                return _conflictsInD1[indexInD1] + _conflictsInD2[Math.Abs(row + column)] + _conflictsInColumns[column];
            }
        }

        private void Restart()
        {
            _conflictsInColumns = new int[_boardSize];
            _conflictsInD1 = new int[_diagonalSize];
            _conflictsInD2 = new int[_diagonalSize];
            _queens = new int[_boardSize];

            GenerateBoard();
        }

        private void GenerateBoard()
        {
            //for (int i = 0; i < _boardSize; i++)
            //{
            //    _queens[i] = GetQueenWithMinConflicts(i);
            //}

            for (int i = 0; i < _boardSize; i++)
            {
                TrackConflicts(i, _queens[i]);
            }
        }
    }
}

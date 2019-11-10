using System;
using System.Collections.Generic;
using System.Text;

namespace NQueensProblem
{
    public class NQueensProblem
    {
        private readonly int _boardSize;
        private readonly int diagonalSize;
        private readonly Random _randomIndex;
        private readonly int[] _queens; //queens' positions

        private int[] _conflictsInColumns;
        private int[] _conflictsInD1; //primary diagonal
        private int[] _conflictsInD2; //secondary diagonal

        public NQueensProblem(int N)
        {
            this._boardSize = N;
            diagonalSize = 2 * _boardSize - 1;
            _queens = new int[_boardSize];
            _conflictsInColumns = new int[_boardSize];
            _conflictsInD1 = new int[diagonalSize];
            _conflictsInD2 = new int[diagonalSize];
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
                int indexOfQueenWitMaxConflicts = GetQueenWithMaxConflicts();

                if (indexOfQueenWitMaxConflicts == -1)
                {
                    Console.WriteLine($"Moves -> {moves}\nRestarts of the board -> {countRestarts}");

                    return;
                }

                int indexOfQueenWithMinConflicts = GetQueenWithMinConflicts(indexOfQueenWitMaxConflicts);

                int currentPosition = _queens[indexOfQueenWitMaxConflicts];
                int nextPosition = indexOfQueenWithMinConflicts;

                if (currentPosition != nextPosition && nextPosition != -1)
                {
                    _queens[indexOfQueenWitMaxConflicts] = nextPosition;

                    _conflictsInColumns[currentPosition]--;
                    _conflictsInColumns[nextPosition]++;

                    TrackConflicts(currentPosition, indexOfQueenWitMaxConflicts);
                    TrackConflicts(nextPosition, indexOfQueenWitMaxConflicts);

                    moves++;
                }

                move++;

                if (move == 100)
                {
                    Restart();

                    moves = 0;
                    move = 0;
                    countRestarts++;

                    continue;
                }
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
                        solutionBoard.Append(col == rows - 1 ? "*\n" : "*");
                    }
                    else
                    {
                        solutionBoard.Append(col == rows - 1 ? "_\n" : "_");
                    }
                }
            }

            return solutionBoard.ToString();
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

                int numberOfConflicts = _conflictsInColumns[column] 
                    + _conflictsInD1[_queens[row] - column + _boardSize - 1] 
                    + _conflictsInD2[_queens[row] + column];

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
                int numberOfConflicts = _conflictsInColumns[i]
                   + _conflictsInD1[_queens[i] - i + _boardSize - 1]
                   + _conflictsInD2[_queens[i] + i] - 3;

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
            _conflictsInColumns[column]++;
            _conflictsInD1[(row - _queens[row]) + _boardSize - 1]++;
            _conflictsInD2[row + _queens[row]]++;
        }

        //private int CalculateConflictsWithQueens(int row, int column)
        //{
        //    int indexInD1;

        //    if (row <= column)
        //    {
        //        indexInD1 = _boardSize - 1 - Math.Abs(row - column);
        //    }
        //    else
        //    {
        //        indexInD1 = _boardSize - 1 + Math.Abs(row - column);
        //    }

        //    return _conflictsInD1[indexInD1] + _conflictsInD2[Math.Abs(row + column)] + _conflictsInColumns[column];
        //}

        private void Restart()
        {
            _conflictsInColumns = new int[_boardSize];
            _conflictsInD1 = new int[diagonalSize];
            _conflictsInD2 = new int[diagonalSize];

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
                _conflictsInColumns[_queens[i]]++;
                _conflictsInD1[(i - _queens[i]) + _boardSize - 1]++;
                _conflictsInD2[i + _queens[i]]++;

                TrackConflicts(i, _queens[i]);
            }
        }
    }
}

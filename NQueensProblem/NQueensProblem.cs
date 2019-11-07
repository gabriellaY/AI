using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NQueensProblem
{
    public class NQueensProblem
    {
        private readonly int MAX_MOVES;
        private readonly int _boardSize;
        private readonly Random RandomIndex;

        //queens' positions
        public int[] Queens { get; set; }
        public int[] ConflictsInColumns { get; set; }

        //primary diagonal
        public int[] ConflictsInD1 { get; set; }

        //secondary diagonal
        public int[] ConflictsInD2 { get; set; }

        public int Conflicts { get; set; }


        public NQueensProblem(int N)
        {
            this._boardSize = N;
            this.MAX_MOVES = 2 * _boardSize;
            this.Queens = new int[_boardSize];
            this.ConflictsInColumns = new int[_boardSize];
            this.ConflictsInD1 = new int[2 * _boardSize - 1];
            this.ConflictsInD2 = new int[2 * _boardSize - 1];
            this.RandomIndex = new Random();

            for (int i = 0; i < _boardSize; i++)
            {
                ConflictsInColumns[i] = 0;
            }

            for (int i = 0; i < 2 * N - 1; i++)
            {
                ConflictsInD1[i] = 0;
                ConflictsInD2[i] = 0;
            }

            Conflicts = 0;
        }

        public void SolveProblem()
        {
            int moves = 0;
            int move = 0;
            int countRestarts = 0;

            while (true)
            {
                int indexOfQueenWitMaxConflicts = GetQueenWithMaxConflicts();

                if (Conflicts == 0)
                {
                    Console.WriteLine($"Moves -> {moves}\nRestarts of the board: {countRestarts}");

                    return;
                }
           
                int indexOfQueenWithMinConflicts = GetQueenWithMinConflicts(indexOfQueenWitMaxConflicts);

                int currentPosition = Queens[indexOfQueenWitMaxConflicts];
                int nextPosition = Queens[indexOfQueenWithMinConflicts];

                if (currentPosition != nextPosition)
                {
                    Queens[indexOfQueenWitMaxConflicts] = nextPosition;

                    ConflictsInColumns[currentPosition] -= 1;
                    ConflictsInColumns[nextPosition] += 1;

                    TrackConflictsInColumns(currentPosition, indexOfQueenWitMaxConflicts);
                    TrackConflictsInColumns(nextPosition, indexOfQueenWitMaxConflicts);

                    TrackConflictsInD1(currentPosition, indexOfQueenWitMaxConflicts);
                    TrackConflictsInD1(nextPosition, indexOfQueenWitMaxConflicts);

                    TrackConflictsInD2(currentPosition, indexOfQueenWitMaxConflicts);
                    TrackConflictsInD2(nextPosition, indexOfQueenWitMaxConflicts);

                    moves++;
                }

                move++;

                if (move >= MAX_MOVES)
                {
                    GenerateBoard();

                    moves = 0;
                    move = 0;
                    countRestarts++;
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
                    if (Queens[row] == col)
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
                int numberOfConflicts = CalculateConflictsWithQueens(row, column);

                if (numberOfConflicts < minConflicts)
                {
                    minConflicts = numberOfConflicts;
                    minConflictsQueens.Add(column);
                    minConflictsQueens.Clear();
                }
                else
                {
                    minConflictsQueens.Add(column);
                }
            }

            return minConflictsQueens[RandomIndex.Next(0, minConflictsQueens.Count)];
        }

        //get column with max conflicts
        private int GetQueenWithMaxConflicts()
        {
            List<int> maxConflictsQueens = new List<int>();

            int maxConflicts = 0;

            for (int row = 0; row < _boardSize; row++)
            {
                int numberOfConflicts = CalculateConflictsWithQueens(row, Queens[row]);

                if (numberOfConflicts > maxConflicts)
                {
                    maxConflicts = numberOfConflicts;
                    this.Conflicts = maxConflicts;
                    maxConflictsQueens.Clear();
                    maxConflictsQueens.Add(row);
                }
                else
                {
                    maxConflictsQueens.Add(row);
                }
            }

            return maxConflictsQueens[RandomIndex.Next(0, maxConflictsQueens.Count)];
        }


        private void TrackConflictsInColumns(int row, int column)
        {
            ConflictsInColumns[column]++;
        }

        private void TrackConflictsInD1(int row, int column)
        {
            ConflictsInD1[Math.Abs(row - column)]++;
        }

        private void TrackConflictsInD2(int row, int column)
        {
            ConflictsInD2[row + column]++;
        }

        private int CalculateConflictsWithQueens(int row, int column)
        {
            int indexInD1 = Queens[Math.Abs(row - column)];
            int indexInD2 = Queens[Math.Abs(row + column)];

            return ConflictsInColumns[column] + ConflictsInD1[indexInD1] + ConflictsInD2[indexInD2] - 3;
        }

        private void RestartConflicts()
        {
            for (int i = 0; i < 2 * _boardSize - 1; i++)
            {
                if (i < _boardSize)
                {
                    ConflictsInColumns[i] = 0;
                }

                ConflictsInD1[i] = 0;
                ConflictsInD2[i] = 0;
            }
        }

        private void GenerateBoard()
        {
            RestartConflicts();

            for (int i = 0; i < _boardSize; i++)
            {
                Queens[i] = i;
            }

            for (int i = 0; i < _boardSize; i++)
            {
                int j = RandomIndex.Next(_boardSize);
                int rowToSwap = Queens[i];

                Queens[i] = Queens[j];
                Queens[j] = rowToSwap;
            }
        }
    }
}

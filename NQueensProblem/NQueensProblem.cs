using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NQueensProblem
{
    public class NQueensProblem
    {
        private readonly int MAX_MOVES;
        private readonly int _n;

        public int[] Rows { get; set; }
        private Random RandomIndex { get; set; }

        public NQueensProblem(int N)
        {
            this._n = N;
            this.MAX_MOVES = 2 * _n;
            this.Rows = new int[_n];
            this.RandomIndex = new Random();
            this.Scramble();
        }

        public void SolveProblem()
        {
            int moves = 0;
            int move = 0;

            List<int> queensWithMaxConflicts = new List<int>();
            List<int> listOfQueensWithMinConflicts = new List<int>();

            while (move < MAX_MOVES)
            {
                int maxConflicts = 0;
                queensWithMaxConflicts.Clear();

                for (int i = 0; i < _n; i++)
                {
                    int conflicts = GetConflictsWithQueens(Rows[i], i);

                    if (conflicts == maxConflicts)
                    {
                        queensWithMaxConflicts.Add(i);
                    }
                    else if (conflicts > maxConflicts)
                    {
                        maxConflicts = conflicts;
                        queensWithMaxConflicts.Clear();
                        queensWithMaxConflicts.Add(i);
                    }
                }

                if (maxConflicts == 0)
                {
                    return;
                }

                int worst = queensWithMaxConflicts[RandomIndex.Next(queensWithMaxConflicts.Count)];

                int minConflicts = _n;

                listOfQueensWithMinConflicts.Clear();

                for (int i = 0; i < _n; i++)
                {
                    int conflictsCount = GetConflictsWithQueens(i, worst);

                    if (conflictsCount == minConflicts)
                    {
                        queensWithMaxConflicts.Add(i);
                    }
                    else if (conflictsCount < minConflicts)
                    {
                        minConflicts = conflictsCount;
                        queensWithMaxConflicts.Clear();
                        queensWithMaxConflicts.Add(i);
                    }

                    if (listOfQueensWithMinConflicts.Any())
                    {
                        Rows[worst] = listOfQueensWithMinConflicts[RandomIndex.Next(listOfQueensWithMinConflicts.Count)];
                    }

                    move++;
                    moves++;

                    if (moves == MAX_MOVES)
                    {
                        Scramble();
                        moves = 0;
                    }
                }

            }
        }

        public void GetSolution(int rows)
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < rows; col++)
                {
                    // Queen
                    if (Rows[col] == row)
                    {
                        Console.Write("* ");
                    }
                    else
                    {
                        Console.Write("_ ");
                    }
                }
                Console.WriteLine();
            }
        }

        private int GetConflictsWithQueens(int row, int column)
        {
            int conflicts = 0;

            for (int i = 0; i < _n; i++)
            {
                if (i == column)
                {
                    continue;
                }

                if (Rows[i] == column ||
                    Math.Abs(Rows[i] - row) == Math.Abs(i - column))
                {
                    conflicts++;
                }
            }

            return conflicts;
        }

        private void Scramble()
        {
            for (int i = 0; i < _n; i++)
            {
                Rows[i] = i;
            }

            for (int i = 0; i < _n; i++)
            {
                int j = RandomIndex.Next(_n);
                int rowToSwap = Rows[i];

                Rows[i] = Rows[j];
                Rows[j] = rowToSwap;
            }
        }
    }
}

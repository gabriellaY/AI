using System;
using System.Diagnostics;

namespace NQueensProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            int N = 10;

            NQueensProblem queensProblem = new NQueensProblem(N);

            var timer = Stopwatch.StartNew();

            queensProblem.SolveProblem();

            timer.Stop();
            Console.WriteLine(0.001 * timer.ElapsedMilliseconds);

            //var solution = queensProblem.GetSolution(N);
            //Console.WriteLine(solution);
        }
    }
}

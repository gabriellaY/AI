using System;

namespace SlidingPuzzleIDAStar
{
    class Program
    {
        static void Main(string[] args)
        {
            var state = IDAStar.CreateState();

            IDAStar.IDAS(state, state.BoardSize);

            int heuristic = state.CalculateHeuristic();

            Console.WriteLine($"Heuristic -> {heuristic}");
        }
    }
}
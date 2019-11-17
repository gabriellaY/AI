using System;
namespace TravelingSalesmanProblem
{
    public class GeneticAlgorithm
    {
        public void Execute(Population population, int generation, bool isBetterSolution)
        {
            while (true)
            {
                if ((generation == 0 || generation == 10))
                {
                    DisplaySolutions(population, generation);
                }

                if (isBetterSolution && generation != 0 && generation != 10)
                {
                    DisplaySolutions(population, generation);
                }

                isBetterSolution = false;
                double oldFit = population.Fittest;

                population = population.Evolve();

                if (population.Fittest > oldFit)
                {
                    isBetterSolution = true;
                }

                generation++;
            }
        }

        public static void DisplaySolutions(Population population, int generation)
        {
            Path bestSolution = population.FindBest();
            Console.WriteLine($"Generation {generation}\nBest fitness: {bestSolution.Fitness}\nBest distance: {bestSolution.TourDistance}\n");
        }
    }
}

using System;

namespace TravelingSalesmanProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter number of cities");
            string numberOfCities = Console.ReadLine();

            Path destination = Path.Random(Convert.ToInt32(numberOfCities));
            Population population = Population.Randomized(destination, Environment.PopulationSize);

            int generation = 0;
            bool isBetterSolution = true;

            GeneticAlgorithm algorithm = new GeneticAlgorithm();
            algorithm.Execute(population, generation, isBetterSolution);
        }
    }
}

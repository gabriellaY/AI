using System;
using System.Collections.Generic;
using System.Linq;

namespace TravelingSalesmanProblem
{
    public class Population
    {
        private static readonly Random _randomIndex = new Random();

        public List<Path> Cities { get; set; }
        public double Fittest { get; set; }

        public Population(List<Path> cites)
        {
            Cities = cites;
            Fittest = FindFittest();
        }

        public static Population Randomized(Path path, int size)
        {
            List<Path> newTour = new List<Path>();

            for (int i = 0; i < size; i++)
            {
                newTour.Add(path.Shuffle());
            }

            return new Population(newTour);
        }

        public Path Select()
        {
            while (true)
            {
                int i = _randomIndex.Next(0, Environment.PopulationSize);

                if (_randomIndex.NextDouble() < Cities[i].Fitness / Fittest)
                {
                    return new Path(Cities[i].Cities);
                }
            }
        }

        public Population GenerateNewPopulation(int size)
        {
            List<Path> tour = new List<Path>();

            for (int i = 0; i < size; ++i)
            {
                Path p = this.Select().Crossover(this.Select());

                foreach (City c in p.Cities)
                {
                    p = p.Mutate();
                }

                tour.Add(p);
            }

            return new Population(tour);
        }

        public Population Elite(int size)
        {
            List<Path> bestTour = new List<Path>();
            Population tempPopulation = new Population(Cities);

            for (int i = 0; i < size; i++)
            {
                bestTour.Add(tempPopulation.FindBest());
                tempPopulation = new Population(tempPopulation.Cities.Except(bestTour).ToList());
            }

            return new Population(bestTour);
        }

        public Path FindBest()
        {
            foreach (Path p in this.Cities)
            {
                if (p.Fitness == this.Fittest)
                {
                    return p;
                }
            }

            return null;
        }

        public Population Evolve()
        {
            Population best = this.Elite(Environment.Elitism);
            Population newPopulation = this.GenerateNewPopulation(Environment.PopulationSize - Environment.Elitism);

            return new Population(best.Cities.Concat(newPopulation.Cities).ToList());
        }

        private double FindFittest()
        {
            return Cities.Max(t => t.Fitness);
        }
    }
}

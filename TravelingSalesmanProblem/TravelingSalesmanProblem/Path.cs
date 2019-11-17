using System;
using System.Collections.Generic;
using System.Linq;

namespace TravelingSalesmanProblem
{
    public class Path
    {
        private static readonly Random _randomIndex = new Random();

        public List<City> Cities { get; set; }
        public double TourDistance { get; set; }
        public double Fitness { get; set; } //using the total distance for fitness

        public Path(List<City> tour)
        {
            Cities = tour;
            TourDistance = CalculateTotalDistance();
            Fitness = CalculateFitness();
        }

        public Path Crossover(Path parent)
        {
            int i = _randomIndex.Next(0, parent.Cities.Count);
            int j = _randomIndex.Next(i, parent.Cities.Count);

            List<City> selected = Cities.GetRange(i, j - i + 1);
            List<City> rest = parent.Cities.Except(selected).ToList();
            List<City> generation = rest.Take(i)
                                        .Concat(selected)
                                        .Concat(rest.Skip(i))
                                        .ToList();

            return new Path(generation);
        }

        public Path Mutate()
        {
            List<City> mutated = new List<City>(Cities);

            if (_randomIndex.NextDouble() < Environment.MutationRate)
            {
                mutated = Swap(_randomIndex.Next(0, Cities.Count), _randomIndex.Next(0, Cities.Count ));
            }

            return new Path(mutated);
        }


        public Path Shuffle()
        {
            List<City> shuffled = new List<City>(Cities);

            int size = shuffled.Count;
            int index;

            while (size > 1)
            {
                size--;
                index = _randomIndex.Next(size);

                City c = shuffled[index];
                shuffled[index] = shuffled[size];
                shuffled[size] = c;
            }

            return new Path(shuffled);
        }

        public static Path Random(int size)
        {
            List<City> tour = new List<City>();

            for (int i = 0; i < size; ++i)
            {
                tour.Add(City.Random());
            }

            return new Path(tour);
        }

        private List<City> Swap(int i, int j)
        {
            List<City> temp = new List<City>(Cities);

            City city = temp[i];
            temp[i] = temp[j];
            temp[j] = city;

            return temp;
        }

        private double CalculateTotalDistance()
        {
            double totalDistance = 0;

            for (int i = 0; i < Cities.Count; i++)
            {
                totalDistance += Cities[i].GetDistanceTo(Cities[(i + 1) % Cities.Count]);
            }

            return totalDistance;
        }

        private double CalculateFitness()
        {
            return 1 / CalculateTotalDistance();
        }
    }
}

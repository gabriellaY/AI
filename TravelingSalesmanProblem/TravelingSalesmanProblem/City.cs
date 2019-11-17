using System;

namespace TravelingSalesmanProblem
{
    public class City
    {
        private static readonly Random _randomIndex = new Random();

        public int X { get; set; }
        public int Y { get; set; }

        public City(int x, int y)
        {
            X = x;
            Y = y;
        }

        //using the (a^2 + b^2)^(1/2) formula for distance
        public double GetDistanceTo(City city)
        {
            int start = Math.Abs(X - city.X);
            int end = Math.Abs(Y - city.Y);

            return Math.Sqrt(Math.Pow(start, 2) + Math.Pow(end, 2));
        }

        public static City Random()
        {
            return new City(City._randomIndex.Next(), City._randomIndex.Next());
        }
    }
}

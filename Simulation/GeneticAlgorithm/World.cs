using Game.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.GeneticAlgorithm
{
    public class World
    {
        public const int PopulationCount = 100;
        private static Random random = new Random();

        public List<Neighbour> Neighbours { get; set; }

        public World()
        {
            Neighbours = new List<Neighbour>();
        }

        public void Spawn()
        {
            // Generate {PopulationCount} neighbours
            for(int i = 0; i < PopulationCount; i++)
            {
                this.Neighbours.Add(GenerateNeighbour());
            }
        }

        private Neighbour GenerateNeighbour()
        {
            var sequence = Enumerable.Range(0, 10).ToList();

            sequence.Shuffle();

            return new Neighbour(sequence);
        }

        public void DoGeneration()
        {
            var offspring = new List<Neighbour>();

            for(int i = 0; i < PopulationCount; i++)
            {
                // Get parents
                var mother = GetParent();
                var father = GetParent();

                while (mother == father)
                {
                    father = GetParent();
                }

                // Perform Crossover

                // Mutate
            }
        }

        private Neighbour GetParent()
        {
            if (random.NextDouble() > 0.5)
            {
                //Tournament
                return TournamentSelection();
            }
            else
            {
                //Biased random
                return BiasedRandomSelection();
            }
        }

        private Neighbour TournamentSelection()
        {
            var candidate1 = Neighbours[random.Next(PopulationCount)];
            var candidate2 = Neighbours[random.Next(PopulationCount)];

            while(candidate1 == candidate2)
            {
                candidate2 = Neighbours[random.Next(PopulationCount)];
            }

            if (candidate1.GetFitness() > candidate2.GetFitness())
            {
                return candidate1;
            }
            else
            {
                return candidate2;
            }
        }

        private Neighbour BiasedRandomSelection()
        {
            var sum = Neighbours.Sum(n => n.GetFitness());
            var proportions = Neighbours.Select(n => sum / n.GetFitness());
            var proportionSum = proportions.Sum();
            var normalizedProportions = proportions.Select(p => p / proportionSum);

            var cumulativeProportions = new List<double>();

            var cumulativeTotal = 0.0;

            foreach(var proportion in normalizedProportions)
            {
                cumulativeTotal += proportion;
                cumulativeProportions.Add(cumulativeTotal);
            }

            var selectedValue = random.NextDouble();

            for (int i = 0; i < cumulativeProportions.Count(); i++)
            {
                var value = cumulativeProportions[i];

                if(value >= selectedValue)
                {
                    return Neighbours[i];
                }
            }

            throw new Exception("Oh no what happened here!!!");
        }

        public Neighbour GetBestNeighbour()
        {
            return Neighbours.OrderBy(n => n.GetFitness()).FirstOrDefault();
        }
    }
}

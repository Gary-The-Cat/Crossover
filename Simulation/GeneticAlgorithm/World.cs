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
            // Generate a list of numbers [0, 1, 2, 3... 9]
            var sequence = Enumerable.Range(0, 10).ToList();

            // Randomly shuffle the list [3, 1, 5, 9... 4]
            sequence.Shuffle();

            // Create a new neighbour with our random sequence
            return new Neighbour(sequence);
        }

        public void DoGeneration()
        {
            // Create a list to hold our new offspring
            var offspring = new List<Neighbour>();

            // While our offspring are less than our current population count, create new offspring
            // :TODO: Once we are generating individuals update to 
            // while (offspring.Count < PopulationCount)
            for (int i = 0; i < PopulationCount; i++)
            {
                // Get parents
                var mother = GetParent();
                var father = GetParent();

                // Handle the case where we have picked the same individual as both parents
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
            // Grab two random individuals from the population
            var candidate1 = Neighbours[random.Next(PopulationCount)];
            var candidate2 = Neighbours[random.Next(PopulationCount)];

            // Ensure that the two individuals are unique
            while(candidate1 == candidate2)
            {
                candidate2 = Neighbours[random.Next(PopulationCount)];
            }

            // Return the individual that has the higher fitness value
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
            // Get the inverse proportion that each individual takes up of the total solution
            // The shorter the path, the larger the value - the fitter that solution is.
            var sum = Neighbours.Sum(n => n.GetFitness());
            var proportions = Neighbours.Select(n => sum / n.GetFitness());

            // Normalize these values to sum to 1
            // This allows us to randomly generate a number between 0-1 and select that individual
            // [0.25, 0.30, 0.45]
            var proportionSum = proportions.Sum();
            var normalizedProportions = proportions.Select(p => p / proportionSum);

            // Create a list to hold our cumulated values
            var cumulativeProportions = new List<double>();
            var cumulativeTotal = 0.0;

            // Populate the cumulated values
            // [0.25, 0.55, 1]
            foreach (var proportion in normalizedProportions)
            {
                cumulativeTotal += proportion;
                cumulativeProportions.Add(cumulativeTotal);
            }

            // Generate a random number between 0 - 1
            // 0.4
            var selectedValue = random.NextDouble();

            // Loop through our cumulative values list until we find a value that is larger than the value we generated.
            // 0.25 < 0.4 - Nope!
            // 0.55 > 0.4 - Great!
            for (int i = 0; i < cumulativeProportions.Count(); i++)
            {
                var value = cumulativeProportions[i];

                if(value >= selectedValue)
                {
                    // Return the neighbour that is at this index.
                    return Neighbours[i];
                }
            }

            // We either generated a number outside of our range or our values didnt sum to 1.
            // Both should be impossible so we hope to never see this.
            throw new Exception("Oh no what happened here!!!");
        }

        public Neighbour GetBestNeighbour()
        {
            return Neighbours.OrderBy(n => n.GetFitness()).First();
        }
    }
}

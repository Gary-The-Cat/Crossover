using Game.DataStructures;
using Game.Helpers;
using SFML.Graphics;
using System.Collections.Generic;

namespace Game.Factories
{
    public static class TownFactory
    {
        public static List<Town> GetTowns()
        {
            var towns = new List<Town>();

            // Load all of our towns
            // Our towns are stored in our resources folder with the names 'Town_1', 'Town_2' etc so this
            // is just a lazy way to load them all without having too much code.
            for(int i = 0; i < TownHelper.TownPositions.Count; i++)
            {
                // Grab the hard coded position
                var townPosition = TownHelper.TownPositions[i];

                // Create a new town that has a single textured quad.
                towns.Add(new Town(townPosition, new Texture($"../../Resources/Town_{i+1}.png")));
            }

            return towns;
        }
    }
}

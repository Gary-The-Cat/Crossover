using Game.GeneticAlgorithm;
using Game.Screens;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace Game
{
    public class Game
    {
        private readonly SimulationScreen screen;
        private readonly RenderWindow window;
        private readonly World world;
        private readonly Clock clock;
        private readonly Clock generationClock;

        public Game()
        {
            // Create the main window
            window = new RenderWindow(new VideoMode(Configuration.Width, Configuration.Height), "Simulation");

            // Set our frame rate to 60fps so the screen is responsive.
            window.SetFramerateLimit(60);

            // Handle window events
            window.Closed += OnClose;
            window.Resized += OnResize;

            // Create a simulation screen. Note that screens can be stacked on top of one another
            // in the screen manager that has been omitted from this specific repository.
            screen = new SimulationScreen(window, Configuration.SinglePlayer);

            // Create our world
            world = new World();

            // Clock to track frame time. If we dont do this, and instead assume we are hitting 60fps, we can get stutters 
            // in our camera movement.
            clock = new Clock();

            // Create a new generation clock to only DoGeneration once per second.
            generationClock = new Clock();
        }

        public void Run()
        {
            world.Spawn();

            generationClock.Restart();

            while (window.IsOpen)
            {
                // Get the amount of time that has passed since we drew the last frame.
                float deltaT = clock.Restart().AsMicroseconds() / 1000000f;

                // Clear the previous frame
                window.Clear(Configuration.Background);
                
                // Process events
                window.DispatchEvents();

                // Draw the paths of the current best individual
                screen.UpdateSequence(world.GetBestNeighbour());

                // Update all the screen components that are unrelated to our sequence.
                screen.Update(deltaT);

                screen.Draw();

                // Update the window
                window.Display();

                // If one second has passed, breed the next generation
                if(generationClock.ElapsedTime.AsSeconds() > 1)
                {
                    // Do one generation of breeding & culling.
                    world.DoGeneration();

                    // Restart our generation timer
                    generationClock.Restart();
                }
            }
        }

        private void OnResize(object sender, SizeEventArgs e)
        {
            var window = (RenderWindow)sender;
            screen.Camera.ScaleToWindow(window.Size.X, window.Size.Y);
        }

        private static void OnClose(object sender, EventArgs e)
        {
            // Close the window when OnClose event is received
            ((RenderWindow)sender).Close();
        }
    }
}
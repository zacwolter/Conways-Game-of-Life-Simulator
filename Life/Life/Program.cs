using System;
using System.Diagnostics;
using System.IO;
using System.IO.Enumeration;
using Display;
using static CLA;
using static FileIO;
using static Simulation;

namespace Life
{
    /// <summary>
    /// Game of Life code - gives users the ability to enter distinct parameters which will affect
    /// how the game plays out. Players need to use the space bar to advance the game if prompted.
    /// </summary>
    /// <author>Zac Wolter</author>
    /// <date>September 2020</date>
    class GameOfLife
    {

        /// <summary>
        /// Main method, which runs the entire program and calls external methods found above and below Main.
        /// </summary>
        /// <author>Zac Wolter</author>
        /// <date>September 2020</date>
        static void Main(string[] args)
        {
            int periodicity = 0;

            // Check for command line arguments...
            Console.WriteLine("Interpreting command line arguments...");
            if (args.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[SUCCESS] No command line arguments found.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("The program will use the following default settings.");

                //Call the WriteValues method with no parameters, hence printing default set values
                WriteFinalValues();
            }
            // If command line arguments exist, perform the for loop to check each value    
            else if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    // Check if the current value contains the dimensions flag 
                    if (args[i].Contains("--dimensions"))
                    {
                        DimensionArgCheck(args);
                    }

                    // Check if the current value contains the survival flag
                    else if (args[i].Contains("--survival"))
                    {
                        SurvivalArgCheck(args);
                    }

                    // Check if the current value contains the survival flag
                    else if (args[i].Contains("--birth"))
                    {
                        BirthArgCheck(args);
                    }

                    //Check for periodic behaviour boolean
                    else if (args[i].Contains("--periodic"))
                    {
                        PerioidicArgCheck();
                    }

                    //Check for Random Factor double
                    else if (args[i].Contains("--random"))
                    {
                        RandomArgCheck(args, i);
                    }

                    // Check for seed argument
                    else if (args[i].Contains("--seed"))
                    {
                        SeedFileCheck(args, i);
                    }

                    // Check for generations argument
                    else if (args[i].Contains("--generations"))
                    {
                        GenerationArgCheck(args, i);
                    }

                    // Check for maximum update rate
                    else if (args[i].Contains("--max-update"))
                    {
                        MaxUpdateArgCheck(args, i);
                    }

                    // Check for step mode value
                    else if (args[i].Contains("--step"))
                    {
                        StepArgCheck();
                    }

                    // Check for neighbourhood value
                    else if (args[i].Contains("--neighbour"))
                    {
                        NeighourhoodCheck(args, i, currentValues.Rows, currentValues.Columns);
                    }

                    // Check for memory value
                    else if (args[i].Contains("--memory"))
                    {
                        MemoryArgCheck(args, i);
                    }

                    // Check for ghost mode value
                    else if (args[i].Contains("--ghost"))
                    {
                        GhostArgCheck();
                    }

                    // Check for output file value
                    else if (args[i].Contains("--output"))
                    {
                        OutputFileCheck(args, i);
                    }
                }

                // Check that the given survival and birth values supplied by the user are less than or equal to the number
                // of neighbourhing cells depending on the neighbourhood type
                CheckSurvivalAndBirthValues(currentValues);

                // Check that the given neighbourhood order supplied by the user is less than or equal to half the shortest
                // grid dimension
                NeighbourhoodOrderCheck(currentValues.NeighbourhoodSize, currentValues.Rows, currentValues.Columns);

                WriteFinalValues();
            }

            CLA paramValues = storedValues();

            // Wait until the user presses the spacebar before beginning the simulation
            Console.WriteLine("Press Spacebar to begin.");
            WaitForSpacebarPress();

            int[,,] currentCells = new int[currentValues.Rows, currentValues.Columns,currentValues.Memory];
            int[,,] newCells = new int[currentValues.Rows, currentValues.Columns, 1];

            // HOW THE NEW 3D ARRAY WILL WORK:
            // EACH CELL: { x, y, z}
            // Where:
            // x = x coordinate
            // y = y coordinate
            // z = generation

            // Seed file initialisation or random seed generation
            if (currentValues.InputFile == "N/A")
            {
                currentCells = GenerateRandomGrid(currentValues.Probability, currentCells, currentValues.Rows,
                    currentValues.Columns);
            } else
            {
                currentCells = CheckAndInitialiseInputFile(currentValues.InputFilePath, currentCells, currentValues.Rows, currentValues.Columns);
            }     

            // Construct grid...
            Grid grid = new Grid(currentValues.Rows, currentValues.Columns);

            // Initialize the grid window (this will resize the window and buffer)
            grid.InitializeWindow();

            // Set the footnote (appears in the bottom left of the screen).
            grid.SetFootnote($"Generation: 0");

            Stopwatch watch = new Stopwatch();

            // Initialise cells
            InitialiseFirstGen(grid, currentCells);

            // Render updates to the console window...
            grid.Render();

            // Integer to keep track of the number of iterations
            int iterations = Convert.ToInt32(currentValues.Generations);
            int passes = 0;

            // Calculate speed of simulation (applies only if step mode is off)
            int updateTime = 1000 / Convert.ToInt32(currentValues.MaxRefreshRate);

            // Run the simulation
            while (passes < iterations)
            {
                watch.Restart();      

                grid.SetFootnote($"Generation: {passes}");
                grid.Render();

                passes++;

                // Check all x layers for any similarities against all other layers
                periodicity = steadyStateCheck(currentCells);

                if (periodicity != -1)
                {
                    steadyStateCompletion(grid, periodicity, currentCells);
                    break;
                }

                // Method that checks every cell in the grid for alive neighbours, decides if each cell is alive or
                // dead in the next generation, and updates the display based on if ghost mode is activated or not
                SimulateNextGen(currentValues.GhostMode, currentCells, grid, newCells);

                // Generates a new array of cells with the previous layers moved forward 1 layer
                // First layer [0] is empty
                currentCells = RotateMemory(currentCells, newCells);

                // Check if step mode is enabled, if so, wait for key press
                if (currentValues.StepMode == true)
                {
                    while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) ;
                }
                else
                {
                    // Update timer if required...
                    while (watch.ElapsedMilliseconds < updateTime) ;
                }
            }

            grid.SetFootnote("Press Space to Exit");

            // Set complete marker as true
            grid.IsComplete = true;

            // Render updates to the console window (grid should now display COMPLETE)...
            grid.Render();

            // Wait for user to press a key...
            while (true)
            {
                var keyPress = Console.ReadKey(true);
                if (keyPress.Key == ConsoleKey.Spacebar)
                {
                    break;
                }
            }

            // Revert grid window size and buffer to normal
            grid.RevertWindow();

            // Tell user that no steady-state was detected (this code will only execute if the number of generations
            // is reached before a steady-state occurs)
            Console.WriteLine("Steady-state not detected...");
            
            if (currentValues.OutputFile != "N/A")
            {
                GenerateOutputFile(currentValues.OutputFile, currentValues.Rows, currentValues.Columns, currentCells);
            }

            Console.WriteLine("Press spacebar to close program...");

            // Wait for user to press spacebar...
            while (true)
            {
                var keyPress = Console.ReadKey(true);
                if (keyPress.Key == ConsoleKey.Spacebar)
                {
                    break;
                }
            }

            // Close the program here
            Environment.Exit(0);
        }
        
        public static void WaitForSpacebarPress()
        {
            while (true)
            {
                var keyPress = Console.ReadKey(true);
                if (keyPress.Key == ConsoleKey.Spacebar)
                {
                    break;
                }
            }

        }

        public static void InitialiseFirstGen(Grid grid, int[,,] currentCells)
        {
            // Double for loop that simply checks whether or not a particular cell is alive or dead in the first
            // generation and then changes it's state to the corresponding full or blank CellState.
            for (int row = 0; row < currentValues.Rows; row++)
            {
                for (int col = 0; col < currentValues.Columns; col++)
                {
                    if (currentCells[row, col, 0] == 1)
                    {
                        grid.UpdateCell(row, col, CellState.Full);
                    }
                    else
                    {
                        grid.UpdateCell(row, col, CellState.Blank);
                    }
                }
            }
        }
    }
}

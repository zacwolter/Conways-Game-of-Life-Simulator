using System;
using Display;
using static CLA;
using System.IO;
using static FileIO;

public class Simulation
{
    public static void steadyStateCompletion(Grid grid, int periodicity, int[,,] currentCells)
    {
        // Set complete marker as true
        grid.IsComplete = true;

        // Render updates to the console window (grid should now display COMPLETE)...
        grid.Render();

        // Wait for user to press spacebar...
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

        // Tell user periodicity amount
        if (periodicity == 1 || periodicity == 0)
        {
            Console.WriteLine("Steady-state detected... periodicity = N/A");
        }
        else if (periodicity > 0)
        {
            Console.WriteLine($"Steady-state detected... periodicity = {periodicity}");
        }
        else
        {
            Console.WriteLine("Steady-state detected... error finding periodicity value");
        }

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

    public static void SimulateNextGen(bool ghostMode, int[,,] currentCells, Grid grid, int[,,] newCells)
    {
        if (ghostMode)
        {
            // Starting from the 4th layer and moving through each layer until the 1st, implement ghost mode

            // 5th layer (Blank state)
            for (int row = 0; row < currentValues.Rows; row++)
            {
                for (int col = 0; col < currentValues.Columns; col++)
                {
                    grid.UpdateCell(row, col, CellState.Blank);
                }
            }

            // 4th layer (Light state)
            for (int row = 0; row < currentValues.Rows; row++)
            {
                for (int col = 0; col < currentValues.Columns; col++)
                {
                    if (currentCells[row, col, 2] == 1)
                    {
                        grid.UpdateCell(row, col, CellState.Light);
                    }
                }
            }

            // 3rd layer (Medium state)
            for (int row = 0; row < currentValues.Rows; row++)
            {
                for (int col = 0; col < currentValues.Columns; col++)
                {
                    if (currentCells[row, col, 1] == 1)
                    {
                        grid.UpdateCell(row, col, CellState.Medium);
                    }
                }
            }

            // 2nd layer (Dark state)
            for (int row = 0; row < currentValues.Rows; row++)
            {
                for (int col = 0; col < currentValues.Columns; col++)
                {
                    if (currentCells[row, col, 0] == 1)
                    {
                        grid.UpdateCell(row, col, CellState.Dark);
                    }
                }
            }

            // 1st layer (Full state)
            for (int row = 0; row < currentValues.Rows; row++)
            {
                for (int col = 0; col < currentValues.Columns; col++)
                {
                    int aliveNeighbours = NeighbourCount(currentCells, currentValues.NeighbourhoodSize, row, col,
                        currentValues.Rows, currentValues.Columns, currentValues.Periodic,
                        currentValues.Neighbourhood, currentValues.CentreCheck);

                    int aliveOrDead = aliveOrDeadCheck(aliveNeighbours, currentValues.SurvivalList,
                        currentValues.BirthList, currentCells, row, col);

                    // Top layer (Full state)
                    if (aliveOrDead == 1)
                    {
                        grid.UpdateCell(row, col, CellState.Full);
                        newCells[row, col, 0] = 1;
                    }
                    else if (aliveOrDead == 0)
                    {
                        newCells[row, col, 0] = 0;
                    }
                }
            }
        }
        else if (!ghostMode)
        {
            for (int row = 0; row < currentValues.Rows; row++)
            {
                for (int col = 0; col < currentValues.Columns; col++)
                {
                    // For each cell, count the alive neighbours using the below method
                    int aliveNeighbours = NeighbourCount(currentCells, currentValues.NeighbourhoodSize, row, col,
                        currentValues.Rows, currentValues.Columns, currentValues.Periodic,
                        currentValues.Neighbourhood, currentValues.CentreCheck);

                    // Then check if the current cell remains survives or is born
                    int aliveOrDead = aliveOrDeadCheck(aliveNeighbours, currentValues.SurvivalList,
                        currentValues.BirthList, currentCells, row, col);

                    // Based on the returned aliveOrDead value, update the grid
                    if (aliveOrDead == 1)
                    {
                        grid.UpdateCell(row, col, CellState.Full);
                        newCells[row, col, 0] = 1;
                    }
                    else if (aliveOrDead == 0)
                    {
                        grid.UpdateCell(row, col, CellState.Blank);
                        newCells[row, col, 0] = 0;
                    }
                    else
                    {
                        throw new Exception("aliveOrDead value NULL. Unsure of why, but check the code in " +
                            "NextGen method");
                    }
                }
            }
        }
        else
        {
            grid.RevertWindow();

            // Tell user that ghostMode value was not detected
            Console.WriteLine("Ghost Mode boolean value not detected.");
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
    }

    public static int[,,] GenerateRandomGrid(double probability, int[,,] currentCells, int numRows, int numCols)
    {
        // Method generates a random number and if the random number is less than probability * 100, then the cell is
        // deemed alive in the grid

        Random random = new Random();

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                if ((random.Next(0, 101)) <= probability * 100)
                {
                    currentCells[row, col, 0] = 1;
                }
            }
        }

        return currentCells;
    }

    public static int NeighbourCount(int[,,] currentCells, int order, int selectedRow, int selectedCol, int numRows, 
        int numColumns, bool periodic, string neighbourhood, bool centreChecking)
    {
        int aliveNeighbours = 0;

        // Both MooreNeighbourhood and VNNeighbourhood below are examples of inheritance and polymorphism, as both
        // classes inherit the NeighbourhoodScanner class properties and methods, however, their methods for checking
        // the number of alive neighbours is slightly different due to different neighbourhood shapes

        if (neighbourhood == "Moore")
        {
            MooreNeighbourhood neighbourhoodScanner = new MooreNeighbourhood(order, selectedRow, selectedCol, numRows,
                numColumns, periodic, centreChecking, neighbourhood);

            aliveNeighbours = neighbourhoodScanner.NeighbourhoodCount(currentCells);
        }
        else if (neighbourhood == "Von Neumann")
        {
            VNNeighbourhood neighbourhoodScanner = new VNNeighbourhood(order, selectedRow, selectedCol, numRows,
                numColumns, periodic, centreChecking, neighbourhood);

            aliveNeighbours = neighbourhoodScanner.NeighbourhoodCount(currentCells);
        }

        return aliveNeighbours;
    }

    public static int aliveOrDeadCheck(int aliveNeighbours, string[] survivalValues, string[] birthValues,
        int[,,] currentCells, int row, int col)
    {
        int aliveOrDead = 0;
        int[] survivalIntValues = new int[survivalValues.Length];
        int[] birthIntValues = new int[birthValues.Length];
        bool survivalCheck = true; // True: target cell is alive already, False: target cell is currently dead.

        if (currentCells[row, col, 0] == 0)
        {
            survivalCheck = false;
        }

        // Convert survival values to an array of integers
        for (int i = 0; i < survivalValues.Length; i++)
        {
            if (survivalValues[i] != "" && survivalValues[i] != null)
            {
                survivalIntValues[i] = Convert.ToInt32(survivalValues[i]);
            }

            if (i == survivalValues.Length - 1 && survivalValues[i] == "")
            {
                survivalIntValues = new int[survivalIntValues.Length - 1];

                for (int j = 0; j < survivalIntValues.Length; j++)
                {
                    survivalIntValues[j] = Convert.ToInt32(survivalValues[j]);
                }
            }
        }

        // Convert birth values to an array of integers
        for (int i = 0; i < birthValues.Length; i++)
        {
            if (birthValues[i] != "" && birthValues[i] != null)
            {
                birthIntValues[i] = Convert.ToInt32(birthValues[i]);
            }
            
            if (i == birthValues.Length - 1 && birthValues[i] == "")
            {
                birthIntValues = new int[birthIntValues.Length - 1];

                for (int j = 0; j < birthIntValues.Length; j++)
                {
                    birthIntValues[j] = Convert.ToInt32(birthValues[j]);
                }
            }
        }

        // Check number of aliveNeighbours against the array of possible survival rates
        if (survivalCheck)
        {
            for (int i = 0; i < survivalIntValues.Length; i++)
            {
                if (aliveNeighbours == survivalIntValues[i])
                {
                    aliveOrDead = 1;
                    break;
                }
                else
                {
                    aliveOrDead = 0;
                }
            }
        }      

        // Check number of aliveNeighbours against the array of possible birth rates
        if (!survivalCheck)
        {
            for (int i = 0; i < birthIntValues.Length; i++)
            {
                if (aliveNeighbours == birthIntValues[i])
                {
                    aliveOrDead = 1;
                    break;
                }
                else
                {
                    aliveOrDead = 0;
                }
            }
        }

        return aliveOrDead;
    }

    public static int[,,] RotateMemory(int[,,] originalCells, int[,,] nextGeneration)
    {
        // Create free space in layer 0, delete last layer, shift all other layers forward 1
        int numRows = originalCells.GetLength(0);
        int numCols = originalCells.GetLength(1);
        int numMemory = originalCells.GetLength(2);

        int[,,] newArray = new int[numRows, numCols, numMemory];

        // First shift everything forward by copying the i'th layer of the original array to the i + 1'th 
        // layer of the new array

        for (int i = 0; i < numMemory - 1; i++)
        {
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    newArray[row, col, i + 1] = originalCells[row, col, i];
                    newArray[row, col, 0] = nextGeneration[row, col, 0];
                }
            }
        }

        return newArray;
    }

    public static int steadyStateCheck(int[,,] currentCells)
    {
        int numRows = currentCells.GetLength(0);
        int numCols = currentCells.GetLength(1);
        int numMemory = currentCells.GetLength(2);
        int numCells = numRows * numCols;
        int periodicity = 0;

        // NOTE: only have to compare the newest layer to everything else to determine steady state

        // Create first comparison array, which will be target array
        int[,] targetArray = new int[numRows, numCols];

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                targetArray[row, col] = currentCells[row, col, 0];
            }
        }
        
        // For loop to compare every other array to the first target array (hence why memory = 1 to start)
        for (int memory = 1; memory < numMemory; memory++)
        {
            int[,] comparisonArray = new int[numRows, numCols];
            periodicity++;

            // Create second comparison array, which will be compared to the target array
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    comparisonArray[row, col] = currentCells[row, col, memory];
                }
            }

            int matchingCells = 0;

            // Count number of matching cells (ideally, if the number of matching cells == total number of cells,
            // then the arrays are equal and a steady-state can be declared)
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    if (targetArray[row, col] == comparisonArray[row, col])
                    {
                        matchingCells++;
                    }
                }
            }

            if (matchingCells == numCells)
            {
                return periodicity;
            }

            // If the steady state check doesn't complete, then double check that the grid isn't empty
            int numZeros = 0;

            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    if (targetArray[row, col] == 0)
                    {
                        numZeros++;
                    }
                }
            }

            if (numZeros == numCells)
            {
                return 0;
            } 
        }

        return -1;
    }
}

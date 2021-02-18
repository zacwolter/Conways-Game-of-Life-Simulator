using System;
using System.IO;

public class FileIO
{
    public static void GenerateOutputFile(string outputFilePath, int numRows, int numCols, int[,,] currentCells)
    {
        // Write the current generaiton (final generation) into a file with a path specified by the user
        // .seed file to be written in version 2.0 style, with only cell structures
        using (StreamWriter writer = File.CreateText($"{outputFilePath}"))
        {
            writer.WriteLine("#version=2.0");

            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    if (currentCells[row, col, 0] == 1)
                    {
                        writer.WriteLine($"(o) cell : {row}, {col}");
                    }
                }
            }

            writer.Close();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[SUCCESS] Final generation written to location: {outputFilePath}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public static int[,,] CheckAndInitialiseInputFile(string inputFilePath, int[,,] cellsArray, int rows, int columns)
    {
        if (inputFilePath != "N/A" && File.Exists(inputFilePath))
        {
            // Extra layer of checking if a file exists (the input seed file is checked during the CLA processing)

            string fileDir = inputFilePath;
            bool exceedsDimensions = false;
            string[] fields;
            const string DELIM = " ";

            try
            {
                using (StreamReader reader = File.OpenText(inputFilePath))
                {
                    string line = "";
                    line = reader.ReadLine();
                    if (line.Contains("1.0"))
                    {
                        // Version 1.0 SEED file processing

                        while ((line = reader.ReadLine()) != null)
                        {
                            // Split the seed line into an array of values
                            fields = line.Split(DELIM);
                            int selectedRowRef = Convert.ToInt32(fields[0]);
                            int selectedColRef = Convert.ToInt32(fields[1]);

                            // Check if any values exceed the dimensions of the grid
                            if (selectedRowRef > rows || selectedColRef > columns)
                            {
                                exceedsDimensions = true;
                            }

                            // Try to set the give cell's value to 1 in the grid
                            try
                            {
                                cellsArray[Convert.ToInt32(fields[0]), Convert.ToInt32(fields[1]), 0] = 1;
                            }
                            catch (Exception)
                            {
                                exceedsDimensions = true;
                            }
                        }

                        if (exceedsDimensions)
                        {
                            throw new SeedValueExceedsDimensionsException();
                        }
                    }
                    else if (line.Contains("2.0"))
                    {
                        // Version 2.0 SEED file processing

                        while ((line = reader.ReadLine()) != null)
                        {
                            // Split the seed line into an array of values
                            fields = line.Split(DELIM);
                            int alive = 1;
                            string structureType = "";
                            int x1 = 0, y1 = 0, x2 = 0, y2 = 0;

                            // Alive or dead status checking
                            if (fields[0].Contains("o"))
                            {
                                alive = 1;
                            }
                            else if (fields[0].Contains("x"))
                            {
                                alive = 0;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("SEED file contains unknown alive or dead status.");
                            }

                            try
                            {
                                // Check the cell structure (cell, rectangle or ellipse)
                                if (fields[1].Contains("cell"))
                                {
                                    x1 = Convert.ToInt32(fields[fields.Length - 2].Trim(','));
                                    y1 = Convert.ToInt32(fields[fields.Length - 1]);

                                    if (x1 > rows || y1 > columns)
                                    {
                                        exceedsDimensions = true;
                                    }
                                    else
                                    {
                                        cellsArray[x1, y1, 0] = 1;
                                    }
                                }
                                else if (fields[1].Contains("rectangle"))
                                {
                                    // Convert all four row and column values for use
                                    // Accounts for both situations where the ":" may be spaced apart from the
                                    // structure keyword and if it isn't
                                    if (fields.Length == 6)
                                    {
                                        x1 = Convert.ToInt32(fields[2].Trim(','));
                                        y1 = Convert.ToInt32(fields[3].Trim(','));
                                        x2 = Convert.ToInt32(fields[4].Trim(','));
                                        y2 = Convert.ToInt32(fields[5].Trim(','));
                                    }
                                    else
                                    {
                                       
                                        x1 = Convert.ToInt32(fields[3].Trim(','));
                                        y1 = Convert.ToInt32(fields[4].Trim(','));
                                        x2 = Convert.ToInt32(fields[5].Trim(','));
                                        y2 = Convert.ToInt32(fields[6].Trim(','));
                                    } 

                                    // Check if any values exceed the row or column dimensions
                                    if (x1 > rows || x2 > rows || y1 > columns || y2 > columns)
                                    {
                                        exceedsDimensions = true;
                                    }
                                    else
                                    {
                                        // If the values don't exceed the row or column dimensions, 
                                        // convert cells in grid to form rectangular shape
                                        for (int i = x1; i <= x2; i++)
                                        {
                                            for (int j = y1; j <= y2; j++)
                                            {
                                                cellsArray[i, j, 0] = alive;
                                            }
                                        }
                                    }
                                }
                                else if (fields[1].Contains("ellipse"))
                                {
                                    double ellipseX1, ellipseX2, ellipseY1, ellipseY2;

                                    // Convert all four row and column values for use
                                    // Accounts for both situations where the ":" may be spaced apart from the
                                    // structure keyword and if it isn't
                                    if (fields.Length == 6)
                                    {
                                        ellipseX1 = Convert.ToDouble(fields[2].Trim(','));
                                        ellipseY1 = Convert.ToDouble(fields[3].Trim(','));
                                        ellipseX2 = Convert.ToDouble(fields[4].Trim(','));
                                        ellipseY2 = Convert.ToDouble(fields[5].Trim(','));
                                    }
                                    else if (fields.Length == 7)
                                    {
                                        ellipseX1 = Convert.ToDouble(fields[3].Trim(','));
                                        ellipseY1 = Convert.ToDouble(fields[4].Trim(','));
                                        ellipseX2 = Convert.ToDouble(fields[5].Trim(','));
                                        ellipseY2 = Convert.ToDouble(fields[6].Trim(','));
                                    }
                                    else
                                    {
                                        throw new Exception();
                                    }

                                    // Check if any values exceed the row or column dimensions
                                    if (ellipseX1 > rows || ellipseX2 > rows || ellipseY1 > columns || ellipseY2 > columns)
                                    {
                                        exceedsDimensions = true;
                                    }
                                    else
                                    {
                                        // If the values don't exceed the row or column dimensions, 
                                        // convert cells in grid to form ellipse shape
                                        double centerX = (ellipseX1 + ellipseX2) / 2;
                                        double centerY = (ellipseY1 + ellipseY2) / 2;

                                        for (double y = ellipseY1; y <= ellipseY2; y++)
                                        {
                                            for (double x = ellipseX1; x <= ellipseX2; x++)
                                            {
                                                int ellipseX = Convert.ToInt32(x);
                                                int ellipseY = Convert.ToInt32(y);
                                                int dx = Convert.ToInt32(ellipseX2 - ellipseX1 + 1);
                                                int dy = Convert.ToInt32(ellipseY2 - ellipseY1 + 1);

                                                if ((4 * Math.Pow(x - centerX, 2) / Math.Pow(dx, 2)) +
                                                   (4 * Math.Pow(y - centerY, 2) / Math.Pow(dy, 2)) <= 1)
                                                {
                                                    cellsArray[ellipseX, ellipseY, 0] = alive;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                exceedsDimensions = true;
                            }
                        }

                        if (exceedsDimensions)
                        {
                            throw new SeedValueExceedsDimensionsException();
                        }
                    }
                    else
                    {
                        // Unable to determine whether seed file is version 1.0 or 2.0
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[ERROR] Unknown version of SEED file.");
                    }
                }
            }
            catch (SeedValueExceedsDimensionsException)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\n[WARNING] One or more row or column values exceed dimensions " +
                    "specified.\n");
                Console.WriteLine("Please Note: Continuing with the simulation will result in the program\n" +
                    "ignoring the cells or structures outside the dimensions.\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press Spacebar to begin or Escape to quit.\n");

                // Checks whether the user presses either spacebar or escape
                while (true)
                {
                    var keyPress = Console.ReadKey(true);
                    if (keyPress.Key == ConsoleKey.Spacebar)
                    {
                        break;
                    }
                    if (keyPress.Key == ConsoleKey.Escape)
                    {
                        System.Environment.Exit(0);
                    }
                }
            }

            return cellsArray;
        }
        else
        {
            return cellsArray;
        }
    }
}

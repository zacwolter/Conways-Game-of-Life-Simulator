using System;
using System.IO;

public class CLA
{
    // List of stored private values within the CLA class
    private int rows = 16, columns = 16, generations = 50, maxRate = 5, totalCells, memory = 16, neighbourhoodSize = 1;
    private bool periodic = false, stepMode = false, ghostMode = false, centreCheck = false;
    private double probability = 0.5;
    private string inputFile = "N/A", inputFilePath = "N/A", outputFile = "N/A";
    private string neighbourhood = "Moore";
    private string survival = "2...3", birth = "3";
    private string[] survivalList = { "2", "3" }, birthList = { "3" };


    public static CLA currentValues = new CLA();

    public int Rows
    {
        get { return rows; }
        set 
        {
            rows = value;
            totalCells = rows * columns;
        }
    }
    public int Columns
    {
        get { return columns; }
        set 
        { 
            columns = value;
            totalCells = rows * columns;
        }
    }
    public int TotalCells
    {
        get { return totalCells; }
    }
    public int Generations
    {
        get { return generations; }
        set { generations = value; }
    }
    public int MaxRefreshRate
    {
        get { return maxRate; }
        set { maxRate = value; }
    }
    public bool Periodic
    {
        get { return periodic; }
        set { periodic = value; }
    }
    public bool StepMode
    {
        get { return stepMode; }
        set { stepMode = value; }
    }
    public double Probability
    {
        get { return probability; }
        set { probability = value; }
    }
    public string InputFile
    {
        get { return inputFile; }
        set { inputFile = value; }
    }
    public string OutputFile
    {
        get { return outputFile; }
        set { outputFile = value; }
    }
    public string Survival
    {
        get { return survival; }
        set { survival = value; }
    }
    public string Birth
    {
        get { return birth; }
        set { birth = value; }
    }
    public string Neighbourhood
    {
        get { return neighbourhood; }
        set { neighbourhood = value; }
    }
    public int Memory
    {
        get { return memory; }
        set { memory = value; }
    }
    public bool GhostMode
    {
        get { return ghostMode; }
        set { ghostMode = value; }
    }
    public int NeighbourhoodSize
    {
        get { return neighbourhoodSize; }
        set { neighbourhoodSize = value; }
    }
    public string InputFilePath
    {
        get { return inputFilePath; }
        set { inputFilePath = value; }
    }
    public string[] SurvivalList
    {
        get { return survivalList; }
        set { survivalList = value; }
    }
    public string[] BirthList
    {
        get { return birthList; }
        set { birthList = value; }
    }
    public bool CentreCheck
    {
        get { return centreCheck; }
        set { centreCheck = value; }
    }


    public static CLA storedValues()
    {
        return currentValues;
    }

    public static void WriteFinalValues()
    {
        try
        {
            if (currentValues.InputFilePath != "N/A")
            {
                FileInfo seedInfo = new FileInfo(currentValues.InputFilePath);
                currentValues.InputFile = seedInfo.Name.ToString();
            }
        }
        catch (Exception e)
        {
            currentValues.InputFile = "N/A";
        }

        // Check if periodic value is true or false and convert to yes or no strings
        string periodicString = "";
        if (currentValues.Periodic == false)
        {
            periodicString = "no";
        }
        else
        {
            periodicString = "yes";
        }

        // Check if step mode value is true or false and convert to yes or no strings
        string stepModeString = "";
        if (currentValues.StepMode == false)
        {
            stepModeString = "no";
        }
        else
        {
            stepModeString = "yes";
        }

        // Check if ghost mode value is true or false and convert to yes or no strings
        string ghostModeString = "";
        if (currentValues.GhostMode == false)
        {
            ghostModeString = "no";
        }
        else
        {
            ghostModeString = "yes";
        }

        // Write all settings/parameters to the console for the user's viewing
        Console.WriteLine("");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("[SUCCESS] All Command Line Arguments Processed:\n");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(String.Format("{0, 25} {1}", "Input File:", currentValues.InputFile));
        Console.WriteLine(String.Format("{0, 25} {1}", "Output File:", currentValues.OutputFile));
        Console.WriteLine(String.Format("{0, 25} {1}", "Generations:", currentValues.Generations));
        Console.WriteLine(String.Format("{0, 25} {1}", "Memory:", currentValues.Memory));
        Console.WriteLine(String.Format("{0, 25} {1} updates/second", "Refresh Rate:", currentValues.MaxRefreshRate));
        Console.WriteLine(String.Format("{0, 25} {1}", "Survival Rules:", currentValues.Survival));
        Console.WriteLine(String.Format("{0, 25} {1}", "Birth Rules:", currentValues.Birth));
        Console.WriteLine(String.Format("{0, 25} {1} ({2})", "Neighbourhood:", currentValues.Neighbourhood, currentValues.NeighbourhoodSize));
        Console.WriteLine(String.Format("{0, 25} {1}", "Centre Checking:", currentValues.CentreCheck));
        Console.WriteLine(String.Format("{0, 25} {1}", "Periodic:", periodicString));
        Console.WriteLine(String.Format("{0, 25} {1}", "Rows:", currentValues.Rows));
        Console.WriteLine(String.Format("{0, 25} {1}", "Columns:", currentValues.Columns));

        if (currentValues.Probability <= 0)
        {
            Console.WriteLine(String.Format("{0, 29}", "Random Factor: N/A"));
        }
        else
        {
            Console.WriteLine(String.Format("{0, 25} {1}%", "Random Factor:", currentValues.Probability * 100));
        }

        Console.WriteLine(String.Format("{0, 25} {1}", "Step Mode:", stepModeString));
        Console.WriteLine(String.Format("{0, 25} {1}", "Ghost Mode:", ghostModeString));
        Console.WriteLine("");
    }

    public static void PerioidicArgCheck()
    {
        // If periodic flag detected, set periodic to true
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Periodic argument detected.");
        Console.ForegroundColor = ConsoleColor.Green;

        currentValues.Periodic = true;

        Console.WriteLine("[SUCCESS] Periodic behaviour set to true\n");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void DimensionArgCheck(string[] args)
    {
        int[] values = new int[2];

        for (int i = 0; i < args.Length; i++)
        {
            // Check if the current value contains the dimensions flag 
            if (args[i].Contains("--dimensions"))
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Dimension argument detected.");
                Console.ForegroundColor = ConsoleColor.White;

                // Check if 2 values are specified 
                if (args.Length - 1 >= i + 2)
                {
                    // Check if both row and column dimensions are specifed
                    if (args.Length >= 3 && !args[i + 1].Contains("--") && !args[i + 2].Contains("--")
                        && Convert.ToInt32(args[i + 1]) >= 1 && Convert.ToInt32(args[i + 1]) <= 48
                        && Convert.ToInt32(args[i + 2]) >= 1 && Convert.ToInt32(args[i + 2]) <= 48)
                    {
                        currentValues.Rows = Convert.ToInt32(args[i + 1]);
                        currentValues.Columns = Convert.ToInt32(args[i + 2]);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[SUCCESS] Dimension arguments passed: {0} rows and {1} columns\n",
                            currentValues.Rows, currentValues.Columns);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[ERROR] Dimension argument MUST have a row AND column integer " +
                            "value specified between 1 and 48 (inclusive).");

                        if (args[i + 2].Contains("--"))
                        {
                            Console.WriteLine("[WARNING] Missing column dimension after --dimension flag.");
                        }

                        Console.WriteLine("Setting dimensions to default values: 16 rows and 16 columns\n");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[ERROR] Dimension argument MUST have a row AND column integer value " +
                        "specified between 1 and 48 (inclusive).");
                    Console.WriteLine("Setting dimensions to default values: 16 rows and 16 columns\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                i += 1;
            }
        }
    }

    public static void SurvivalArgCheck(string[] args)
    {
        int index = 0;
        bool failedArray = false;

        currentValues.Survival = "";

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Survival argument detected.");

        // Determine where the survival flag is in the arguments
        while (true)
        {
            if (args[index].Contains("--survival"))
            {
                index++;
                break;
            }

            index++;
        }

        int numParams = 0;
        int otherIndex = 0;

        // Determine how many parameters for survival were provided by the user
        while (true)
        {
            if (index + otherIndex == args.Length - 1 && !args[index + otherIndex].Contains("--"))
            {
                numParams++;
                break;
            }
            else if (index + otherIndex >= args.Length)
            {
                break;
            }
            else if (args[index + otherIndex].Contains("--"))
            {
                break;
            }

            otherIndex++;
            numParams++;
        }

        // Set the survival value to 0 if no parameters are detected
        if (numParams == 0)
        {
            currentValues.Survival = "0";
            string[] value = { "0" };
            currentValues.SurvivalList = value;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[SUCCESS] Survival argument value: 0\n");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }

        // Create new array with only the parameters provided by the user
        string[] newArray = new string[numParams];

        for (int i = 0; i < numParams; i++)
        {
            newArray[i] = args[index + i]; 
        }

        string arrayForSplitting = "";

        // While loop to calculate what values to include in the survival array
        for (int i = 0; i < numParams; i++)
        {
            if (newArray[i].Contains("."))
            {
                char[] numberCheck = newArray[i].ToCharArray();
                int numberIndex = 0;
                int dotCount = 0;
                string firstVal = "", secondVal = "";

                while (true)
                {
                    if (numberCheck[numberIndex] == '.')
                    {
                        numberIndex++;
                        dotCount++;
                        break;
                    } 
                    else
                    {
                        firstVal += numberCheck[numberIndex];
                        numberIndex++;
                    }
                }

                while (true)
                {
                    if (numberCheck[numberIndex] == '.')
                    {
                        dotCount++;
                        numberIndex++;
                    }
                    else
                    {
                        break;
                    }
                }

                // Make sure that the correct formatting (3 dots or ...) is used
                if (dotCount != 3)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[ERROR] Survival range contains either too many or not enough dots to correctly" +
                        " process the values.");
                    Console.WriteLine("[ERROR] Setting survival range values to standard values: { 2...3 }\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    currentValues.Survival = "2...3";
                    failedArray = true;

                    break;             
                }

                currentValues.Survival += $"{newArray[i]} ";

                while (true)
                {
                    if (numberIndex == numberCheck.Length)
                    {
                        break;
                    }
                    else
                    {
                        secondVal += numberCheck[numberIndex];
                        numberIndex++;
                    }
                }

                int firstValInt = Convert.ToInt32(firstVal);
                int secondValInt = Convert.ToInt32(secondVal);

                // If first value is greater than second value, tell user and switch values around for them
                if (firstValInt > secondValInt)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[ERROR] First value provided in a ranged survival parameter is greater than the " +
                        "second value.");

                    int temp = firstValInt;
                    firstValInt = secondValInt;
                    secondValInt = temp;

                    Console.WriteLine("[ERROR] Switching survival values to the following: { " + firstValInt + "..." +
                        secondValInt + " }");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                string rangeOfValues = "";

                // Add values that aren't already in the arrray to the range of values
                for (int j = firstValInt; j <= secondValInt; j++)
                {
                    if (!arrayForSplitting.Contains(Convert.ToString(j)))
                    {
                        rangeOfValues += Convert.ToString(j);
                        rangeOfValues += ",";
                    }                    
                }

                arrayForSplitting += rangeOfValues;
            }
            else
            {
                // Single integer value parameters
                string additionalString = $"{newArray[i]},";

                if (!arrayForSplitting.Contains(Convert.ToString(additionalString)))
                {                    
                    arrayForSplitting += additionalString;
                    currentValues.Survival += $"{newArray[i]} ";
                }
            }
        }

        // Write list of values to console should the array be processed without errors
        if (!failedArray)
        {
            string[] finalArray = arrayForSplitting.Split(",");

            currentValues.SurvivalList = finalArray;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[SUCCESS] Survival argument range includes values: ");
            for (int i = 0; i < finalArray.Length; i++)
            {
                if (i == finalArray.Length - 2)
                {
                    Console.WriteLine($"{finalArray[i]}\n");
                    break;
                }
                else
                {
                    Console.Write($"{finalArray[i]}, ");
                }
            }  
        }
    }

    public static void BirthArgCheck(string[] args)
    {
        int index = 0;
        bool failedArray = false;

        currentValues.Birth = "";

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Birth argument detected.");

        // Determine where the birth flag is in the arguments
        while (true)
        {
            if (args[index].Contains("--birth"))
            {
                index++;
                break;
            }

            index++;
        }

        int numParams = 0;
        int otherIndex = 0;

        // Determine how many parameters for birth were provided by the user
        while (true)
        {
            if (index + otherIndex == args.Length - 1 && !args[index + otherIndex].Contains("--"))
            {
                numParams++;
                break;
            }
            else if (args[index + otherIndex].Contains("--"))
            {
                break;
            }

            otherIndex++;
            numParams++;
        }

        // Set birth value to 0 if no parameters following the flag are detected
        if (numParams == 0)
        {
            currentValues.Birth = "0";
            string[] value = { "0" };
            currentValues.BirthList = value;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[SUCCESS] Birth argument value: 0\n");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }

        // Create new array with only the parameters provided by the user
        string[] newArray = new string[numParams];

        for (int i = 0; i < numParams; i++)
        {
            newArray[i] = args[index + i];
        }

        string arrayForSplitting = "";

        // While loop to calculate what values to include in the birth array
        for (int i = 0; i < numParams; i++)
        {
            if (newArray[i].Contains("."))
            {
                char[] numberCheck = newArray[i].ToCharArray();
                int numberIndex = 0;
                int dotCount = 0;
                string firstVal = "", secondVal = "";

                while (true)
                {
                    if (numberCheck[numberIndex] == '.')
                    {
                        numberIndex++;
                        dotCount++;
                        break;
                    }
                    else
                    {
                        firstVal += numberCheck[numberIndex];
                        numberIndex++;
                    }
                }

                while (true)
                {
                    if (numberCheck[numberIndex] == '.')
                    {
                        dotCount++;
                        numberIndex++;
                    }
                    else
                    {
                        break;
                    }
                }

                // Ensure the correct formatting (3 dots or ...) is used
                if (dotCount != 3)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[ERROR] Birth range contains either too many or not enough dots to correctly" +
                        " process the values.");
                    Console.WriteLine("[ERROR] Setting birth range values to standard values: { 3 }\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    currentValues.Birth = "3";
                    failedArray = true;

                    break;           
                }

                currentValues.Birth += $"{newArray[i]} ";

                while (true)
                {
                    if (numberIndex == numberCheck.Length)
                    {
                        break;
                    }
                    else
                    {
                        secondVal += numberCheck[numberIndex];
                        numberIndex++;
                    }
                }

                int firstValInt = Convert.ToInt32(firstVal);
                int secondValInt = Convert.ToInt32(secondVal);

                // Should the first value be larger than the second value, tell the user and switch them around
                if (firstValInt > secondValInt)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[ERROR] First value provided in a ranged birth parameter is greater than the " +
                        "second value.");

                    int temp = firstValInt;
                    firstValInt = secondValInt;
                    secondValInt = temp;

                    Console.WriteLine("[ERROR] Switching birth values to the following: { " + firstValInt + "..." +
                        secondValInt + " }");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                string rangeOfValues = "";

                // Add values that aren't already in the arrray to the range of values
                for (int j = firstValInt; j <= secondValInt; j++)
                {
                    if (!arrayForSplitting.Contains(Convert.ToString(j)))
                    {
                        rangeOfValues += Convert.ToString(j);
                        rangeOfValues += ",";
                    }
                }

                arrayForSplitting += rangeOfValues;
            }
            else
            {
                string additionalString = $"{newArray[i]},";

                // Single integer value parameters
                if (!arrayForSplitting.Contains(Convert.ToString(additionalString)))
                {
                    arrayForSplitting += additionalString;
                    currentValues.Birth += $"{newArray[i]} ";
                }
            }
        }

        // Write list of values to console should the array be processed without errors
        if (!failedArray)
        {
            string[] finalArray = arrayForSplitting.Split(",");

            currentValues.BirthList = finalArray;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[SUCCESS] Birth argument range includes values: ");
            for (int i = 0; i < finalArray.Length; i++)
            {
                if (i == finalArray.Length - 2)
                {
                    Console.WriteLine($"{finalArray[i]}\n");
                    break;
                }
                else
                {
                    Console.Write($"{finalArray[i]}, ");
                }
            }
        }
    }

    public static void CheckSurvivalAndBirthValues(CLA currentValues)
    {
        double numNeighbours = 0;
        bool throwError = false;

        try
        {
            if (currentValues.Neighbourhood == "Moore")
            {
                // Calculate number of neighbours based on the order
                int order = currentValues.NeighbourhoodSize + 1;

                numNeighbours = Math.Pow(2 * order - 1, 2);

                // Reduce number of neighbours by 1 if the centre cell of a neighbourhood isn't being checked
                if (!currentValues.CentreCheck)
                {
                    numNeighbours--;
                }

                // Check all survival values and make sure that none of them are above the number of possible
                // neighbours - if the is a value exceeding the number, reset the survival values to default.
                for (int i = 0; i < currentValues.SurvivalList.Length; i++)
                {
                    if (currentValues.SurvivalList[i] != "" &&
                        Convert.ToDouble(currentValues.SurvivalList[i]) > numNeighbours)
                    {
                        string[] values = { "2", "3" };
                        currentValues.SurvivalList = values;
                        currentValues.Survival = "2...3";
                        throwError = true;
                        break;
                    }
                }

                // Check all birth values and make sure that none of them are above the number of possible
                // neighbours - if the is a value exceeding the number, reset the birth values to default.
                for (int i = 0; i < currentValues.BirthList.Length; i++)
                {
                    if (currentValues.BirthList[i] != "" &&
                        Convert.ToDouble(currentValues.BirthList[i]) > numNeighbours)
                    {
                        string[] values = { "3" };
                        currentValues.BirthList = values;
                        currentValues.Birth = "3";
                        throwError = true;
                        break;
                    }
                }
            }
            else if (currentValues.Neighbourhood == "Von Neumann")
            {
                // Calculate number of neighbours based on the order
                int order = currentValues.NeighbourhoodSize;

                numNeighbours = Math.Pow(order, 2) + Math.Pow(order + 1, 2);

                // Reduce number of neighbours by 1 if the centre cell of a neighbourhood isn't being checked
                if (!currentValues.CentreCheck)
                {
                    numNeighbours--;
                }

                // Check all survival values and make sure that none of them are above the number of possible
                // neighbours - if the is a value exceeding the number, reset the survival values to default.
                for (int i = 0; i < currentValues.SurvivalList.Length; i++)
                {
                    if (currentValues.SurvivalList[i] != "" &&
                        Convert.ToDouble(currentValues.SurvivalList[i]) > numNeighbours)
                    {
                        string[] values = { "2", "3" };
                        currentValues.SurvivalList = values;
                        currentValues.Survival = "2...3";
                        throwError = true;
                        break;
                    }
                }

                // Check all birth values and make sure that none of them are above the number of possible
                // neighbours - if the is a value exceeding the number, reset the birth values to default.
                for (int i = 0; i < currentValues.BirthList.Length; i++)
                {
                    if (currentValues.BirthList[i] != "" &&
                        Convert.ToDouble(currentValues.BirthList[i]) > numNeighbours)
                    {
                        string[] values = { "3" };
                        currentValues.BirthList = values;
                        currentValues.Birth = "3";
                        throwError = true;
                        break;
                    }
                }
            }

            if (throwError)
            {
                throw new SurvivalOrBirthOutsideNumberOfNeighboursException();
            }
        }
        catch (SurvivalOrBirthOutsideNumberOfNeighboursException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] One or more Survival or Birth values exceeds the " +
                $"number of neighbours ({numNeighbours}).");
            Console.WriteLine("[ERROR] Resetting the exceeding value holder (either Survival or Birth or both) " +
                "to their default values.");
            Console.ForegroundColor = ConsoleColor.White;

            return;
        }

        return;
    }

    public static void RandomArgCheck(string[] args, int index)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Random Factor argument detected.");
        Console.ForegroundColor = ConsoleColor.White;

        // Check if probability value is specifed
        if (index < args.Length - 1 && !args[index + 1].Contains("--")
            && Convert.ToDouble(args[index + 1]) >= 0 && Convert.ToDouble(args[index + 1]) <= 1)
        {
            currentValues.Probability = Convert.ToDouble(args[index + 1]);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[SUCCESS] Random Factor argument passed: {0}%\n", currentValues.Probability);
            Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] Random Factor argument MUST have a decimal value specified " +
                "between 0 and 1 (inclusive).");
            Console.WriteLine("[ERROR] Setting random factor to default value: 0.5\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public static void SeedFileCheck(string[] args, int index)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Seed argument detected.");
        Console.ForegroundColor = ConsoleColor.White;

        currentValues.Probability = -1;

        // Check if seed value is specifed and file exists (validation)
        if (args.Length >= 2 && index < args.Length - 1 && !args[index + 1].Contains("--")
            && args[index + 1].Contains(".seed"))
        {
            currentValues.InputFilePath = args[index + 1];

            if (File.Exists(currentValues.InputFilePath))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[SUCCESS] Seed file found.");
                string fileName = Path.GetFileName(currentValues.InputFilePath);
                Console.WriteLine("File Name: {0}\n", fileName);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERROR] File not found.");
                Console.WriteLine("Setting seed to default value: N/A\n");
                Console.ForegroundColor = ConsoleColor.White;

                currentValues.InputFilePath = "N/A";
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] Seed argument MUST have a file name with the .seed extension " +
                "specified.");
            Console.WriteLine("[ERROR] Setting seed to default value: N/A\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public static void OutputFileCheck(string[] args, int index)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Output File argument detected.");
        Console.ForegroundColor = ConsoleColor.White;

        // Check if seed file is specifed
        if (args.Length >= 2 && index < args.Length - 1 && !args[index + 1].Contains("--")
            && args[index + 1].Contains(".seed"))
        {
            try
            {
                string outputFilePath = args[index + 1];
                string trimmedFilePath = Path.GetDirectoryName(outputFilePath);
                bool pathCheck = Directory.Exists(trimmedFilePath);
                
                if (!pathCheck)
                {
                    throw new DirectoryNotFoundException();
                }

                currentValues.OutputFile = args[index + 1];

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[SUCCESS] Output file with name {currentValues.OutputFile} will be found at:");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(outputFilePath);
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERROR] Output file path does not exist. Please use a valid file path.");
                Console.WriteLine("[ERROR] Setting output file to default value: N/A\n");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] Output file argument MUST have a file path and file name with the .seed " +
                "extension specified.");
            Console.WriteLine("[ERROR] Setting output file to default value: N/A\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public static void GenerationArgCheck(string[] args, int index)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Generation argument detected.");
        Console.ForegroundColor = ConsoleColor.White;

        // Check if generation value is specifed
        if (index < args.Length - 1 && !args[index + 1].Contains("--")
            && Convert.ToInt32(args[index + 1]) > 0)
        {
            currentValues.Generations = Convert.ToInt32(args[index + 1]);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[SUCCESS] Generation argument passed: {0}\n", currentValues.Generations);
            Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] Generation argument MUST have an integer value specified " +
                "above 0.");
            Console.WriteLine("[ERROR] Setting Generation to default value: 50\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public static void MaxUpdateArgCheck(string[] args, int index)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Maximum Update Rate argument detected.");
        Console.ForegroundColor = ConsoleColor.White;

        // Check if update rate value is specifed
        if (index < args.Length - 1 && !args[index + 1].Contains("--")
            && Convert.ToInt32(args[index + 1]) <= 30 && Convert.ToInt32(args[index + 1]) >= 1)
        {
            currentValues.MaxRefreshRate = Convert.ToInt32(args[index + 1]);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[SUCCESS] Maximum Update Rate argument passed: {0}\n", currentValues.MaxRefreshRate);
            Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] Maximum Update Rate argument MUST have a integer value " +
                "specified between 1 and 30 (inclusive).");
            Console.WriteLine("Setting Maximum Update Rate to default value: 5\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public static void StepArgCheck()
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Step Mode argument detected.");
        Console.ForegroundColor = ConsoleColor.Green;

        currentValues.StepMode = true;

        Console.WriteLine("[SUCCESS] Step Mode behaviour set to true\n");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void GhostArgCheck()
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Ghost Mode argument detected.");
        Console.ForegroundColor = ConsoleColor.Green;

        currentValues.GhostMode = true;

        Console.WriteLine("[SUCCESS] Ghost Mode behaviour set to true\n");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void NeighourhoodCheck(string[] args, int i, int numRows, int numCols)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Neighbourhood argument detected.");
        Console.ForegroundColor = ConsoleColor.Green;

        bool orderOutOfRange = false;

        // Check if all three required arguments are provided, if not, give user statement/warning
        if (i > args.Length - 4)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] Invalid number of neighbourhood parameters provided.");
            Console.WriteLine("[ERROR] Setting neighbourhood parameters to default values: Moore, 1, False\n");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }

        if (!args[i + 1].Contains("--") && !args[i + 2].Contains("--") && !args[i + 3].Contains("--"))
        {
            if (args[i + 1].Contains("moore"))
            {
                // Moore Neighbourhood declared
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[SUCCESS] Moore neighbourhood type declared.");
                currentValues.Neighbourhood = "Moore";
            }
            else if (args[i + 1].Contains("vonNeumann"))
            {
                // Von Neumann Neighbourhood declared
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[SUCCESS] Von Neumann neighbourhood type declared.");
                currentValues.Neighbourhood = "Von Neumann";
            }
            else
            {
                // Invalid Neighbourhood declared. Reset to Moore
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERROR] Invalid Neighbourhood type declared. Setting neighbourhood to " +
                    "default value: moore");
                currentValues.Neighbourhood = "Moore";
            }

            try
            {
                // Ensure the order is correct and within specifications
                int userSpecifiedOrder = Convert.ToInt32(args[i + 2]);

                if (userSpecifiedOrder > 10 || userSpecifiedOrder < 0)
                {
                    throw new Exception();
                }
                
                currentValues.NeighbourhoodSize = userSpecifiedOrder;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[SUCCESS] Neighbourhood order declared: {currentValues.NeighbourhoodSize}");
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERROR] Invalid Neighbourhood order declared. Setting order to default value: 1");
            }

            try
            {
                // Check if valid centre checking boolean is provided
                currentValues.CentreCheck = Convert.ToBoolean(args[i + 3]);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[SUCCESS] Neighbourhood centre checking bool declared: {currentValues.CentreCheck}\n");
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERROR] Invalid Neighbourhood centre checking bool declared. Setting order to default value: false\n");
            }

            Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] Invalid number of neighbourhood parameters provided.");
            Console.WriteLine("[ERROR] Setting neighbourhood parameters to default values: Moore, 1, False\n");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }        
    }

    public static void NeighbourhoodOrderCheck(int userSpecifiedOrder, int numRows, int numCols)
    {
        // Make sure that the order specified is between 1 and 10 (inclusive) and less than half of the
        // smallst dimension (rows or columns)

        try
        {
            if (userSpecifiedOrder > numRows / 2 || userSpecifiedOrder > numCols / 2)
            {
                throw new Exception();
            }
        }
        catch
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] Neighbourhood order declared is greater than half the shortest row " +
                        "or column length.");
            Console.WriteLine("[ERROR] Invalid Neighbourhood order declared. Setting order to default value: 1");
            currentValues.NeighbourhoodSize = 1;
            Console.ForegroundColor = ConsoleColor.White;
        }
        
    }

    public static void MemoryArgCheck(string[] args, int index)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Memory argument detected.");
        Console.ForegroundColor = ConsoleColor.White;

        // Check if memory value is specifed
        if (index < args.Length - 1 && !args[index + 1].Contains("--")
            && Convert.ToInt32(args[index + 1]) <= 512 && Convert.ToInt32(args[index + 1]) >= 4)
        {
            currentValues.Memory = Convert.ToInt32(args[index + 1]);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[SUCCESS] Memory argument passed: {0}\n", currentValues.Memory);
            Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] Memory argument MUST have a integer value " +
                "specified between 4 and 512 (inclusive).");
            Console.WriteLine("Setting Memory to default value: 16\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}

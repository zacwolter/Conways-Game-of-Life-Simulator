# Build Instructions

Please navigate into the first "Life" folder, which should contain two folders, one named "Life" and another named "Display", 
as well as a .sln file. Please open the .sln file in Microsoft Visual Studio.

Upon opening the file in Visual Studio, you may take a look at the code (and alter if you wish, however, beware that
it could break the program and not allow you to build it). Once you are ready to build the program, click on the button
at the top of the screen which a green arrow that says "Life". This button will build the program for you, and you 
will find the built "life.dll" file within the Life/Life/bin/Debug/netcoreapp3.1 directory in the extracted folder.

# Usage 

### Windows Usage Method

Open command prompt and use the cd command (formatting: __*cd {your-file-path}*__) to navigate to the folder which the built 
life.dll file sits in. Once located, type __*dotnet life.dll*__ into the command prompt and feel free to immediately run
the program, as it will run using default parameters. 


### Running Life With Custom Parameters

The game of life has multiple parameters which are customizable to provide different experiences and simulations.
These parameters can be changed by typing __*--{flag-name} {parameters}*__ after __*dotnet life.dll*__ in the command 
prompt (making one long command).

Once all chosen parameters have been specified, simply press the enter button to start the game, where you will be
notified in green, yellow and red messages whether or not the specified parameters were correctly and successfully
loaded into the game. Finally, the game will display all the final parameters which the game will use to simulate
life. Simply press the space bar to continue and begin the simulation.

If you recieve a warning regarding cells in the seed file being out of the boundaries, refer to the __INPUT FILE__ 
(__*--seed*__) parameter information for further information.

There are two ways in which the simulation may end:

1. The simulation will detect a steady-state (this can vary from an empty grid, to 0 changes in cells between
generations or a looping/oscillating scenario)
        
OR
    
2. The simulation will reach the specified number of generations (either the default 50 generations if you don't 
specify a number of generations or the number of generations you specify before starting the game)

Once the game has ended, simply press the space bar once more to end the simulation and return to the command prompt, 
where the game will tell you what the periodicity of the simulation was (this will only occur if the simulation reaches 
a steady-state before reaching the number of generations).

## Periodicity

Periodicity is simply the number of generations between a particular generation and the final one (both of which will be 
the same). If the periodicity is equal to 0, that means that an empty grid was detected, meaning the simulation will no 
longer have any changes in future generations. If the periodicity is greater than 0, it means that there is a loop where 
the same generation grid occurs every n generations. For example, if the periodicity is equal to 1, there will be no changes
to the grid in future generations as it has reached a full steady-state. If the periodicity is equal to 2 or greater, then 
every 2nd or n-th generation the same grid layout will be seen. 


# Customizable Parameters

## Row and Column Dimensions

__Usage:__ __*--dimensions {rows} {columns}*__

The __*--dimensions*__ parameter allows you to alter the size of the grid which is used to simulate the game of life.
The flag must be followed by two parameters, the first specifying the number of rows and the second specifying
the number of columns. Note that both number of rows and columns must be a number __BETWEEN 4 and 48 (inclusive)__.

__Default values:__ The game board will have 16 rows and 16 columns

__Accepted values:__ Two integers between 4 and 48 (inclusive)

__** PLEASE NOTE **__

It is highly recommended that the __*--dimensions*__ flag is used before any other flags to assist other argument checking
software in checking if the user specified values are valid.


## Periodic Behaviour

__Usage:__ __*--periodic*__

Whether the game board acts in a periodic manner may be specified using the __*--periodic*__ option. This flag does not 
require any following parameters, as it is a boolean (either it's on or it's not). Periodic behaviour can be
described as the "wrapping around" of the board, so a cell that exists on the boundary of the universe (or grid)
will have neighbours that "wrap around" to the other side of the universe. As defualt, the periodic behaviour is
turned off.

__Default value:__ N/A


## Random Factor

__Usage:__ __*--random {probability}*__

If no seed is specified, you can change the probability of any cell to be initially active using the __*--random*__ option.
This flag needs to be followed by a single parameter - probability, specified as a decimal value between 0 (0% chance
of being initially active) and 1 (100% chance of being initially active) (inclusive).

__Default value:__ 0.5 (50%)


## Input File (Seeds)

__Usage: *--seed {file-path}*__

The path of the file representing the intial game state may be specified using the --seed option. This flag should be
followed by a single parameter - the file provided must use a .seed file extension, otherwise it will NOT be accepted
by the game. If the .seed file is located in the same folder where the life.dll file exists, you only need to use
the <filename>.seed, whereas if the file is located anywhere else, it is highly recommended that you use the full file
path, starting from the drive (e.g. D:/Documents/Life/Seeds/<filename>.seed). The command line interface will tell you
if the file has been found in the location specified and if so, will use the seed to set the intial state of the game.

__** PLEASE NOTE **__

If the seed file contains cells that are outside of the boundaries (either the default boundaries or those set by the 
user), the game will produce a warning after pressing space to begin the simulation, giving you the option to press
escape to leave the game and alter the dimensions of the board, or proceed with the simulation, where the values of 
cells specified outside the boundaries will not be included. It is important to note that this will most likely affect
the game and how the simulation plays out compared to the full seed being within the boundaries.

__Default value:__ N/A


## Generations

__Usage: *--generations {number}*__

The number of generations in the game may be specified using the __*--generations*__ option. This flag needs to be followed 
by a single parameter, which should be a positive non-zero integer. The more generations specified, the longer the 
game will last for. 

__Default value:__ 50

__Accepted values:__ positive non-zero integers (e.g. any number that isn't a decimal and higher than 0)


## Maximum Update Rate

__Usage: *--max-update {updatesPerSecond}*__

The maximum number of generational updates per second may be specified using the __*--max-update*__ option. This flag needs 
to be followed by a single parameter that must be a float between 1 and 30 (inclusive). Increasing the maximum update 
rate will speed up the simulation and create a smoother animation, however, you can miss the smaller details within 
each generation.

__** PLEASE NOTE **__

If step mode is enabled by the user, the maximum update rate will default to 0, as it has no use in that situation.

__Default value:__ 5 updates per second

__Accepted values:__ Any float (decimal) value between 1 and 30 (inclusive).


## Step Mode

__Usage: *--step*__

Using the __*--step*__ option will enable the step feature, where the program will wait for the user to press the space bar
to progress to the next generation. This mode can be useful when testing the program for faults or for better
clarification on how the game works.

__Default value:__ The program will NOT run in step mode.

## Neighbourhoods

__Usage: *--neighbour {type} {order} {centre-count}*__

The type and size of the neighbourhood to be used may be specified using the __*--neighbour*__ flag. The flag must be
followed by three parameters, the first specifying the neighbourhood type, the second specifying order (size) of
the neighbourhood, and the third specifying whether the centre of the neighbourhood is counted as a neighbour or not.

__Defaults:__ The game will use a 1st order Moore neighbourhood that doesn't count the centre.

__Accepted values:__ the neighbourhood type must be one of two strings, either "moore" or "vonNeumann", __CASE SENSITIVE.__
The order must be an integer between 1 and 10 (inclusive) and less than half of the smallest dimensions (rows or 
columns). Whether the centre is counted must be a boolean (true or false).


## Survival and Birth

__Usage: *--survival {param1} {param2} {param3} ... --birth {param1} {param2} {param3} ...*__

The number of live neighbours required for a cell to survive or be born in evolution may be specified using the 
__*--survival*__ and __*--birth*__ options respectively. These flags should be followed by an artbitrary number of parameters
(greater than or equal to 0).

__Defaults:__ Either 2 or 3 live neighbours are required for a cell to survive. Exactly 3 live neighbours are required for
a cell to be born.

__Accepted values:__ Each parameter must be a single integer, or two integers separated by ellipses (...). Integers 
separated by ellipses represent all number within that range (inclusively). The numbers provided must be less 
than or equal to the number of neighbouring cells and non-negative.


## Generational Memory

__Usage: *--memory {number}*__

The number of generations stored in memory for the detection of a steady-state may be specified using the __*--memory*__ 
option. This flag should be followed by a single parameter.

__Defaults:__ The program stores 16 generations for detecting a steady-state.

__Accepted values:__ The value must be an integer between 4 and 512 (inclusive).


## Output File

__Usage: *--output {file-name/path}

The path of the output file may be specified using the __*--output*__ option. This flag should be followed by a 
single parameter.

__Defaults:__ No output file is used.

__Accepted values:__ The value must be a vaild or absolute or relative file path with the .seed file extension.


## Ghost Mode

__Usage: *--ghost*__

Whether the program will render the game using ghost mode may be specified using the __*--ghost*__ option.

__Defaults:__ The program wil NOT run in ghost mode.


# Notes 

The simulation and all aspects of the game have been thoroughly tested and I believe most, if not all possible errors
or bugs have been accounted for and dealt with, whilst providing the user with helpful and informative error messages
to clarify what went wrong.

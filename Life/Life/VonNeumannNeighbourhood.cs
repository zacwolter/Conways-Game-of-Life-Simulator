using System;
using System.Collections.Generic;
using System.Text;

class VNNeighbourhood : NeighbourhoodScanner
{
    public VNNeighbourhood(int order, int selectedRow, int selectedColumn, int numRows, int numCols,
    bool periodic, bool centreChecking, string neighbourhoodType)
        : base(order, selectedRow, selectedColumn, numRows, numCols, periodic, centreChecking, neighbourhoodType)
    {
        Order = order;
        SelectedRow = selectedRow;
        SelectedCol = selectedColumn;
        NumRows = numRows;
        NumCols = numCols;
        Periodic = periodic;
        CentreChecking = centreChecking;
        NeighbourhoodType = neighbourhoodType;
    }

    public override int NeighbourhoodCount(int[,,] currentCells)
    {
        int aliveNeighbours = 0;

        for (int row = -Order + SelectedRow; row <= SelectedRow + Order; row++)
        {
            for (int col = -Order + SelectedCol; col <= SelectedCol + Order; col++)
            {
                    // ***** Non-Periodic Check *****
                    if (!Periodic && row >= 0 && row < NumRows && col >= 0 && col < NumCols
                    && Math.Abs(row - SelectedRow) + Math.Abs(col - SelectedCol) <= Order)
                    {
                        if (row == SelectedRow && col == SelectedCol)
                        {
                            if (CentreChecking)
                            {
                                if (currentCells[row, col, 0] == 1)
                                {
                                    aliveNeighbours++;
                                }
                            }
                        }
                        else if (currentCells[row, col, 0] == 1)
                        {
                            aliveNeighbours++;
                        }
                    }

                    // ***** Periodic Check *****
                    // Periodic cases: 
                    // 1. Outside left side
                    if (Periodic && row >= 0 && row < NumRows && col >= 0 && col < NumCols
                        && Math.Abs(row - SelectedRow) + Math.Abs(col - SelectedCol) <= Order)
                    {
                        if (row == SelectedRow && col == SelectedCol)
                        {
                            if (CentreChecking)
                            {
                                if (currentCells[row, col, 0] == 1)
                                {
                                    aliveNeighbours++;
                                }
                            }
                        }
                        else if (currentCells[row, col, 0] == 1)
                        {
                            aliveNeighbours++;
                        }
                    }
                    else if (Periodic && col < 0 && col < NumCols && row >= 0 && row < NumRows
                        && Math.Abs(row - SelectedRow) + Math.Abs(col - SelectedCol) <= Order)
                    {
                        // check cell that is x cells to the left from the right side of the grid
                        // NOTE: col will be a negative number
                        if (currentCells[row, NumCols + col, 0] == 1)
                        {
                            aliveNeighbours++;
                        }
                    }

                    // 2. Outside right side
                    else if (Periodic && col >= NumCols && row >= 0 && row < NumRows
                        && Math.Abs(row - SelectedRow) + Math.Abs(col - SelectedCol) <= Order)
                    {
                        // check cell that is x cells to the left from the left side of the grid
                        if (currentCells[row, col - NumCols, 0] == 1)
                        {
                            aliveNeighbours++;
                        }
                    }

                    // 3. Outside top side
                    else if (Periodic && col >= 0 && col < NumCols && row >= 0 && row >= NumRows
                        && Math.Abs(row - SelectedRow) + Math.Abs(col - SelectedCol) <= Order)
                    {
                        // check cell that is x cells from the bottom of the grid
                        if (currentCells[row - NumRows, col, 0] == 1)
                        {
                            aliveNeighbours++;
                        }
                    }

                    // 4. Outside bottom side
                    else if (Periodic && col >= 0 && col < NumCols && row < 0
                        && Math.Abs(row - SelectedRow) + Math.Abs(col - SelectedCol) <= Order)
                    {
                        // check cell that is x cells from the top of the grid
                        // NOTE: row will be a negative number
                        if (currentCells[NumRows + row, col, 0] == 1)
                        {
                            aliveNeighbours++;
                        }
                    }

                    // 5. Outside top left corner
                    else if (Periodic && col < 0 && row >= NumRows
                        && Math.Abs(row - SelectedRow) + Math.Abs(col - SelectedCol) <= Order)
                    {
                        // check bottom right corresponding cell based on location
                        // NOTE: col will be a negative number
                        if (currentCells[row - NumRows, NumCols + col, 0] == 1)
                        {
                            aliveNeighbours++;
                        }
                    }

                    // 6. Outside top right corner
                    else if (Periodic && col >= NumCols && row >= NumRows
                        && Math.Abs(row - SelectedRow) + Math.Abs(col - SelectedCol) <= Order)
                    {
                        // check bottom right corresponding cell based on location
                        if (currentCells[row - NumRows, col - NumCols, 0] == 1)
                        {
                            aliveNeighbours++;
                        }
                    }

                    // 7. Outside bottom left corner
                    else if (Periodic && col < 0 && row < 0
                        && Math.Abs(row - SelectedRow) + Math.Abs(col - SelectedCol) <= Order)
                    {
                        // check bottom right corresponding cell based on location
                        // NOTE: row and col will be negative numbers
                        if (currentCells[NumRows + row, NumCols + col, 0] == 1)
                        {
                            aliveNeighbours++;
                        }
                    }

                    // 8. Outside bottom right corner
                    else if (Periodic && col >= NumCols && row < 0
                        && Math.Abs(row - SelectedRow) + Math.Abs(col - SelectedCol) <= Order)
                    {
                        // check bottom right corresponding cell based on location
                        // NOTE: row will be a negative number
                        if (currentCells[NumRows + row, col - NumCols, 0] == 1)
                        {
                            aliveNeighbours++;
                        }
                    }
                }
            }

        return aliveNeighbours;
    }
}

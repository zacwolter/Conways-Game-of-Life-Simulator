using System;
using System.Collections.Generic;
using System.Text;

class NeighbourhoodScanner
{
    private int order = 1, selectedRow, selectedCol, numRows = 16, numColumns = 16;
    private bool periodic = false, centreChecking = true;
    private string neighbourhoodType;

    public int Order
    {
        get
        {
            return order;
        }
        set
        {
            order = value;
        }
    }

    public int SelectedRow
    {
        get
        {
            return selectedRow;
        }
        set
        {
            selectedRow = value;
        }
    }

    public int SelectedCol
    {
        get
        {
            return selectedCol;
        }
        set
        {
            selectedCol = value;
        }
    }

    public int NumRows
    {
        get
        {
            return numRows;
        }
        set
        {
            numRows = value;
        }
    }

    public int NumCols
    {
        get
        {
            return numColumns;
        }
        set
        {
            numColumns = value;
        }
    }

    public bool Periodic
    {
        get
        {
            return periodic;
        }
        set
        {
            periodic = value;
        }
    }

    public bool CentreChecking
    {
        get
        {
            return centreChecking;
        }
        set
        {
            centreChecking = value;
        }
    }

    public string NeighbourhoodType
    {
        get
        {
            return neighbourhoodType;
        }
        set
        {
            neighbourhoodType = value;
        }
    }

    public NeighbourhoodScanner(int order, int selectedRow, int selectedColumn, int numRows, int numCols,
    bool periodic, bool centreChecking, string neighbourhoodType)
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

    // NeighbourhoodCount method is designed for overriding so that the respective neighbourhoods can have varying
    // methods due to different neighbourhood shapes
    public virtual int NeighbourhoodCount(int[,,] currentCells)
    {
        return 0;
    }
}

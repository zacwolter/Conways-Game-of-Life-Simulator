using System;
using System.Collections.Generic;
using System.Text;

class SeedValueExceedsDimensionsException : Exception
{
    private static string message = "One or more values in .seed file exceeds grid dimensions. ";

    public SeedValueExceedsDimensionsException()
    {
    }

    public SeedValueExceedsDimensionsException(string message) : base(message)
    {
    }
}

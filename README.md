# LightCSV
A very lightweight CSV parser that balances speed and convenience, written in C#.

## Installation
Use the following command:
```
dotnet add package LightCSV
```

## Important Links
[Nuget Page](https://www.nuget.org/packages/LightCSV)

## Example
```c#
using LightCSV;

class Program
{
    public static void Main()
    {
        //Parse from CSV string
        CSVObject fromString = CSVObject.Parse("csvString");

        //Parse from CSV file
        CSVObject fromFile = CSVObject.ParseFromFile("file.csv");

        //Get the position of a string value
        fromFile.GetPosition("Test!");

        //Get a column from a value included in it
        fromFile.GetColumn("valueInColumn");

        //Get a row from a value included in it
        fromFile.GetRow("valueInRow");

        //Get a column by index
        fromFile.GetColumn(0);

        //Get a row by index
        fromFile.GetRow(0);

        //A matrix of all the string entries
        string[,] entries = fromFile.Entries;
    }
}
```

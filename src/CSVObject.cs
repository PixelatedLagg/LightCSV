using System.Text;

namespace LightCSV
{
    /// <summary>Stores/handles CSV parsing operations.</summary>
    public sealed class CSVObject
    {
        /// <summary>The entries of the CSV data.</summary>
        public readonly string[,] Entries;

        /// <summary>Creates a CSVObject instance.</summary>
        /// <param name="entries">The CSV entries to store.</param>
        public CSVObject(string[,] entries)
        {
            Entries = entries;
        }

        /// <summary>Gets a row of CSV entries by index.</summary>
        /// <param name="index">The index of the row to get.</param>
        public string[] GetRow(int index)
        {
            int length = Entries.GetLength(1);
            string[] result = new string[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = Entries[index, i];
            }
            return result;
        }

        /// <summary>Gets a row of CSV entries by a string value included in the row.</summary>
        /// <param name="entry">The string value included in the row.</param>
        public string[] GetRow(string entry)
        {
            int index = GetPosition(entry).Item1;
            int length = Entries.GetLength(1);
            string[] result = new string[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = Entries[index, i];
            }
            return result;
        }

        /// <summary>Gets a column of CSV entries by index.</summary>
        /// <param name="index">The index of the column to get.</param>
        public string[] GetColumn(int index)
        {
            int length = Entries.GetLength(0);
            string[] result = new string[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = Entries[i, index];
            }
            return result;
        }

        /// <summary>Gets a column of CSV entries by a string value included in the column.</summary>
        /// <param name="entry">The string value included in the column.</param>
        public string[] GetColumn(string entry)
        {
            int index = GetPosition(entry).Item2;
            int length = Entries.GetLength(0);
            string[] result = new string[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = Entries[i, index];
            }
            return result;
        }

        /// <summary>Gets the position of a string value in (row, column) format. Returns (-1, -1) if the entry is not found in the CSV data.</summary>
        /// <param name="entry">The string value to get the position of.</param>
        public (int, int) GetPosition(string entry)
        {
            for (int i = 0; i < Entries.GetLength(0); i++)
            {
                for (int j = 0; j < Entries.GetLength(1); j++)
                {
                    if (Entries[i, j] == entry || Entries[i, j] == $"\"{entry}\"")
                    {
                        return (i, j);
                    }
                }
            }
            return (-1, -1);
        }

        /// <summary>Create and populate a CSVObject with the contents of a CSV file.</summary>
        /// <param name="file">The CSV file to get the data from.</param>
        /// <param name="separationChar">The character that separates each value (default for CSV is of course ',').</param>
        public static CSVObject ParseFromFile(string file, char separationChar = ',') => Parse(File.ReadAllText(file), separationChar);

        /// <summary>Create and populate a CSVObject with the contents of a CSV string.</summary>
        /// <param name="csv">The CSV string to get the data from.</param>
        /// <param name="separationChar">The character that separates each value (default for CSV is of course ',').</param>
        public static CSVObject Parse(string csv, char separationChar = ',')
        {
            int width = 1;
            string[] lines = csv.Split('\n');
            for (int i = 0; i < lines[0].Length; i++)
            {
                switch (lines[0][i])
                {
                    case '"':
                        while (lines[0][i] != '"')
                        {
                            i++;
                        }
                        break;
                    default:
                        if (lines[0][i] == separationChar)
                        {
                            width++;
                        }
                        break;
                }
            }
            string[,] entries = new string[lines.Length, width];
            int valueIndex = 0;
            StringBuilder token = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    switch (lines[i][j])
                    {
                        case '"':
                            j++;
                            while (lines[i][j] != '"')
                            {
                                token.Append(lines[i][j]);
                                j++;
                            }
                            break;
                        default:
                            if (lines[i][j] == separationChar)
                            {
                                entries[i, valueIndex] = token.ToString();
                                token.Clear();
                                valueIndex++;
                            }
                            else
                            {
                                token.Append(lines[i][j]);
                            }
                            break;
                    }
                }
                entries[i, valueIndex] = token.ToString();
                token.Clear();
                valueIndex = 0;
            }
            return new CSVObject(entries);
        }
    }
}
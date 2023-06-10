using System;
using System.IO;

public class ChunkReader
{
    public ChunkReader(StreamReader _reader, string _line)
    {
        reader = _reader;
        SetValues(_line);
    }
    public StreamReader reader;
    public string line;
    public int? number;
    public string str;

    public int Compare(ChunkReader cr)
    {
        int result = string.Compare(str, cr.str, StringComparison.Ordinal);
        if (result == 0)
        {
            int number1 = number ?? GetNumber(line);
            int number2 = cr.number ?? GetNumber(cr.line);
            number = number1;
            cr.number = number2;

            result = number1.CompareTo(number2);
        }

        return result;
    }

    public void SetValues(string _line)
    {
        line = _line;
        number = null;
        int dotIndex = _line.IndexOf('.');
        str = _line.Substring(dotIndex + 2);
    }

    public int GetNumber(string _line)
    {
        int dotIndex = _line.IndexOf('.');
        return int.Parse(_line.Substring(0, dotIndex));
    }
}
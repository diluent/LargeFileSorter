using System;
using System.Diagnostics;
using System.IO;
using System.Text;

class TestFileGenerator
{
    private const int MaxStringLength = 50;
    private const int MaxNumber = 100000;

    public string Run(string filePath, int numberOfLines)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        GenerateTestFile(numberOfLines, filePath);

        stopwatch.Stop();

        long fileSizeInBytes = new FileInfo(filePath).Length;
        double fileSizeInMB = fileSizeInBytes / (1024.0 * 1024.0);

        Console.WriteLine($"Test file generated successfully at: {filePath}");
        Console.WriteLine($"File size: {fileSizeInMB:F2} MB");
        Console.WriteLine($"Time taken: {stopwatch.Elapsed}");

        return filePath;
    }

    void GenerateTestFile(int numberOfLines, string filePath)
    {
        Random random = new Random();
        StringBuilder sb = new StringBuilder();

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < numberOfLines; i++)
            {
                int number = random.Next(MaxNumber);
                string str = GenerateRandomString(random);

                sb.Clear();
                sb.Append(number.ToString());
                sb.Append(". ");
                sb.Append(str);

                writer.WriteLine(sb.ToString());
            }
        }
    }

    string GenerateRandomString(Random random)
    {
        int length = random.Next(1, MaxStringLength + 1);
        StringBuilder sb = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            char randomChar = (char)random.Next('a', 'z' + 1);
            sb.Append(randomChar);
        }

        return sb.ToString();
    }
}

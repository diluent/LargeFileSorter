
using System;

namespace Sorting
{
    class Program
    {
        private const string defaultFilePath = "test_data.txt";
        private const string defaultSortedFilePath = "sorted_data.txt";

        // args[0] should be either "geneartor", "sorter", or empty
        // in case of empy, both generation and sorting will run.
        // Possible command could look like: "dotnet run generator --project Sorting"
        static void Main(string[] args)
        {
            var mode = args.Length > 0 ? args[0] : "";

            switch (mode)
            {
                case "generator":
                    Generator();
                    break;
                case "sorter":
                    Console.WriteLine("Running only sorter");
                    Sorter("");
                    break;
                default:
                    Console.WriteLine("Running generator and sorter");
                    string filePath = Generator();
                    Sorter(filePath);
                    break;
            }
        }

        static string Generator()
        {
            Console.WriteLine("Enter the number of lines to generate in the test file (31000000 is about 1Gb, 310000000 is about 10Gb):");
            if (!int.TryParse(Console.ReadLine(), out int numberOfLines) || numberOfLines <= 0)
            {
                throw new Exception("Invalid input. Please enter a positive integer.");
            }

            Console.WriteLine($"Enter the path for the output test file ({defaultFilePath}):");
            var outputFilePath = Console.ReadLine();
            outputFilePath = String.IsNullOrEmpty(outputFilePath) ? defaultFilePath : outputFilePath;

            (new TestFileGenerator()).Run(outputFilePath, numberOfLines);

            return outputFilePath;
        }

        static void Sorter(string inputFilePath)
        {
            if (inputFilePath == "")
            {
                Console.WriteLine($"Enter the path of the input file to sort ({defaultFilePath}):");
                inputFilePath = Console.ReadLine();
                inputFilePath = String.IsNullOrEmpty(inputFilePath) ? defaultFilePath : inputFilePath;
            }

            Console.WriteLine($"Enter the path for the sorted output file ({defaultSortedFilePath}):");
            string outputFilePath = Console.ReadLine();
            outputFilePath = String.IsNullOrEmpty(outputFilePath) ? defaultSortedFilePath : outputFilePath;

            (new ChunkSorter()).Run(inputFilePath, outputFilePath);
        }
    }
}

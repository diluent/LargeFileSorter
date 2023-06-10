using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sorting
{
    public class ChunkSorter
    {
        // Max number of lines to load into memory at once (adjust as needed)
        private const int MaxChunkSize = 4000000;

        // Maximum number of sorting threads (adjust as needed)
        private const int MaxThreads = 8;

        public void Run(string inputFilePath, string outputFilePath)
        {
            Stopwatch stopwatch = new Stopwatch();
            Stopwatch stopwatchChunks = new Stopwatch();
            stopwatch.Start();
            stopwatchChunks.Start();

            string[] chunkFilePaths = CreateSortedChunks(inputFilePath);

            stopwatchChunks.Stop();
            Console.WriteLine($"Time taken for sorting chunks: {stopwatchChunks.Elapsed}");

            Stopwatch stopwatchMerge = new Stopwatch();
            stopwatchMerge.Start();

            Console.WriteLine($"Begin merging sorted chunks");
            (new MergeSorter()).MergeSortedChunks(chunkFilePaths, outputFilePath);

            stopwatchMerge.Stop();
            Console.WriteLine($"Time taken for merge chunks: {stopwatchMerge.Elapsed}");

            DeleteChunks(chunkFilePaths);

            stopwatch.Stop();
            Console.WriteLine("Sorting complete.");
            Console.WriteLine($"Time taken: {stopwatch.Elapsed}");
        }

        string[] CreateSortedChunks(string inputFilePath)
        {
            List<string> chunkFilePaths = new List<string>();
            int numThreads = Math.Min(MaxThreads, Environment.ProcessorCount);
            List<Task> sortingTasks = new List<Task>(numThreads);
            SemaphoreSlim semaphore = new SemaphoreSlim(numThreads);

            long fileSizeInBytes = new FileInfo(inputFilePath).Length;
            double fileSizeInMB = fileSizeInBytes / (1024.0 * 1024.0);
            Console.WriteLine($"Sorting file {fileSizeInMB:F2}Mb");

            using (StreamReader reader = new StreamReader(inputFilePath))
            {
                List<string> lines = new List<string>(MaxChunkSize);
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);

                    if (lines.Count >= MaxChunkSize || (reader.EndOfStream && lines.Count > 0))
                    {
                        var linesStash = lines;
                        lines = new List<string>(MaxChunkSize);
                        var chunkNumber = sortingTasks.Count;
                        var chunkFilePath = $"{inputFilePath}.chunk{chunkNumber}";
                        chunkFilePaths.Add(chunkFilePath);

                        semaphore.Wait();

                        Task sortingTask = Task.Run(() => {
                            SortAndWriteChunk(linesStash, chunkFilePath, chunkNumber);
                            semaphore.Release();
                        });
                        sortingTasks.Add(sortingTask);
                    }
                }
            }

            Task.WaitAll(sortingTasks.ToArray());

            return chunkFilePaths.ToArray();
        }

        void SortAndWriteChunk(List<string> lines, string chunkFilePath, int chunkNumber)
        {
            Console.WriteLine($"Begin sorting chunk {chunkNumber} - {lines.Count} lines");

            lines.Sort(new LineComparer());

            using (StreamWriter writer = new StreamWriter(chunkFilePath))
            {
                for (var i = 0; i < lines.Count; i++)
                {
                    writer.WriteLine(lines[i]);
                }
            }

            Console.WriteLine($"Complete sorting chunk {chunkNumber}");
        }

        void DeleteChunks(string[] chunkFilePaths)
        {
            foreach (string chunkFilePath in chunkFilePaths)
            {
                File.Delete(chunkFilePath);
            }
        }
    }
}

using System.Collections.Generic;
using System.IO;

class MergeSorter
{
    public void MergeSortedChunks(string[] chunkFilePaths, string outputFilePath)
    {
        List<ChunkReader> chunkReaders = new List<ChunkReader>();

        foreach (string chunkFilePath in chunkFilePaths)
        {
            StreamReader reader = new StreamReader(chunkFilePath);
            string line = reader.ReadLine();

            if (line != null)
            {
                chunkReaders.Add(new ChunkReader(reader, line));
            }
        }

        using (StreamWriter writer = new StreamWriter(outputFilePath))
        {
            while (chunkReaders.Count > 0)
            {
                int minChunkReaderIndex = GetMinIndex(chunkReaders);

                writer.WriteLine(chunkReaders[minChunkReaderIndex].line);

                string line = chunkReaders[minChunkReaderIndex].reader.ReadLine();

                if (line == null)
                {
                    chunkReaders[minChunkReaderIndex].reader.Close();
                    chunkReaders[minChunkReaderIndex].reader.Dispose();
                    chunkReaders.RemoveAt(minChunkReaderIndex);
                }
                else
                {
                    chunkReaders[minChunkReaderIndex].SetValues(line);
                }
            }
        }
    }

    int GetMinIndex(List<ChunkReader> chunkReaders)
    {
        int minChunkReaderIndex = 0;
        for (int i = 1; i < chunkReaders.Count; i++)
        {
            if (chunkReaders[i].Compare(chunkReaders[minChunkReaderIndex]) < 0)
            {
                minChunkReaderIndex = i;
            }
        }

        return minChunkReaderIndex;
    }
}

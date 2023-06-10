using NUnit.Framework;
using Sorting;
using System.IO;
using System.Linq;

public class ChunkSorterTest
{
    private const string InputFilePath = "input.txt";
    private const string OutputFilePath = "output.txt";

    private string[] mockUnsortedLines = {
        "415. Apple",
        "30432. Something something something",
        "1. Apple",
        "32. Cherry is the best",
        "2. Banana is yellow"
    };

    private string[] mockSortedLines = {
        "1. Apple",
        "415. Apple",
        "2. Banana is yellow",
        "32. Cherry is the best",
        "30432. Something something something"
    };

    [SetUp]
    public void Setup()
    {
        CreateInputFile();
    }

    [TearDown]
    public void TearDown()
    {
        // Delete the input and output files after testing
        File.Delete(InputFilePath);
        File.Delete(OutputFilePath);
    }

    [Test]
    public void ChunkSorter_ValidInputFile_OutputFileSorted()
    {
        // Arrange
        string[] expectedLines = mockSortedLines;

        // Act
        (new ChunkSorter()).Run(InputFilePath, OutputFilePath);

        // Assert
        string[] actualLines = File.ReadAllLines(OutputFilePath);
        CollectionAssert.AreEqual(mockSortedLines, actualLines);
    }

    [Test]
    public void ChunkSorter_EmptyInputFile_OutputFileEmpty()
    {
        // Arrange
        File.WriteAllText(InputFilePath, string.Empty);

        // Act
        (new ChunkSorter()).Run(InputFilePath, OutputFilePath);

        // Assert
        string[] actualLines = File.ReadAllLines(OutputFilePath);
        Assert.IsEmpty(actualLines);
    }

    [Test]
    public void ChunkSorter_SingleLineInputFile_OutputFileContainsSingleLine()
    {
        // Arrange
        const string line = "1. Apple";
        File.WriteAllText(InputFilePath, line);

        // Act
        (new ChunkSorter()).Run(InputFilePath, OutputFilePath);

        // Assert
        string[] actualLines = File.ReadAllLines(OutputFilePath);
        Assert.AreEqual(1, actualLines.Length);
        Assert.AreEqual(line, actualLines[0]);
    }

    private void CreateInputFile()
    {
        File.WriteAllLines(InputFilePath, mockUnsortedLines);
    }

}
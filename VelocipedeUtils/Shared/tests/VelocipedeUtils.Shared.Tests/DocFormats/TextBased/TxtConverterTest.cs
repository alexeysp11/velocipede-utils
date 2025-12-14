using System.Reflection;
using Xunit;
using VelocipedeUtils.Shared.Office.DocFormats.TextBased;
using VelocipedeUtils.Shared.Models.Documents;
using VelocipedeUtils.Shared.Models.Documents.Enums;

namespace VelocipedeUtils.Shared.Tests.DocFormats.TextBased;

public sealed class TxtConverterTest
{
    private static string FolderName
        => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", typeof(TxtConverterTest).ToString().Split('.').Last());

    [Fact]
    public void TextDocElementsToDocument_CorrectParams_FileExists()
    {
        // Arrange
        string filename = MethodBase.GetCurrentMethod()?.Name + ".txt";
        string filepath = Path.Combine(FolderName, filename);
        List<TextDocElement> elements =
        [
            new() 
            {
                Content = "Header 1", 
                FontSize = 50, 
                TextAlignment = TextAlignment.CENTER
            }, 
            new() 
            {
                Content = "Paragraph 1\nLet's print out some content to the paragraph...", 
                FontSize = 14, 
                TextAlignment = TextAlignment.LEFT
            }, 
            new() 
            {
                Content = "Header 2", 
                FontSize = 50, 
                TextAlignment = TextAlignment.CENTER
            }, 
            new() 
            {
                Content = "Paragraph 2\nLet's print out again some content to the paragraph...", 
                FontSize = 14, 
                TextAlignment = TextAlignment.JUSTIFIED
            }
        ];

        TxtConverter converter = new();
        CreateFolderIfNotExists(FolderName);

        // Act
        converter.TextDocElementsToDocument(FolderName, filename, elements);

        // Assert
        Assert.True(File.Exists(filepath));
    }

    [Fact]
    public void ConvertFileToTde_CorrectParams_FileExists()
    {
        // Arrange
        string filename = MethodBase.GetCurrentMethod()?.Name + ".txt";
        string copyFilename = filename.Replace(".txt", "_copy.txt");
        string filepath = Path.Combine(FolderName, filename);
        string copyFilePath = Path.Combine(FolderName, copyFilename);
        List<TextDocElement> elements =
        [
            new() 
            {
                Content = "Header 1", 
                FontSize = 50, 
                TextAlignment = TextAlignment.CENTER
            }, 
            new() 
            {
                Content = "Paragraph 1\nLet's print out some content to the paragraph...", 
                FontSize = 14, 
                TextAlignment = TextAlignment.LEFT
            }, 
            new() 
            {
                Content = "Header 2", 
                FontSize = 50, 
                TextAlignment = TextAlignment.CENTER
            }, 
            new() 
            {
                Content = "Paragraph 2\nLet's print out again some content to the paragraph...", 
                FontSize = 14, 
                TextAlignment = TextAlignment.JUSTIFIED
            }
        ];

        TxtConverter converter = new();
        CreateFolderIfNotExists(FolderName);

        // Act
        converter.TextDocElementsToDocument(FolderName, filename, elements);
        List<TextDocElement> tmpElements = TxtConverter.ConvertFileToTde(FolderName, filename);
        converter.TextDocElementsToDocument(FolderName, copyFilename, tmpElements);

        // Assert
        Assert.True(File.Exists(filepath));
        Assert.True(File.Exists(copyFilePath));
        // Assert.True(File.ReadAllText(filepath).Equals(File.ReadAllText(copyFilePath)));
    }

    #region Private methods
    private static void CreateFolderIfNotExists(string foldername)
    {
        if (!Directory.Exists(foldername)) Directory.CreateDirectory(foldername);
    }
    #endregion  // Private methods
}
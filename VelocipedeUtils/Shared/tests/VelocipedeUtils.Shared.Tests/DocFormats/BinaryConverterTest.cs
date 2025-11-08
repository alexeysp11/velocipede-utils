using System.Reflection;
using Xunit;
using VelocipedeUtils.Shared.Office.DocFormats;
using VelocipedeUtils.Shared.Models.Documents;
using VelocipedeUtils.Shared.Models.Documents.Enums;

namespace VelocipedeUtils.Shared.Tests.Office.DocFormats;

public sealed class BinaryConverterTest
{
    private static string FolderName
        => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()?.Location) ?? "", typeof(BinaryConverterTest).ToString().Split('.').Last());

    [Fact]
    public void SaveAsBinaryFile_CorrectParams_FileExists()
    {
        // Arrange
        string filename = MethodBase.GetCurrentMethod()?.Name + ".pdf";
        string filepath = Path.Combine(FolderName, filename);
        string binFile = filepath.Replace(".pdf", ".bin");
        string copyFile1 = filepath.Replace(".pdf", "_copy_1.pdf");
        string copyFile2 = filepath.Replace(".pdf", "_copy_2.pdf");
        List<TextDocElement> elements =
        [
            new TextDocElement() 
            {
                Content = "Header 1", 
                FontSize = 50, 
                TextAlignment = TextAlignment.CENTER
            }, 
            new TextDocElement() 
            {
                Content = "Paragraph 1\nLet's print out some content to the paragraph...", 
                FontSize = 14, 
                TextAlignment = TextAlignment.LEFT
            }, 
            new TextDocElement() 
            {
                Content = "Header 2", 
                FontSize = 50, 
                TextAlignment = TextAlignment.CENTER
            }, 
            new TextDocElement() 
            {
                Content = "Paragraph 2\nLet's print out again some content to the paragraph...", 
                FontSize = 14, 
                TextAlignment = TextAlignment.JUSTIFIED
            }
        ];

        PdfConverter pdfConverter = new();
        CreateFolderIfNotExists(FolderName);

        // Act
        pdfConverter.TextDocElementsToDocument(FolderName, filename, elements);
        byte[] bytes = BinaryConverter.GetBinaryFile(filepath);
        BinaryConverter.SaveAsBinaryFile(binFile, bytes);
        BinaryConverter.SaveAsBinaryFile(copyFile1, bytes);
        BinaryConverter.SaveAsBinaryFile(copyFile2, BinaryConverter.GetBinaryFile(binFile));

        // Assert
        Assert.True(File.Exists(filepath));
        Assert.True(File.Exists(binFile));
        Assert.True(File.Exists(copyFile1));
        Assert.True(File.Exists(copyFile2));
    }

    private static void CreateFolderIfNotExists(string foldername)
    {
        if (!Directory.Exists(foldername))
            Directory.CreateDirectory(foldername);
    }
}

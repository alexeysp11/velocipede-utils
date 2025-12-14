using System.Reflection;
using Xunit;
using VelocipedeUtils.Shared.Office.DocFormats.Spreadsheets;
using VelocipedeUtils.Shared.Models.Documents;
using VelocipedeUtils.Shared.Models.Documents.Enums;

namespace VelocipedeUtils.Shared.Tests.DocFormats.Spreadsheets;

public sealed class MSExcelConverterTest
{
    private static string FolderName
        => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()?.Location) ?? "", typeof(MSExcelConverterTest).ToString().Split('.').Last());

    [Fact]
    public void SpreadsheetElementsToDocument_CorrectParams_FileExists()
    {
        // Arrange
        string filename = MethodBase.GetCurrentMethod()?.Name + ".xlsx";
        string filepath = Path.Combine(FolderName, filename);
        uint worksheetId = 1;
        string worksheetName = "TestSheet";
        List<SpreadsheetElement> elements =
        [
            new() 
            {
                CellName = "A1",
                TextDocElement = new TextDocElement 
                {
                    Content = "First test header", 
                    FontSize = 14, 
                    TextAlignment = TextAlignment.CENTER
                }
            }, 
            new() 
            {
                CellName = "A2",
                TextDocElement = new TextDocElement 
                {
                    Content = "Header 2", 
                    FontSize = 14, 
                    TextAlignment = TextAlignment.CENTER
                }
            }
        ];

        MSExcelConverter converter = new();
        CreateFolderIfNotExists(FolderName);

        // Act
        converter.SpreadsheetElementsToDocument(FolderName, filename, worksheetId, worksheetName, elements);

        // Assert
        Assert.True(File.Exists(filepath));
    }

    [Theory]
    [InlineData(1, "OnlyPositive", "12", "24")]
    public void CalculateSumOfCellRange_CorrectParams_FileExists(
        uint worksheetId, 
        string worksheetName, 
        string content1, 
        string content2)
    {
        // Arrange
        string filename = MethodBase.GetCurrentMethod()?.Name + ".xlsx";
        string filepath = Path.Combine(FolderName, filename);
        string firstCellName = "A1";
        string lastCellName = "A2";
        string resultCell = "A3";
        List<SpreadsheetElement> elements =
        [
            new() 
            {
                CellName = firstCellName,
                TextDocElement = new TextDocElement 
                {
                    Content = content1, 
                    FontSize = 14, 
                    TextAlignment = TextAlignment.CENTER
                }
            }, 
            new() 
            {
                CellName = lastCellName,
                TextDocElement = new TextDocElement 
                {
                    Content = content2, 
                    FontSize = 14, 
                    TextAlignment = TextAlignment.CENTER
                }
            }
        ];

        MSExcelConverter converter = new();
        CreateFolderIfNotExists(FolderName);

        // Act
        converter.SpreadsheetElementsToDocument(FolderName, filename, worksheetId, worksheetName, elements);
        converter.CalculateSumOfCellRange(filepath, worksheetName, firstCellName, lastCellName, resultCell);

        // Assert
        Assert.True(File.Exists(filepath));
    }

    private static void CreateFolderIfNotExists(string foldername)
    {
        if (!Directory.Exists(foldername))
            Directory.CreateDirectory(foldername);
    }
}

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Data;
using Xunit;
using WorkflowLib.Shared.Office.DocFormats.Spreadsheets;
using WorkflowLib.Shared.Models.Documents;
using WorkflowLib.Shared.Models.Documents.Enums;

namespace Cims.Tests.WorkflowLib.Shared.Office.DocFormats.Spreadsheets
{
    public class MSExcelConverterTest
    {
        private string FolderName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), typeof(MSExcelConverterTest).ToString().Split('.').Last());

        [Fact]
        public void SpreadsheetElementsToDocument_CorrectParams_FileExists()
        {
            // Arrange
            string filename = System.Reflection.MethodBase.GetCurrentMethod().Name + ".xlsx";
            string filepath = Path.Combine(FolderName, filename);
            uint worksheetId = 1;
            string worksheetName = "TestSheet";
            var elements = new System.Collections.Generic.List<SpreadsheetElement>()
            {
                new SpreadsheetElement() 
                {
                    CellName = "A1",
                    TextDocElement = new TextDocElement 
                    {
                        Content = "First test header", 
                        FontSize = 14, 
                        TextAlignment = TextAlignment.CENTER
                    }
                }, 
                new SpreadsheetElement() 
                {
                    CellName = "A2",
                    TextDocElement = new TextDocElement 
                    {
                        Content = "Header 2", 
                        FontSize = 14, 
                        TextAlignment = TextAlignment.CENTER
                    }
                }
            };

            ISpreadsheets converter = new MSExcelConverter();
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
            string filename = System.Reflection.MethodBase.GetCurrentMethod().Name + ".xlsx";
            string filepath = Path.Combine(FolderName, filename);
            string firstCellName = "A1";
            string lastCellName = "A2";
            string resultCell = "A3";
            var elements = new System.Collections.Generic.List<SpreadsheetElement>()
            {
                new SpreadsheetElement() 
                {
                    CellName = firstCellName,
                    TextDocElement = new TextDocElement 
                    {
                        Content = content1, 
                        FontSize = 14, 
                        TextAlignment = TextAlignment.CENTER
                    }
                }, 
                new SpreadsheetElement() 
                {
                    CellName = lastCellName,
                    TextDocElement = new TextDocElement 
                    {
                        Content = content2, 
                        FontSize = 14, 
                        TextAlignment = TextAlignment.CENTER
                    }
                }
            };

            ISpreadsheets converter = new MSExcelConverter();
            CreateFolderIfNotExists(FolderName);

            // Act
            converter.SpreadsheetElementsToDocument(FolderName, filename, worksheetId, worksheetName, elements);
            converter.CalculateSumOfCellRange(filepath, worksheetName, firstCellName, lastCellName, resultCell);

            // Assert
            Assert.True(File.Exists(filepath));
        }

        #region Private methods
        private void CreateFolderIfNotExists(string foldername)
        {
            if (!Directory.Exists(foldername)) Directory.CreateDirectory(foldername);
        }
        #endregion  // Private methods
    }
}

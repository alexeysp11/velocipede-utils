using System.Reflection;
using Xunit;
using VelocipedeUtils.Shared.Office.DocFormats.TextBased;
using VelocipedeUtils.Shared.Models.Documents;
using VelocipedeUtils.Shared.Models.Documents.Enums;

namespace VelocipedeUtils.Shared.Tests.Office.DocFormats.TextBased
{
    public sealed class MSWordConverterTest
    {
        private static string FolderName
            => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()?.Location) ?? "", typeof(MSWordConverterTest).ToString().Split('.').Last());

        [Fact]
        public void TextDocElementsToDocument_CorrectParams_FileExists()
        {
            // Arrange
            string filename = MethodBase.GetCurrentMethod()?.Name + ".doc";
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

            MSWordConverter converter = new();
            CreateFolderIfNotExists(FolderName);

            // Act
            converter.TextDocElementsToDocument(FolderName, filename, elements);

            // Assert
            Assert.True(File.Exists(filepath));
        }

        [Fact(Skip = "Skip for now")]
        public void ConvertToPdf_CorrectParams_FileExists()
        {
            // Arrange.
            string? methodName = MethodBase.GetCurrentMethod()?.Name;
            string wordFilename = methodName + ".doc";
            string pdfFilename = methodName + ".pdf";
            string wordFilepath = Path.Combine(FolderName, wordFilename);
            string pdfFilepath = Path.Combine(FolderName, pdfFilename);
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

            MSWordConverter converter = new();
            CreateFolderIfNotExists(FolderName);

            // Act
            converter.TextDocElementsToDocument(FolderName, wordFilename, elements);
            //converter.ConvertToPdf(FolderName, wordFilename, pdfFilename);

            // Assert
            Assert.True(File.Exists(wordFilepath));
            Assert.True(File.Exists(pdfFilepath));
        }

        #region Private methods
        private static void CreateFolderIfNotExists(string foldername)
        {
            if (!Directory.Exists(foldername)) Directory.CreateDirectory(foldername);
        }
        #endregion  // Private methods
    }
}
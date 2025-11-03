using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;
using VelocipedeUtils.Shared.Office.DocFormats;
using VelocipedeUtils.Shared.Models.Documents;
using VelocipedeUtils.Shared.Models.Documents.Enums;

namespace Cims.Tests.VelocipedeUtils.Shared.Office.DocFormats
{
    public class PdfConverterTest
    {
        private readonly string FolderName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), typeof(PdfConverterTest).ToString().Split('.').Last());

        [Fact]
        public void TextDocElementsToDocument_CorrectParams_FileExists()
        {
            // Arrange
            string filename = System.Reflection.MethodBase.GetCurrentMethod().Name + ".pdf";
            var elements = new System.Collections.Generic.List<TextDocElement>()
            {
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
            };

            PdfConverter converter = new PdfConverter();
            CreateFolderIfNotExists(FolderName);

            // Act
            converter.TextDocElementsToDocument(FolderName, filename, elements);
            string filepath = Path.Combine(FolderName, filename);

            // Assert
            Assert.True(File.Exists(filepath));
        }

        #region Private methods
        private static void CreateFolderIfNotExists(string foldername)
        {
            if (!Directory.Exists(foldername)) Directory.CreateDirectory(foldername);
        }
        #endregion  // Private methods
    }
}
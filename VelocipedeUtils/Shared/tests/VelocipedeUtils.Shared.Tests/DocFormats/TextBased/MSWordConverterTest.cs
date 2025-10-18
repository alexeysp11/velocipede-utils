using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Data;
using Xunit;
using VelocipedeUtils.Shared.Office.DocFormats.TextBased;
using VelocipedeUtils.Shared.Models.Documents;
using VelocipedeUtils.Shared.Models.Documents.Enums;

namespace Cims.Tests.VelocipedeUtils.Shared.Office.DocFormats.TextBased
{
    public class MSWordConverterTest
    {
        private string FolderName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), typeof(MSWordConverterTest).ToString().Split('.').Last());

        [Fact]
        public void TextDocElementsToDocument_CorrectParams_FileExists()
        {
            // Arrange
            string filename = System.Reflection.MethodBase.GetCurrentMethod().Name + ".doc";
            string filepath = Path.Combine(FolderName, filename);
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

            IWorkflowTextBased converter = new MSWordConverter();
            CreateFolderIfNotExists(FolderName);

            // Act
            converter.TextDocElementsToDocument(FolderName, filename, elements);

            // Assert
            Assert.True(File.Exists(filepath));
        }

        // [Fact]
        // public void ConvertToPdf_CorrectParams_FileExists()
        // {
        //     // Arrange
        //     string wordFilename = System.Reflection.MethodBase.GetCurrentMethod().Name + ".doc";
        //     string pdfFilename = System.Reflection.MethodBase.GetCurrentMethod().Name + ".pdf";
        //     string wordFilepath = Path.Combine(FolderName, wordFilename);
        //     string pdfFilepath = Path.Combine(FolderName, pdfFilename);
        //     var elements = new System.Collections.Generic.List<TextDocElement>()
        //     {
        //         new TextDocElement() 
        //         {
        //             Content = "Header 1", 
        //             FontSize = 50, 
        //             TextAlignment = TextAlignment.CENTER
        //         }, 
        //         new TextDocElement() 
        //         {
        //             Content = "Paragraph 1\nLet's print out some content to the paragraph...", 
        //             FontSize = 14, 
        //             TextAlignment = TextAlignment.LEFT
        //         }, 
        //         new TextDocElement() 
        //         {
        //             Content = "Header 2", 
        //             FontSize = 50, 
        //             TextAlignment = TextAlignment.CENTER
        //         }, 
        //         new TextDocElement() 
        //         {
        //             Content = "Paragraph 2\nLet's print out again some content to the paragraph...", 
        //             FontSize = 14, 
        //             TextAlignment = TextAlignment.JUSTIFIED
        //         }
        //     };

        //     MSWordConverter converter = new MSWordConverter();
        //     CreateFolderIfNotExists(FolderName);

        //     // Act
        //     converter.TextDocElementsToDocument(FolderName, wordFilename, elements);
        //     converter.ConvertToPdf(FolderName, wordFilename, pdfFilename);

        //     // Assert
        //     Assert.True(File.Exists(wordFilepath));
        //     Assert.True(File.Exists(pdfFilepath));
        // }

        #region Private methods
        private void CreateFolderIfNotExists(string foldername)
        {
            if (!Directory.Exists(foldername)) Directory.CreateDirectory(foldername);
        }
        #endregion  // Private methods
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;
using VelocipedeUtils.Shared.Office.DocFormats.TextBased;
using VelocipedeUtils.Shared.Models.Documents;
using VelocipedeUtils.Shared.Models.Documents.Enums;

namespace Cims.Tests.VelocipedeUtils.Shared.Office.DocFormats.TextBased
{
    public class TxtConverterTest
    {
        private string FolderName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), typeof(TxtConverterTest).ToString().Split('.').Last());

        [Fact]
        public void TextDocElementsToDocument_CorrectParams_FileExists()
        {
            // Arrange
            string filename = System.Reflection.MethodBase.GetCurrentMethod().Name + ".txt";
            string filepath = Path.Combine(FolderName, filename);
            var elements = new List<TextDocElement>()
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

            IWorkflowTextBased converter = new TxtConverter();
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
            string filename = System.Reflection.MethodBase.GetCurrentMethod().Name + ".txt";
            string copyFilename = filename.Replace(".txt", "_copy.txt");
            string filepath = Path.Combine(FolderName, filename);
            string copyFilePath = Path.Combine(FolderName, copyFilename);
            var elements = new List<TextDocElement>()
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

            var converter = new TxtConverter();
            CreateFolderIfNotExists(FolderName);

            // Act
            converter.TextDocElementsToDocument(FolderName, filename, elements);
            var tmpElements = converter.ConvertFileToTde(FolderName, filename);
            converter.TextDocElementsToDocument(FolderName, copyFilename, tmpElements);

            // Assert
            Assert.True(File.Exists(filepath));
            Assert.True(File.Exists(copyFilePath));
            // Assert.True(File.ReadAllText(filepath).Equals(File.ReadAllText(copyFilePath)));
        }

        #region Private methods
        private void CreateFolderIfNotExists(string foldername)
        {
            if (!Directory.Exists(foldername)) Directory.CreateDirectory(foldername);
        }
        #endregion  // Private methods
    }
}
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;
using VelocipedeUtils.Shared.Office.DocFormats.Images;

namespace Cims.Tests.VelocipedeUtils.Shared.Office.DocFormats.Images
{
    public class PngConverterTest
    {
        private readonly string Text = "Hello,_world! 123;532.52,642'2332\"w342\\432/243^w\n(test&something#1@ok)+$32.5~tt`qwerty\ttabulated\n\nTest text was written!";
        private readonly string FolderName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), typeof(PngConverterTest).ToString().Split('.').Last());

        #region TextToImg
        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void TextToImg_OneOrMoreStringParamsEmpty_ReturnsException(bool isTextEmpty, bool isFoldernameEmpty, bool isFilenameEmpty)
        {
            // Arrange 
            string text = isTextEmpty ? string.Empty : Text;
            string foldername = isFoldernameEmpty ? string.Empty : FolderName;
            string filename = isFilenameEmpty ? string.Empty : System.Reflection.MethodBase.GetCurrentMethod().Name + ".png";

            IImageConverter converter = new PngConverter();
            if (!string.IsNullOrEmpty(foldername)) CreateFolderIfNotExists(FolderName);

            // Act 
            Action act = () => converter.TextToImg(text, foldername, filename);

            // Assert 
            System.Exception exception = Assert.Throws<System.Exception>(act);
        }

        [Fact]
        public void TextToImg_IncorrectFoldername_ReturnsException()
        {
            // Arrange 
            string foldername = "incorrect path";
            string filename = System.Reflection.MethodBase.GetCurrentMethod().Name + ".png";

            IImageConverter converter = new PngConverter();

            // Act 
            Action act = () => converter.TextToImg(Text, foldername, filename);

            // Assert 
            System.Exception exception = Assert.Throws<System.Exception>(act);
        }

        [Fact]
        public void TextToImg_IncorrectFileExtenstion_ReturnsException()
        {
            // Arrange 
            string filename = System.Reflection.MethodBase.GetCurrentMethod().Name + ".jpg";

            IImageConverter converter = new PngConverter();
            CreateFolderIfNotExists(FolderName);

            // Act 
            Action act = () => converter.TextToImg(Text, FolderName, filename);

            // Assert 
            System.Exception exception = Assert.Throws<System.Exception>(act);
        }

        [Fact]
        public void TextToImg_CorrectParameters_FileExists()
        {
            // Arrange
            string filename = System.Reflection.MethodBase.GetCurrentMethod().Name + ".png";

            IImageConverter converter = new PngConverter();
            CreateFolderIfNotExists(FolderName);

            // Act
            converter.TextToImg(Text, FolderName, filename);
            string filepath = Path.Combine(FolderName, filename);

            // Assert
            Assert.True(File.Exists(filepath));
        }
        #endregion  // TextToImg

        #region Private methods
        private static void CreateFolderIfNotExists(string foldername)
        {
            if (!Directory.Exists(foldername)) Directory.CreateDirectory(foldername);
        }
        #endregion  // Private methods
    }
}
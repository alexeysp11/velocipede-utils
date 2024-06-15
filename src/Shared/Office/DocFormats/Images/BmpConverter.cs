using System.IO; 
using System.Drawing; 
using System.Linq; 

namespace WorkflowLib.Shared.Office.DocFormats.Images
{
    /// <summary>
    /// Class for converting text into a BMP image.
    /// </summary>
    public class BmpConverter : BaseImageConverter, IImageConverter
    {
        /// <summary>
        /// Method for converting text into an image.
        /// </summary>
        public void TextToImg(string text, string foldername, string filename)
        {
#if NET5_0_OR_GREATER
            if (!System.OperatingSystem.IsWindows())
#else
            if (System.Environment.OSVersion.Platform != System.PlatformID.Win32NT 
                && System.Environment.OSVersion.Platform != System.PlatformID.Win32S
                && System.Environment.OSVersion.Platform != System.PlatformID.Win32Windows
                && System.Environment.OSVersion.Platform != System.PlatformID.WinCE)
#endif
                throw new System.NotSupportedException("The method is available only on Windows OS");
            
            base.CheckText(text); 
            base.CheckFolderName(foldername); 
            base.CheckFileName(filename, "bmp"); 

            // 
            MemoryStream ms = new MemoryStream();            
            System.Drawing.Image img = new Bitmap(200, 30, System.Drawing.Imaging.PixelFormat.Format64bppArgb);
            Graphics drawing = Graphics.FromImage(img);
            Font font = new Font("Arial", 30, FontStyle.Bold, GraphicsUnit.Pixel);

            SizeF textSize = drawing.MeasureString(text, font);
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width + 30, (int)textSize.Height + 40);
            drawing = Graphics.FromImage(img);
            Color backColor = Color.White;
            Color textColor = Color.FromArgb(1, 119, 245);

            //textColor. = "#0177F5";

            //paint the background
            drawing.Clear(backColor);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);
            drawing.DrawString(text, font, textBrush, 40, 20);
            drawing.Save();
            font.Dispose();
            textBrush.Dispose();
            drawing.Dispose();

            string filepath = Path.Combine(foldername, filename); 
            img.Save(filepath, System.Drawing.Imaging.ImageFormat.Bmp);
            img.Dispose();
        }

        /// <summary>
        /// Method for converting binary to image.
        /// </summary>
        public void BinaryToImg(byte[] bytes, string foldername, string filename)
        {
            base.CheckFolderName(foldername); 
            base.CheckFileName(filename, "bmp"); 

            // 
        }

        /// <summary>
        /// Method for converting image into binary 
        /// </summary>
        public byte[] ImgToBinary(string foldername, string filename)
        {
            // 
            base.CheckFolderName(foldername); 
            base.CheckFileName(filename, "bin"); 

            return new byte[1]; 
        }
    }
}
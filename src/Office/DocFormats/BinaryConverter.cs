using System.IO; 

namespace WorkflowLib.Office.DocFormats
{
    /// <summary>
    /// Class for using banaries (binary converter)
    /// </summary>
    public class BinaryConverter
    {
        /// <summary>
        /// Saves array of bytes as a binary file.
        /// </summary>
        public void SaveAsBinaryFile(string filename, byte[] bytes)
        {
            File.WriteAllBytes(filename, bytes); 
        }
        /// <summary>
        /// Reads all bytes from a specified file.
        /// </summary>
        public byte[] GetBinaryFile(string filename)
        {
            byte[] bytes;
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
            }
            return bytes;
        }
    }
}
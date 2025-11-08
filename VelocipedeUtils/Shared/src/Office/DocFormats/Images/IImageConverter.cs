namespace VelocipedeUtils.Shared.Office.DocFormats.Images;

/// <summary>
/// Interface for image converter.
/// </summary>
public interface IImageConverter
{
    /// <summary>
    /// Method for converting text i an to binary image.
    /// </summary>
    void TextToImg(string text, string foldername, string filename);

    /// <summary>
    /// Method for converting binary to image.
    /// </summary>
    void BinaryToImg(byte[] bytes, string foldername, string filename);

    /// <summary>
    /// Method for converting image to binary.
    /// </summary>
    byte[] ImgToBinary(string foldername, string filename);
}
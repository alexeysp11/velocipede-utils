using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using VelocipedeUtils.Shared.Models.Documents;
using VelocipedeUtils.Shared.Office.DocFormats.TextBased;

namespace VelocipedeUtils.Shared.Office.DocFormats;

/// <summary>
/// Class for using PDF (PDF converter)
/// </summary>
public class PdfConverter : IWorkflowTextBased
{
    /// <summary>
    /// Method for converting a list of TextDocElement into TXT document.
    /// </summary>
    public void TextDocElementsToDocument(string foldername, string filename, List<TextDocElement> elements)
    {
        if (!Directory.Exists(foldername))
            throw new Exception("Folder name does not exist");
        if (string.IsNullOrEmpty(filename))
            throw new Exception("File name could not be null or empty");
        if (filename.Split('.').Last().ToLower() != "pdf")
            throw new Exception("Incorrect file extension");

        // TODO: Check if the following piece of code catches all the possible exceptions! 
        try
        {
            string filepath = Path.Combine(foldername, filename);
            using (var writer = new PdfWriter(filepath))
            using (PdfDocument pdf = new PdfDocument(writer))
            using (Document doc = new Document(pdf))
            {
                foreach (var element in elements)
                {
                    Paragraph paragraph = new Paragraph(element.Content)
                        .SetTextAlignment((iText.Layout.Properties.TextAlignment)element.TextAlignment)
                        .SetFontSize(element.FontSize);
                    doc.Add(paragraph);
                }
                doc.Close();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    #region Convert to list of TextDocElement 
    /// <summary>
    /// Convert to list of TextDocElement 
    /// </summary>
    public static List<TextDocElement> ConvertFileToTde(string foldername, string filename)
    {
        if (!Directory.Exists(foldername))
            throw new Exception("Folder does not exist");
        if (string.IsNullOrEmpty(filename))
            throw new Exception("File name could not be null or empty");

        return ConvertFileToTde(Path.Combine(foldername, filename));
    }

    /// <summary>
    /// Convert to list of TextDocElement 
    /// </summary>
    public static List<TextDocElement> ConvertFileToTde(string filepath)
    {
        if (string.IsNullOrEmpty(filepath))
            throw new Exception("File name could not be null or empty");
        if (!File.Exists(filepath))
            throw new Exception("File does not exist");

        return ConvertFileToTde(new FileInfo(filepath));
    }

    /// <summary>
    /// Convert to list of TextDocElement 
    /// </summary>
    public static List<TextDocElement> ConvertFileToTde(FileInfo file)
    {
        string content = System.IO.File.ReadAllText(file.FullName);
        if (string.IsNullOrEmpty(content))
            throw new Exception("File content could not be empty");

        return ConvertStringToTde(content);
    }

    /// <summary>
    /// Convert to list of TextDocElement 
    /// </summary>
    public static List<TextDocElement> ConvertStringToTde(string xmlContent)
    {
        if (string.IsNullOrEmpty(xmlContent))
            throw new Exception("XML content could not be empty");

        // 

        return [];
    }
    #endregion  // Convert to list of TextDocElement 
}
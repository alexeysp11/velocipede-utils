using System.Collections.Generic;
using VelocipedeUtils.Shared.Models.Documents;

namespace VelocipedeUtils.Shared.Office.DocFormats.TextBased;

/// <summary>
/// Interface for using text based documents.
/// </summary>
public interface IWorkflowTextBased 
{
    /// <summary>
    /// Method for converting a list of TextDocElement into text-based document.
    /// </summary>
    void TextDocElementsToDocument(string foldername, string filename, List<TextDocElement> elements);
}
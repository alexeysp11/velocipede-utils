using VelocipedeUtils.Shared.Models.Documents.Enums;

namespace VelocipedeUtils.Shared.Models.Documents;

/// <summary>
/// Class for representing an element of text document.
/// </summary>
public class TextDocElement
{
    /// <summary>
    /// Content of an element of text document.
    /// </summary>
    public required string Content { get; set; }
    
    /// <summary>
    /// Font size of an element of text document.
    /// </summary>
    public int FontSize { get; set; }

    /// <summary>
    /// Text alignment of an element of text document.
    /// </summary>
    public TextAlignment TextAlignment { get; set; }
}
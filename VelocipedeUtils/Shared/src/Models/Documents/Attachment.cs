using VelocipedeUtils.Shared.Models.Documents.Enums;

namespace VelocipedeUtils.Shared.Models.Documents;

/// <summary>
/// Attachment to the message.
/// </summary>
public class Attachment
{
    /// <summary>
    /// File name.
    /// </summary>
    public required string FileName { get; set; }
    
    /// <summary>
    /// Extention.
    /// </summary>
    public required string Extention { get; set; }

    /// <summary>
    /// Upload date.
    /// </summary>
    public DateTime UploadDate { get; set; }

    /// <summary>
    /// File.
    /// </summary>
    public required byte[] File { get; set; }

    /// <summary>
    /// Size.
    /// </summary>
    public long Size { get; private set; }

    /// <summary>
    /// File type.
    /// </summary>
    public AttachmentFileType FileType { get; set; }
}
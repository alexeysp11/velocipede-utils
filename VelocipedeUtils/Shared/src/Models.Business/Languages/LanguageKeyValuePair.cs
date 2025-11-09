namespace VelocipedeUtils.Shared.Models.Business.Languages;

/// <summary>
/// Language key-value pair.
/// </summary>
public class LanguageKeyValuePair
{
    /// <summary>
    /// ID of the business entity.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Language type.
    /// </summary>
    public LanguageType LanguageType { get; set; }
    
    /// <summary>
    /// Language key.
    /// </summary>
    public LanguageKey? LanguageKey { get; set; }
    
    /// <summary>
    /// Key of the language key-value pair.
    /// </summary>
    public required string Key { get; set; }
    
    /// <summary>
    /// Value of the language key-value pair.
    /// </summary>
    public required string Value { get; set; }
    
    /// <summary>
    /// UID of the application.
    /// </summary>
    public string? ApplicationUid { get; set; }
    
    /// <summary>
    /// The date when the business entity was created.
    /// </summary>
    public DateTime? DateCreated { get; set; }

    /// <summary>
    /// The date when the business entity was changed.
    /// </summary>
    public DateTime? DateChanged { get; set; }

    /// <summary>
    /// Business entity status.
    /// </summary>
    public BusinessEntityStatus? BusinessEntityStatus { get; set; }
}
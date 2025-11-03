namespace VelocipedeUtils.Ubp.Core.Models;

/// <summary>
/// Represents data retrieved after applying a filter against the inintial dataset.
/// </summary>
public class FilteredRecord<TEntity> where TEntity : class
{
    /// <summary>
    /// Collection of filtered items.
    /// </summary>
    public IEnumerable<TEntity> Value { get; set; }
    
    /// <summary>
    /// Date and time of creating the object (the property is used for the proper deleting old objects).
    /// </summary>
    public System.DateTime DateTimeCreated { get; set; }
    
    /// <summary>
    /// Represents if you can already delete the object from the filtered dataset.
    /// </summary>
    public bool IsReadyForDeleting { get; set; }
}
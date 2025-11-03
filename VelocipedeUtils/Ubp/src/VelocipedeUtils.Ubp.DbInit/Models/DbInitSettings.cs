namespace VelocipedeUtils.Ubp.DbInit.Models;

/// <summary>
/// Settings for initializing the database.
/// </summary>
public class DbInitSettings
{
    /// <summary>
    /// Default connection string.
    /// </summary>
    public string? DefaultConnectionString { get; set; }

    /// <summary>
    /// Connection string.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// Actions that should be performed for initializing the database.
    /// </summary>
    public List<DbInitAction>? Actions { get; set; }
}
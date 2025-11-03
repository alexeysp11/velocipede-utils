namespace VelocipedeUtils.Ubp.DbInit.Models;

/// <summary>
/// Action that should be performed for initializing the database.
/// </summary>
public class DbInitAction
{
    /// <summary>
    /// Name of the action.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Type of the action.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// File path to retrieve the SQL file.
    /// </summary>
    public string? FilePath { get; set; }

    /// <summary>
    /// File path of the csproj.
    /// </summary>
    public string? ProjFile { get; set; }
}
namespace VelocipedeUtils.Shared.Models.Database; 

/// <summary>
/// DB settings.
/// </summary>
public class DbSettings 
{
    /// <summary>
    /// Name of the database provider (e.g. SQLite, PostgreSQL, MySQL, MS SQL, Oracle)
    /// </summary>
    public required string Provider { get; set; }

    /// <summary>
    /// String that contains information aobut a data source (usually a database engine), 
    /// as well as the information necessary to connect to it 
    /// </summary>
    public required string ConnectionString { get; set; }

    /// <summary>
    /// Indicates if ORM is necessary to interact with database 
    /// </summary>
    public bool UseOrm { get; set; }
    
    /// <summary>
    /// Name of the ORM (e.g. EF Core, Dapper)
    /// </summary>
    public required string Orm { get; set; }
}
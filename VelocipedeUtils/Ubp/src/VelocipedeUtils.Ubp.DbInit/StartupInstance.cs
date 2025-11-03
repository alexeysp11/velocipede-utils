using Microsoft.EntityFrameworkCore;
using VelocipedeUtils.Ubp.Core.DbContexts;
using VelocipedeUtils.Ubp.DbInit.Dal;
using VelocipedeUtils.Ubp.DbInit.Models;

namespace VelocipedeUtils.Ubp.DbInit;

/// <summary>
/// Class for standardized launch of an application instance.
/// </summary>
public class StartupInstance : IStartupInstance
{
    /// <summary>
    /// Settings for initializing the database.
    /// </summary>
    private DbInitSettings _settings;

    /// <summary>
    /// Database context.
    /// </summary>
    private UbpDbContext _dbContext;

    public StartupInstance(DbInitSettings settings, UbpDbContext dbContext)
    {
        _settings = settings;
        _dbContext = dbContext;
    }

    /// <summary>
    /// Method for standardized launch of an application instance.
    /// </summary>
    public void Start()
    {
        try
        {
            Console.WriteLine("Start to initialize the database.");

            string? connectionString = _settings?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Connection string is not specified");
            }

            string? defaultConnectionString = _settings?.DefaultConnectionString;
            if (string.IsNullOrEmpty(defaultConnectionString))
            {
                throw new Exception("Default connection string is not specified");
            }

            // Check if the database already exists.
            CheckDbExists(defaultConnectionString, "sql/CheckDbExists.sql");

            // Execute actions.
            if (_settings?.Actions == null || !_settings.Actions.Any())
            {
                throw new Exception("Settings or actions are not defined. Please, edit the appsettings file");
            }
            foreach (DbInitAction action in _settings.Actions)
            {
                Console.WriteLine();
                Console.WriteLine($"Start action: {action.Name}");

                // Execute SQL.
                if (!string.IsNullOrEmpty(action.FilePath))
                {
                    Console.WriteLine($"{action.FilePath}");
                    string sql = ReadSqlFromFile(action.FilePath);
                    PgDatabaseInitDao.ExecuteSqlQuery(
                        action.Name == "Create database" ? defaultConnectionString : connectionString,
                        sql);
                    continue;
                }

                // Process proj file.
                if (!string.IsNullOrEmpty(action.ProjFile))
                {
                    Console.WriteLine($"{action.ProjFile}");
                    ApplyEfCoreMigrations(_dbContext);
                    continue;
                }

                throw new Exception($"Action {action.Name} could not be executed because it does not contain SQL path or csproj file path");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error:");
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            Console.WriteLine();
            Console.WriteLine("Database initialization finished.");
        }
    }

    /// <summary>
    /// Check if the database exists.
    /// </summary>
    /// <param name="connectionString">Connection string</param>
    /// <param name="filepath">Specified file path</param>
    private static void CheckDbExists(string connectionString, string filepath)
    {
        string sql = ReadSqlFromFile(filepath);
        if (PgDatabaseInitDao.CheckDbExists(connectionString, sql))
        {
            throw new Exception("The specified database already exists");
        }
    }

    /// <summary>
    /// Read SQL from the specified file path.
    /// </summary>
    /// <param name="filepath">The specified file path</param>
    /// <returns>SQL query</returns>
    private static string ReadSqlFromFile(string filepath)
    {
        return File.ReadAllText(filepath);
    }

    /// <summary>
    /// Apply EF Core migrations
    /// </summary>
    /// <param name="dbContext">Database context</param>
    private static void ApplyEfCoreMigrations(UbpDbContext dbContext)
    {
        dbContext.Database.Migrate();
    }
}
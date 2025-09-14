using System.Data;
using System.Data.SQLite;
using System.Text;
using Dapper;
using FileMqBroker.MqLibrary.Models;

namespace FileMqBroker.MqLibrary.DAL;

/// <summary>
/// Defines the functionality required for communication with the database at the Data Access Layer (DAL) level 
/// as part of working with a message.
/// </summary>
public class SqliteMessageFileDAL : IMessageFileDAL
{
    #region Private fields
    private object m_obj = new object();
    private readonly string m_connectionString;
    private readonly string m_requestMessageFiles = "RequestMessageFiles";
    private readonly string m_responseMessageFiles = "ResponseMessageFiles";
    private readonly string m_selectAllSQL = "SELECT m.Name, m.HttpMethod, m.HttpPath, m.MessageFileState FROM {0} m ";
    private readonly string m_insertMessageSQL = "INSERT INTO {0} (Name, HttpMethod, HttpPath, MessageFileState) VALUES ";
    private readonly string m_updateOldMessageSQL = "update RequestMessageFiles set MessageFileState = 6 where MessageFileState not in (6, 11, 12) and CreatedAt < datetime('now', '-5 seconds');";
    #endregion  // Private fields

    #region Constructors
    /// <summary>
    /// Default constructor.
    /// </summary>
    public SqliteMessageFileDAL(AppInitConfigs appInitConfigs)
    {
        m_connectionString = appInitConfigs.DbConnectionString;
    }
    #endregion  // Constructors

    #region Public methods
    /// <summary>
    /// Method for inserting the specified files in DB.
    /// </summary>
    public virtual void InsertMessageFileState(IReadOnlyList<MessageFile> fileMessages)
    {
        if (fileMessages == null)
            throw new System.ArgumentNullException(nameof(fileMessages));
        if (fileMessages.Count == 0)
            return;
        
        var sqlQuery = GenerateInsertSqlByFileMessages(fileMessages);

        lock (m_obj)
        {
            using (var connection = new SQLiteConnection(m_connectionString))
            {
                connection.Execute(sqlQuery.Query, sqlQuery.Parameters);
            }
        }
    }

    /// <summary>
    /// Method for updating state of old files.
    /// </summary>
    public virtual void UpdateOldMessageFileState()
    {
        var sqlQuery = m_updateOldMessageSQL;

        lock (m_obj)
        {
            using (var connection = new SQLiteConnection(m_connectionString))
            {
                connection.Execute(sqlQuery);
            }
        }
    }

    /// <summary>
    /// Method for updating state of the specified files.
    /// </summary>
    public virtual void UpdateMessageFileState(IReadOnlyList<MessageFile> fileMessages)
    {
        if (fileMessages == null)
            throw new System.ArgumentNullException(nameof(fileMessages));
        if (fileMessages.Count == 0)
            return;
        
        var sqlQuery = GenerateUpdateSqlByFileNames(fileMessages);

        lock (m_obj)
        {
            using (var connection = new SQLiteConnection(m_connectionString))
            {
                connection.Execute(sqlQuery.Query, sqlQuery.Parameters);
            }
        }
    }

    /// <summary>
    /// Method for obtaining information about files.
    /// </summary>
    public virtual IReadOnlyList<MessageFile> GetMessageFileInfo(int pageSize, int pageNumber, MessageFileType messageFileType)
    {
        var sqlQuery = GenerateSelectSqlReadyToReadFiles(pageSize, pageNumber, messageFileType);

        IReadOnlyList<MessageFile> result;
        lock (m_obj)
        {
            using (var connection = new SQLiteConnection(m_connectionString))
            {
                result = connection.Query<MessageFile>(sqlQuery).ToList();
            }
        }
        return result;
    }
    #endregion  // Public methods

    #region Private methods
    /// <summary>
    /// Method for generating an SQL query for inserting data about a file.
    /// </summary>
    protected (string Query, DynamicParameters Parameters) GenerateInsertSqlByFileMessages(IReadOnlyList<MessageFile> fileMessages)
    {
        var queryParameters = new DynamicParameters();
        var stringBuilder = new StringBuilder();

        for (int i = 0; i < fileMessages.Count; i++)
        {
            var messageFileType = fileMessages[i].MessageFileType;
            stringBuilder.Append(string.Format(m_insertMessageSQL, (messageFileType == MessageFileType.Request ? m_requestMessageFiles : m_responseMessageFiles)));
            stringBuilder.Append($"(@file_{i}_Name, @file_{i}_HttpMethod, @file_{i}_HttpPath, @file_{i}_FileState);");

            queryParameters.Add($"file_{i}_Name", fileMessages[i].Name);
            queryParameters.Add($"file_{i}_HttpMethod", fileMessages[i].HttpMethod);
            queryParameters.Add($"file_{i}_HttpPath", fileMessages[i].HttpPath);
            queryParameters.Add($"file_{i}_FileState", fileMessages[i].MessageFileState);
        }

        return (stringBuilder.ToString(), queryParameters);
    }

    /// <summary>
    /// Method for generating an SQL query for updating data about a file.
    /// </summary>
    protected (string Query, DynamicParameters Parameters) GenerateUpdateSqlByFileNames(IReadOnlyList<MessageFile> fileMessages)
    {
        var queryParameters = new DynamicParameters();
        var stringBuilder = new StringBuilder();

        for (int i = 0; i < fileMessages.Count; i++)
        {
            var table = fileMessages[i].MessageFileType == MessageFileType.Request ? m_requestMessageFiles : m_responseMessageFiles;
            stringBuilder.Append($"UPDATE {table} SET MessageFileState = @MessageFileState_{i} WHERE Name = @Name_{i};");
            
            queryParameters.Add($"MessageFileState_{i}", fileMessages[i].MessageFileState);
            queryParameters.Add($"Name_{i}", fileMessages[i].Name);
        }

        return (stringBuilder.ToString(), queryParameters);
    }

    /// <summary>
    /// Generates an SQL query to retrieve readable files.
    /// </summary>
    protected string GenerateSelectSqlReadyToReadFiles(int pageSize, int pageNumber, MessageFileType messageFileType)
    {
        if (pageSize <= 0)
            throw new System.ArgumentException("Page size should be greater than zero", nameof(pageSize));
        if (pageNumber <= 0)
            throw new System.ArgumentException("Page number should be greater than zero", nameof(pageNumber));
        
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(string.Format(m_selectAllSQL, (messageFileType == MessageFileType.Request ? m_requestMessageFiles : m_responseMessageFiles)));
        stringBuilder.Append(" WHERE m.MessageFileState = 6");
        stringBuilder.Append($" LIMIT {pageSize} OFFSET {pageSize * (pageNumber - 1)};");

        return stringBuilder.ToString();
    }
    #endregion  // Private methods
}
namespace VelocipedeUtils.Shared.DbOperations.Models
{
    /// <summary>
    /// The result of executing the foreach operation for the specified database entities.
    /// </summary>
    public class VelocipedeForeachResult
    {
        /// <summary>
        /// A dictionary representing the result of the foreach operation.
        /// </summary>
        public Dictionary<string, VelocipedeForeachTableInfo>? Result { get; set; }

        /// <summary>
        /// Add result into the collection.
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="info">Table info</param>
        public void Add(string tableName, VelocipedeForeachTableInfo info)
        {
            if (Result == null)
                Result = new Dictionary<string, VelocipedeForeachTableInfo>();

            if (Result.ContainsKey(tableName))
            {
                Result[tableName] = info;
            }
            else
            {
                Result.Add(tableName, info);
            }
        }

        /// <summary>
        /// Remove info about the table from the collection.
        /// </summary>
        /// <param name="tableName">Table name</param>
        public void Remove(string tableName)
        {
            if (Result == null)
                Result = new Dictionary<string, VelocipedeForeachTableInfo>();
            
            if (Result.ContainsKey(tableName))
            {
                Result.Remove(tableName);
            }
        }
    }
}

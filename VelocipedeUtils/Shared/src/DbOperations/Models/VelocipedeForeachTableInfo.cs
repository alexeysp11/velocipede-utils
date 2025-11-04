using System.Data;
using VelocipedeUtils.Shared.DbOperations.DbConnections;

namespace VelocipedeUtils.Shared.DbOperations.Models
{
    /// <summary>
    /// The result of executing the <c>foreach</c> operation for a specific database table.
    /// </summary>
    public class VelocipedeForeachTableInfo
    {
        /// <summary>
        /// Table name.
        /// </summary>
        public required string TableName { get; set; }

        /// <summary>
        /// Data obtained as a result of executing the query 
        /// <see cref="IVelocipedeDbConnectionExtensions.GetAllData(IVelocipedeDbConnection, string, out DataTable)"/>.
        /// </summary>
        public DataTable? Data { get; set; }

        /// <summary>
        /// Column info.
        /// </summary>
        public List<VelocipedeColumnInfo>? ColumnInfo { get; set; }

        /// <summary>
        /// Foreign key info.
        /// </summary>
        public List<VelocipedeForeignKeyInfo>? ForeignKeyInfo { get; set; }

        /// <summary>
        /// Trigger info.
        /// </summary>
        public List<VelocipedeTriggerInfo>? TriggerInfo { get; set; }

        /// <summary>
        /// SQL definition of the table.
        /// </summary>
        public string? SqlDefinition { get; set; }
    }
}

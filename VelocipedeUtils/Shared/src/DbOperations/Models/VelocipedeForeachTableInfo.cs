using System.Data;

namespace VelocipedeUtils.Shared.DbOperations.Models
{
    /// <summary>
    /// The result of executing the foreach operation for a specific database table.
    /// </summary>
    public class VelocipedeForeachTableInfo
    {
        /// <summary>
        /// Table name.
        /// </summary>
        public required string TableName { get; set; }

        /// <summary>
        /// All data in the table.
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

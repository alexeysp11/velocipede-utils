using System.Data;

namespace VelocipedeUtils.Shared.DbOperations.Models
{
    /// <summary>
    /// Parameter used within a SQL command.
    /// </summary>
    public class VelocipedeCommandParameter
    {
        public required string Name { get; set; }
        public object? Value { get; set; }
        public DbType? DbType { get; set; }
        public ParameterDirection? Direction { get; set; }
        public int? Size { get; set; }
        public byte? Precision { get; set; }
        public byte? Scale { get; set; }
    }
}

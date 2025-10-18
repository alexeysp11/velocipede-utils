namespace VelocipedeUtils.Shared.DbOperations.Models
{
    public sealed class VelocipedeColumnInfo
    {
        public int ColumnId { get; set; }
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
        public object DefaultValue { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsNullable { get; set; }
        public bool? IsSelfReferencing { get; set; }
        public bool? IsGenerated { get; set; }
        public bool? IsUpdatable { get; set; }
    }
}

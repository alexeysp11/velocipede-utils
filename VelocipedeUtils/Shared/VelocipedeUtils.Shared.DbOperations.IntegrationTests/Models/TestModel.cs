using System.ComponentModel.DataAnnotations;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.Models
{
    public class TestModel
    {
        [Key]
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? AdditionalInfo { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.Models
{
    public class TestCity
    {
        [Key]
        public int Id { get; set; }

        public required string Name { get; set; }
    }
}

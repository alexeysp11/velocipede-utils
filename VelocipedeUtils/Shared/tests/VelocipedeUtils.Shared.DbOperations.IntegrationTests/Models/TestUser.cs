using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.Models
{
    public class TestUser
    {
        [Key]
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public int? CityId { get; set; }

        [ForeignKey(nameof(CityId))]
        public TestCity? City { get; set; }

        public string? AdditionalInfo { get; set; }
    }
}

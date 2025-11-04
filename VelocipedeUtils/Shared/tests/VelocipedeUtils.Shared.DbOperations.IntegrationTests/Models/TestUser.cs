using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.Models
{
    /// <summary>
    /// A test model representing a user.
    /// </summary>
    public class TestUser
    {
        /// <summary>
        /// Identifier of the user.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Name of the user.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Email of the user.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// The identifier of the city in which the user lives.
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        /// The instance of the city in which the user lives.
        /// </summary>
        [ForeignKey(nameof(CityId))]
        public TestCity? City { get; set; }

        /// <summary>
        /// Additional info about the user.
        /// </summary>
        public string? AdditionalInfo { get; set; }
    }
}

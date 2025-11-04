using System.ComponentModel.DataAnnotations;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.Models
{
    /// <summary>
    /// A test model representing a city.
    /// </summary>
    public class TestCity
    {
        /// <summary>
        /// The identifier of the city.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The name of the city.
        /// </summary>
        public required string Name { get; set; }
    }
}

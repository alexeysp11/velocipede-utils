namespace VelocipedeUtils.Shared.Models.Business.InformationSystem
{
    /// <summary>
    /// Employee performance.
    /// </summary>
    public class EmployeePerformance : WfBusinessEntity, IWfBusinessEntity
    {
        /// <summary>
        /// Managers who rated employee performance.
        /// </summary>
        public required ICollection<Employee> Managers { get; set; }

        /// <summary>
        /// Employee.
        /// </summary>
        public Employee? Employee { get; set; }
        
        /// <summary>
        /// Performance rating.
        /// </summary>
        public int PerformanceRating { get; set; }
        
        /// <summary>
        /// Performance date.
        /// </summary>
        public DateTime PerformanceDate { get; set; }
    }
}
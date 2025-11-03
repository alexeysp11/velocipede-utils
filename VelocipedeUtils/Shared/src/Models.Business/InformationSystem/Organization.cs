using VelocipedeUtils.Shared.Models.Business.BusinessDocuments;
using VelocipedeUtils.Shared.Models.Business.Customers;

namespace VelocipedeUtils.Shared.Models.Business.InformationSystem
{
    /// <summary>
    /// Organization.
    /// </summary>
    public class Organization : WfBusinessEntity, IWfBusinessEntity
    {
        /// <summary>
        /// Company.
        /// </summary>
        public Company? Company { get; set; }
        
        /// <summary>
        /// Head organization item.
        /// </summary>
        public OrganizationItem? HeadItem { get; set; }
        
        /// <summary>
        /// Contracts.
        /// </summary>
        public required ICollection<Contract> Contracts { get; set; }
    }
}
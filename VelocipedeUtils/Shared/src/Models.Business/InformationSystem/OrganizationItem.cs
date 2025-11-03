namespace VelocipedeUtils.Shared.Models.Business.InformationSystem
{
    /// <summary>
    /// Organization item.
    /// </summary>
    public class OrganizationItem : WfBusinessEntity, IWfBusinessEntity
    {
        /// <summary>
        /// Type of the organization item.
        /// </summary>
        public OrganizationItemType ItemType { get; set; }
        
        /// <summary>
        /// Boolean variable that shows if the organization item is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
        
        /// <summary>
        /// Boolean variable that shows if the organization item could not be deleted automatically.
        /// </summary>
        public bool HardDelete { get; set; }
        
        /// <summary>
        /// Parent organization item.
        /// </summary>
        public OrganizationItem? ParentItem { get; set; }
        
        /// <summary>
        /// Collection of child organization items.
        /// </summary>
        public required ICollection<OrganizationItem> SubItems { get; set; }
        
        /// <summary>
        /// Collection of employees that are related to the organization item.
        /// </summary>
        public required ICollection<Employee> Employees { get; set; }

        /// <summary>
        /// Address of the organization item.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Name of the organization to which the item belongs.
        /// </summary>
        public string? OrganizationName { get; set; }
    }
}
using VelocipedeUtils.Shared.Models.Business.BusinessDocuments;
using VelocipedeUtils.Shared.Models.Business.Projects;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;

namespace VelocipedeUtils.Shared.Models.Business.Customers;

/// <summary>
/// Company (usually related to the customer).
/// </summary>
public class Company : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Registration number of the company.
    /// </summary>
    public string? RegistrationNumber { get; set; }

    /// <summary>
    /// VAT number of the company.
    /// </summary>
    public string? VatNumber { get; set; }
    
    /// <summary>
    /// CRM role type.
    /// </summary>
    public CrmRoleType CrmRoleType { get; set; }

    /// <summary>
    /// Contact of the company.
    /// </summary>
    public Contact? Contact { get; set; }

    /// <summary>
    /// Address of the company.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Shipping address of the company.
    /// </summary>
    public string? ShippingAddress { get; set; }

    /// <summary>
    /// Does the company have VAT registration.
    /// </summary>
    public bool HasVatRegistration { get; set; }

    /// <summary>
    /// Collection of the employees of the company.
    /// </summary>
    public required ICollection<Employee> Employees { get; set; }

    /// <summary>
    /// Collection of the projects related to the company.
    /// </summary>
    [Obsolete("It's better to use CompanyProject object")]
    public required ICollection<Project> Projects { get; set; }

    /// <summary>
    /// Contracts.
    /// </summary>
    [Obsolete("It's better to use CompanyContract object")]
    public required ICollection<Contract> Contracts { get; set; }
}
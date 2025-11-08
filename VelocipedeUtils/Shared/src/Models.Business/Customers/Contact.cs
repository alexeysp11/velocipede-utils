namespace VelocipedeUtils.Shared.Models.Business.Customers;

/// <summary>
/// Contact.
/// </summary>
public class Contact : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Mobile phone.
    /// </summary>
    public string? MobilePhone { get; set; }

    /// <summary>
    /// Home phone.
    /// </summary>
    public string? HomePhone { get; set; }

    /// <summary>
    /// Work phone.
    /// </summary>
    public string? WorkPhone { get; set; }

    /// <summary>
    /// Office phone.
    /// </summary>
    public string? OfficePhone { get; set; }
    
    /// <summary>
    /// Email.
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Office email.
    /// </summary>
    public string? OfficeEmail { get; set; }
}
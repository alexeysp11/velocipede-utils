using VelocipedeUtils.Shared.Models.Business.InformationSystem;

namespace VelocipedeUtils.Shared.Models.Business.BusinessDocuments;

/// <summary>
/// Offer.
/// </summary>
public class Offer : WfBusinessEntity, IWfBusinessEntity, ISendableBusinessEntity
{
    /// <summary>
    /// 
    /// </summary>
    public string? HtmlContent { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public string? ContractId { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public string? InquiryId { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public Inquiry? Inquiry { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public int? Revision { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? DateLastRevised { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? DateApproved { get; private set; }

    /// <summary>
    /// Date the business entity was sent.
    /// </summary>
    public DateTime? DateSent { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? SentById { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public Employee? EmployeeSent { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public string? CreatedById { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public Employee? EmployeeCreated { get; private set; }

    //public CompanyContract Contract { get; private set; }
}
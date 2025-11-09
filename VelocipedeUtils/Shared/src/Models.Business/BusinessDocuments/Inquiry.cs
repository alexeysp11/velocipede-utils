using VelocipedeUtils.Shared.Models.Business.Customers;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;

namespace VelocipedeUtils.Shared.Models.Business.BusinessDocuments;

/// <summary>
/// Inquiry.
/// </summary>
public class Inquiry : WfBusinessEntity, IWfBusinessEntity, IReceivableBusinessEntity
{
    /// <summary>
    /// Timestamp when the inquiry was closed.
    /// </summary>
    public DateTime? DateClosed { get; set; }

    /// <summary>
    /// Date the business entity was received.
    /// </summary>
    public DateTime? DateReceived { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? ReceivedById { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Employee? ReceivedBy { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? CustomerId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Company? CustomerCompany { get; set; }

    /// <summary>
    /// List of offers.
    /// </summary>
    public required List<Offer> Offers { get; set; }

    //public List<Attachment> Attachments { get; private set; }

    //public List<InfoRequest> InfoRequests { get; private set; }

    //public List<InquiryEmployee> Assignees { get; private set; }
}
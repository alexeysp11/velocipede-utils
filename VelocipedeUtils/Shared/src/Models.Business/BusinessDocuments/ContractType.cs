using System.ComponentModel.DataAnnotations;

namespace VelocipedeUtils.Shared.Models.Business.BusinessDocuments;

/// <summary>
/// Contract type.
/// </summary>
public enum ContractType
{
    [Display(Name = "Not Selected")]
    None = 0,

    // Sales
    [Display(Name = "Purchase Order")]
    PurchaseOrder = 1,
    [Display(Name = "Renewal Order")]
    RenewalOrder = 2,
    [Display(Name = "Sales Contract")]
    SalesContract = 3,
    [Display(Name = "Vendor Agreement")]
    VendorAgreement = 4,
    [Display(Name = "Warranty Contract")]
    WarrantyContract = 5,

    // Marketing
    [Display(Name = "Advisory Agreement")]
    AdvisoryAgreement = 6,
    [Display(Name = "Confidentiality Agreement")]
    ConfidentialityAgreement = 7,
    [Display(Name = "Influencer Agreement")]
    InfluencerAgreement = 8,
    [Display(Name = "Photography Release")]
    PhotographyRelease = 9,
    [Display(Name = "Privacy Agreement")]
    PrivacyAgreement = 10,

    // IT/Operation
    [Display(Name = "Cloud Computing Agreement")]
    CloudComputingAgreement = 11,
    [Display(Name = "Equipment Lease Agreement")]
    EquipmentLeaseAgreement = 12,
    [Display(Name = "Non Disclosure Agreement")]
    NonDisclosureAgreement = 13,
    [Display(Name = "Property Lease Agreement")]
    PropertyLeaseAgreement = 14,
    [Display(Name = "Security Agreement")]
    SecurityAgreement = 15,
    [Display(Name = "Terms Of Use")]
    TermsOfUse = 16,

    // Human Resources
    [Display(Name = "Director Service Agreement")]
    DirectorServiceAgreement = 17,
    [Display(Name = "Employment Contract")]
    EmploymentContract = 18,
    [Display(Name = "Employee Transfer Agreement")]
    EmployeeTransferAgreement = 19,
    [Display(Name = "Independent Contractor Agreement")]
    IndependentContractorAgreement = 20,
    [Display(Name = "Non-Complete Agreement")]
    NonCompleteAgreement = 21,
    [Display(Name = "Recruitment Agreement")]
    RecruitmentAgreement = 22,
    [Display(Name = "Termination Agreement")]
    TerminationAgreement = 23,

    // Procurement
    [Display(Name = "Contract Amendment")]
    ContractAmendment = 24,
    [Display(Name = "Joint Venture Agreement")]
    JointVentureAgreement = 25,
    [Display(Name = "License Agreement")]
    LicenseAgreement = 26,
    [Display(Name = "Master Service Agreement")]
    MasterServiceAgreement = 27,
    [Display(Name = "Memorandum of understanding")]
    MemorandumOfUnderstandingAgreement = 28,
    [Display(Name = "Partnership Agreement")]
    PartnershipAgreement = 29,
    [Display(Name = "Promissory note")]
    PromissoryNote = 30,
    [Display(Name = "Statement of work")]
    SatementOfWork = 31
}
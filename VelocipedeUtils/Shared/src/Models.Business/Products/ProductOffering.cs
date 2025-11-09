using VelocipedeUtils.Shared.Models.Business.Customers;

namespace VelocipedeUtils.Shared.Models.Business.Products;

/// <summary>
/// Product offering.
/// </summary>
public class ProductOffering : WfBusinessEntity, IWfBusinessEntity, ITemporalBusinessEntity
{
    /// <summary>
    /// Supplier (comany).
    /// </summary>
    public Company? CompanySupplier { get; set; }

    /// <summary>
    /// Supplier (customer).
    /// </summary>
    public Customer? CustomerSupplier { get; set; }

    /// <summary>
    /// Beneficiaries (comanies).
    /// </summary>
    public required ICollection<Company> CompanyBeneficiaries { get; set; }

    /// <summary>
    /// Beneficiaries (customers).
    /// </summary>
    public required ICollection<Customer> CustomerBeneficiaries { get; set; }

    /// <summary>
    /// Products.
    /// </summary>
    public required ICollection<Product> Products { get; set; }

    /// <summary>
    /// Actual price.
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Actual start date.
    /// </summary>
    public DateTime? DateStartActual { get; set; }
    
    /// <summary>
    /// Actual end date.
    /// </summary>
    public DateTime? DateEndActual { get; set; }
    
    /// <summary>
    /// Expected start date.
    /// </summary>
    public DateTime? DateStartExpected { get; set; }
    
    /// <summary>
    /// Expected end date.
    /// </summary>
    public DateTime? DateEndExpected { get; set; }
}
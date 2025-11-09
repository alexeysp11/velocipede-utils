namespace VelocipedeUtils.Shared.Models.Business.Products;

/// <summary>
/// Interface that represents a business entity which is expirable.
/// </summary>
public interface IExpirableProduct : IWfBusinessEntity
{
    /// <summary>
    /// Production date.
    /// </summary>
    DateTime? DateProduction { get; set; }
    
    /// <summary>
    /// Packaging date.
    /// </summary>
    DateTime? DatePackaging { get; set; }
    
    /// <summary>
    /// Expiration date.
    /// </summary>
    DateTime? DateExpiration { get; set; }
}
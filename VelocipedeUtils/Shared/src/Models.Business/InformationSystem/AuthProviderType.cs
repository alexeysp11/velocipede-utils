using System.ComponentModel.DataAnnotations;

namespace VelocipedeUtils.Shared.Models.Business.InformationSystem;

/// <summary>
/// Authentication provider type.
/// </summary>
public enum AuthProviderType
{
    [Display(Name = "Standard")]
    Standard,
    
    [Display(Name = "LDAP/AD")]
    LdapAd
}
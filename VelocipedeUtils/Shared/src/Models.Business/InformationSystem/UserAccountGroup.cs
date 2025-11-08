namespace VelocipedeUtils.Shared.Models.Business.InformationSystem;

/// <summary>
/// Establishes a dependency between the <see cref="UserAccount"/> and <see cref="UserGroup"/> classes.
/// </summary>
public class UserAccountGroup : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// User account.
    /// </summary>
    public UserAccount? UserAccount { get; set; }

    /// <summary>
    /// User group.
    /// </summary>
    public UserGroup? UserGroup { get; set; }
}
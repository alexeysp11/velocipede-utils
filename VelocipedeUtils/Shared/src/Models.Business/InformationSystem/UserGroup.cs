namespace VelocipedeUtils.Shared.Models.Business.InformationSystem;

/// <summary>
/// User group.
/// </summary>
public class UserGroup : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Is it a group by default for the new user
    /// </summary>
    public bool IsGroupByDefault { get; set; }
    
    /// <summary>
    /// Boolean variable that shows if the user group is system.
    /// </summary>
    public bool IsSystem { get; set; }
    
    /// <summary>
    /// User that created the user group.
    /// </summary>
    public UserAccount? AuthorCreated { get; set; }
    
    /// <summary>
    /// User that changed the user group.
    /// </summary>
    public UserAccount? AuthorChanged { get; set; }
}
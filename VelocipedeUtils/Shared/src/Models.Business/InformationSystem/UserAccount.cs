namespace VelocipedeUtils.Shared.Models.Business.InformationSystem;

/// <summary>
/// User account.
/// </summary>
public class UserAccount : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Login of the user.
    /// </summary>
    public required string Login { get; set; }

    /// <summary>
    /// Email of the user.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Phone number of the user.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Password of the user.
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// Photo of the user.
    /// </summary>
    public byte[]? Photo { get; set; }

    /// <summary>
    /// Photo URL of the user.
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Status of the user.
    /// </summary>
    public UserStatus? Status { get; set; }

    /// <summary>
    /// Timestamp when the user was online the last time.
    /// </summary>
    public DateTime? LastSeenDt { get; set; }
}
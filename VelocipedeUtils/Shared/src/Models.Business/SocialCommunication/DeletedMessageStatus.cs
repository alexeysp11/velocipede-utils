using System.ComponentModel.DataAnnotations;

namespace VelocipedeUtils.Shared.Models.Business.SocialCommunication;

/// <summary>
/// Deleted message status.
/// </summary>
public enum DeletedMessageStatus
{
    [Display(Name = "None")]
    None,

    [Display(Name = "Delete For Sender")]
    DeleteForSender,

    [Display(Name = "Delete For Everybody")]
    DeleteForEverybody
}
using System.ComponentModel.DataAnnotations;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;

namespace VelocipedeUtils.Ubp.ViewModels;

public class UserAccountViewModel
{
    [Key]
    public long UserAccountId { get; set; }

    public EmployeeUserAccount EmployeeUserAccount { get; set; }

    public IEnumerable<UserAccountGroup> UserAccountGroups { get; set; }
}
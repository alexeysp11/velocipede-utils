using Microsoft.EntityFrameworkCore;
using VelocipedeUtils.Shared.Models.Business.Customers;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;
using VelocipedeUtils.Shared.Models.Business.Languages;

namespace VelocipedeUtils.Ubp.Core.DbContexts;

/// <summary>
/// Database context of the UnifiedBusinessPlatform.
/// </summary>
public class UbpDbContext : DbContext
{
    public UbpDbContext(DbContextOptions<UbpDbContext> options) : base(options) { }

    /// <summary>
    /// User accounts.
    /// </summary>
    public DbSet<UserAccount> UserAccounts { get; set; }

    /// <summary>
    /// User groups.
    /// </summary>
    public DbSet<UserGroup> UserGroups { get; set; }

    /// <summary>
    /// Establishes a dependency between user accounts and user groups.
    /// </summary>
    public DbSet<UserAccountGroup> UserAccountGroups { get; set; }

    /// <summary>
    /// Establishes a dependency between user accounts and employees.
    /// </summary>
    public DbSet<EmployeeUserAccount> EmployeeUserAccounts { get; set; }

    /// <summary>
    /// Employees.
    /// </summary>
    public DbSet<Employee> Employees { get; set; }

    /// <summary>
    /// Organizations.
    /// </summary>
    public DbSet<Organization> Organizations { get; set; }

    /// <summary>
    /// Organization items.
    /// </summary>
    public DbSet<OrganizationItem> OrganizationItems { get; set; }

    /// <summary>
    /// Companies.
    /// </summary>
    public DbSet<Company> Companies { get; set; }

    /// <summary>
    /// Contacts.
    /// </summary>
    public DbSet<Contact> Contacts { get; set; }

    /// <summary>
    /// Absenses.
    /// </summary>
    public DbSet<Absense> Absenses { get; set; }

    /// <summary>
    /// Language keys.
    /// </summary>
    public DbSet<LanguageKey> LanguageKeys { get; set; }

    /// <summary>
    /// Language key-value pairs.
    /// </summary>
    public DbSet<LanguageKeyValuePair> LanguageKeyValuePairs { get; set; }

    /// <summary>
    /// Establishes a dependency between language keys and forms, or views.
    /// </summary>
    public DbSet<LanguageKeyForm> LanguageKeyForms { get; set; }
}
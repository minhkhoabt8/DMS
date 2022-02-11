using System.ComponentModel.DataAnnotations;

namespace Auth.Core.Entities;

/// <summary>
/// Represents a role of an account
/// </summary>
public class Role
{
    public int ID { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; }

    public ICollection<Account> Accounts { get; set; }
}
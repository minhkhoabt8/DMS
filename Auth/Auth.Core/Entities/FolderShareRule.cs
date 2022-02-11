using Auth.Core.Enums;

namespace Auth.Core.Entities;

/// <summary>
/// Rule for sharing a resource
/// </summary>
public class FolderShareRule
{
    /// <summary>
    /// ShareRule ID
    /// </summary>
    public int ID { get; set; }

    public Guid FolderID { get; set; }
    public Folder Folder { get; set; }

    /// <summary>
    /// ShareRule Type
    /// </summary>
    public ShareType Type { get; set; }

    /// <summary>
    /// What the shared user can do with the resource 
    /// </summary>
    public ShareScope Scope { get; set; }

    /// <summary>
    /// ShareRule value
    /// </summary>
    /// <remarks>Depending on ShareType, value will be of different format</remarks>
    /// <example>For AccountID <see cref="ShareType"/>, value will be ID of an account</example>
    /// <example>For PIN <see cref="ShareType"/>, value will be a PIN</example>
    public string Value { get; set; }

    /// <summary>
    /// Whether this share rule is the original rule created by user or is inherited by parent folder
    /// </summary>
    public bool IsRoot { get; set; }

    /// <summary>
    /// The time after which this rule will be invalid
    /// </summary>
    public DateTime ExpirationDate { get; set; }
}
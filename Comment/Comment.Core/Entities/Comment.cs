namespace Comment.Core.Entities;

/// <summary>
/// Represents a comment on a resource
/// </summary>
public class Comment
{
    public int ID { get; set; }
    public DateTime DateCommented { get; set; }
    public string Content { get; set; }
    public Guid CommenterID { get; set; }
    public Account Commenter { get; set; }
    public int FileID { get; set; }
}
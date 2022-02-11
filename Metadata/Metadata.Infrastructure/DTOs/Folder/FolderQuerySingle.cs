using Metadata.Infrastructure.DTOs.Common;

namespace Metadata.Infrastructure.DTOs.Folder;

public class FolderQuerySingle : IIncludeQuery, ISearchTextQuery
{
    public Guid? ID { get; set; }
    public bool? IsRoot { get; set; }
    public Guid? ParentFolderID { get; set; }
    public Guid? OwnerID { get; set; }
    public string Include { get; set; }
    public string SearchText { get; set; }
}
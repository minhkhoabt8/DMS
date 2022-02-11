using Metadata.Infrastructure.DTOs.Common;

namespace Metadata.Infrastructure.DTOs.Folder;

public class FolderQuery : IIncludeQuery, ISearchTextQuery
{
    public Guid? ParentFolderID { get; set; }
    public Guid? AccountID { get; set; }
    public string Include { get; set; }
    public string SearchText { get; set; }
}
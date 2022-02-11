using Metadata.Infrastructure.DTOs.Common;

namespace Metadata.Infrastructure.DTOs.File;

public class FileQuerySingle : IIncludeQuery, ISearchTextQuery
{
    public Guid? ID { get; set; }
    public string Include { get; set; }
    public string SearchText { get; set; }
    public Guid? ParentFolderID { get; set; }
    public Guid? OwnerID { get; set; }
}
using Content.Infrastructure.DTOs.Common;

namespace Content.Infrastructure.DTOs.File;

public class FileQuerySingle : IIncludeQuery
{
    public Guid? ID { get; set; }
    public string Include { get; set; }
}
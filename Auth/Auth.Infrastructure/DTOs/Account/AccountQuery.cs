using Auth.Infrastructure.DTOs.Common;

namespace Auth.Infrastructure.DTOs.Account;

public class AccountQuery : PaginatedQuery, IIncludeQuery, ISearchTextQuery, IActiveQuery
{
    public string Include { get; set; }
    public bool ShowInactive { get; set; } = false;
    public string SearchText { get; set; }
}
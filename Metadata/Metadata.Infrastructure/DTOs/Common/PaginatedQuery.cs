namespace Metadata.Infrastructure.DTOs.Common;

public class PaginatedQuery
{
    public static int MAX_PAGE_SIZE = 50;

    // Default values
    private int _pageNumber = 1;
    private int _maxPageSize = 10;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = Math.Max(1, value);
    }

    public int MaxPageSize
    {
        get => _maxPageSize;
        set => _maxPageSize = Math.Min(Math.Max(1, value), MAX_PAGE_SIZE);
    }
}
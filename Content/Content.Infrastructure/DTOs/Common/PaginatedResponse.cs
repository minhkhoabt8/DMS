using AutoMapper;

namespace Content.Infrastructure.DTOs.Common;

public class PaginatedResponse<T> : List<T>
{
    public Pagination Pagination { get; }

    public PaginatedResponse(IEnumerable<T> items, int count, int pageNumber, int maxPageSize)
    {
        int totalPages = (int) Math.Ceiling(count / (double) maxPageSize);

        Pagination = new Pagination
        {
            CurrentPage = pageNumber,
            MaxPageSize = maxPageSize,
            TotalPages = totalPages,
            TotalCount = count,
            HasNext = pageNumber < totalPages,
            HasPrevious = pageNumber > 1
        };
        AddRange(items);
    }

    public static PaginatedResponse<T> FromEnumerable(IEnumerable<T> source, PaginatedQuery query)
    {
        var count = source.Count();
        var items = source.Skip((query.PageNumber - 1) * query.MaxPageSize).Take(query.MaxPageSize).ToList();

        return new PaginatedResponse<T>(items, count, query.PageNumber, query.MaxPageSize);
    }

    public static PaginatedResponse<T> FromEnumerableWithMapping<TSource>(IEnumerable<TSource> source,
        PaginatedQuery query, IMapper mapper)
    {
        var count = source.Count();
        var items = source.Skip((query.PageNumber - 1) * query.MaxPageSize).Take(query.MaxPageSize).ToList();

        return new PaginatedResponse<T>(mapper.Map<IEnumerable<T>>(items), count, query.PageNumber,
            query.MaxPageSize);
    }
}

public class Pagination
{
    public int TotalCount { get; set; }
    public int MaxPageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public bool HasNext { get; set; }
    public bool HasPrevious { get; set; }
}
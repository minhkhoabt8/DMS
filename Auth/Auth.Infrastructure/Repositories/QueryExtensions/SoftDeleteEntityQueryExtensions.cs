using System.Linq;
using Auth.Core.Entities.Common;

namespace Auth.Infrastructure.Repositories.QueryExtensions;

public static class SoftDeleteEntityQueryExtensions
{
    public static IQueryable<T> AllowInactive<T>(this IQueryable<T> query, bool showInactive)
        where T : ISoftDeleteEntity
    {
        return query.Where(e => showInactive || e.IsActive == true);
    }
}
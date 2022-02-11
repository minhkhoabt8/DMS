namespace Auth.Infrastructure.DTOs.Common;

public interface ICacheIDQuery
{
    // Used to avoid cached responses
    public string CacheID { get; set; }
}
namespace Auth.Infrastructure.DTOs.Common;

public interface IOrderedQuery
{
    const string ASCENDING_EXPRESSION = "ASC";
    const string DESCENDING_EXPRESSION = "DESC";

    string OrderBy { get; set; }
}
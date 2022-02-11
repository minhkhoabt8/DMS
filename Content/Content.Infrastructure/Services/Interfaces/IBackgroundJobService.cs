namespace Content.Infrastructure.Services.Interfaces;

public interface IBackgroundJobService
{
    Task SaveDeltaAsync(int versionID, byte[] fileBytes);
    Task SaveFileAsync(int versionID, byte[] fileBytes);
}
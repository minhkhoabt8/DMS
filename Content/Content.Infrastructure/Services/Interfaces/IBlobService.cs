namespace Content.Infrastructure.Services.Interfaces
{
    public interface IBlobService
    {
        Task<string> UploadAsync(string fname, Stream stream);
        Task<string> UploadAsync(string fname, byte[] data);
        Task DeleteAsync(string url);
        Task<Stream> DownloadAsync(string url);
    }
}
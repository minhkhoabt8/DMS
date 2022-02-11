using Azure.Storage;
using Azure.Storage.Blobs;
using Content.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Content.Infrastructure.Services.Implementations
{
    public class AzureBlobService : IBlobService
    {
        private readonly IConfiguration _configuration;

        public AzureBlobService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> UploadAsync(string fname, byte[] data)
        {
            var (accountName, containerName, accountKey) = GetAzureStorageConfig();

            Uri blobUri = new(
                $"https://{accountName}.blob.core.windows.net/{containerName}/{fname}"
            );

            Log.Information($"Uploading blob {blobUri.AbsoluteUri}");

            StorageSharedKeyCredential storageCredentials = new(accountName, accountKey);

            BlobClient blobClient = new(blobUri, storageCredentials);

            await blobClient.UploadAsync(new BinaryData(data), overwrite: true);

            return blobUri.AbsoluteUri;
        }

        public async Task DeleteAsync(string url)
        {
            Log.Information($"Deleting blob {url}");

            var (accountName, _, accountKey) = GetAzureStorageConfig();

            Uri blobUri = new(url);

            StorageSharedKeyCredential storageCredentials = new(accountName, accountKey);

            BlobClient blobClient = new(blobUri, storageCredentials);

            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<Stream> DownloadAsync(string url)
        {
            Log.Information($"Downloading blob {url}");

            var (accountName, _, accountKey) = GetAzureStorageConfig();

            Uri blobUri = new(url);
            StorageSharedKeyCredential storageCredentials = new(accountName, accountKey);

            BlobClient blobClient = new(blobUri, storageCredentials);

            var stream = new MemoryStream();

            await blobClient.DownloadToAsync(stream);

            stream.Position = 0;

            return stream;
        }

        public async Task<string> UploadAsync(string fname, Stream stream)
        {
            var (accountName, containerName, accountKey) = GetAzureStorageConfig();

            Uri blobUri = new(
                $"https://{accountName}.blob.core.windows.net/{containerName}/{fname}"
            );

            Log.Information($"Uploading blob {blobUri.AbsoluteUri}");

            StorageSharedKeyCredential storageCredentials = new(accountName, accountKey);

            BlobClient blobClient = new(blobUri, storageCredentials);

            await blobClient.UploadAsync(stream, overwrite: true);

            return blobUri.AbsoluteUri;
        }

        private (string accountName, string containerName, string accountKey) GetAzureStorageConfig()
        {
            return (_configuration["AzureStorage:AccountName"],
                _configuration["AzureStorage:ContainerName"], _configuration["AzureStorage:AccountKey"]);
        }
    }
}
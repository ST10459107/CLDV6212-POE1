using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using CoffeeNChill.Interface;
using CoffeeNChill.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CoffeeNChill.Service
{
    public class FileStorageService : IFileStorageService
    {
        private readonly ShareDirectoryClient _rootDirectory;
        private const string ShareName = "staff-docs";

        public FileStorageService(IConfiguration configuration)
        {
            string connectionString =
                configuration["AzureFileStorage"]
                ?? throw new InvalidOperationException("AzureFileStorage connection string is missing.");

            var shareClient = new ShareClient(connectionString, ShareName);
            shareClient.CreateIfNotExists();

            _rootDirectory = shareClient.GetRootDirectoryClient();
        }

        public async Task<StaffDocument> UploadDocumentAsync(IFormFile file)
        {
            ShareFileClient fileClient = _rootDirectory.GetFileClient(file.FileName);

            using Stream stream = file.OpenReadStream();
            await fileClient.CreateAsync(file.Length);
            await fileClient.UploadRangeAsync(new Azure.HttpRange(0, file.Length), stream);
            await fileClient.SetHttpHeadersAsync(new ShareFileSetHttpHeadersOptions
            {
                HttpHeaders = new ShareFileHttpHeaders
                {
                    ContentType = file.ContentType
                }
            });

            return new StaffDocument
            {
                FileName = file.FileName,
                FileExtension = Path.GetExtension(file.FileName),
                ContentType = file.ContentType,
                FileSize = file.Length,
                UploadedOn = DateTime.UtcNow,
                ContainerName = ShareName
            };
        }

        public async Task<Stream?> DownloadDocumentAsync(string fileName)
        {
            ShareFileClient fileClient = _rootDirectory.GetFileClient(fileName);

            if (!await fileClient.ExistsAsync())
                return null;

            ShareFileDownloadInfo download = await fileClient.DownloadAsync();
            return download.Content;
        }

        public async Task<bool> DeleteDocumentAsync(string fileName)
        {
            ShareFileClient fileClient = _rootDirectory.GetFileClient(fileName);
            var response = await fileClient.DeleteIfExistsAsync();
            return response.Value;
        }

        public async Task<List<StaffDocument>> GetAllDocumentsAsync()
        {
            var documents = new List<StaffDocument>();

            await foreach (ShareFileItem item in _rootDirectory.GetFilesAndDirectoriesAsync())
            {
                if (item.IsDirectory) continue;

                ShareFileClient fileClient = _rootDirectory.GetFileClient(item.Name);
                ShareFileProperties properties = await fileClient.GetPropertiesAsync();

                documents.Add(new StaffDocument
                {
                    FileName = item.Name,
                    FileExtension = Path.GetExtension(item.Name),
                    ContentType = properties.ContentType ?? string.Empty,
                    FileSize = properties.ContentLength,
                    UploadedOn = properties.LastModified.DateTime,
                    ContainerName = ShareName
                });
            }

            return documents;
        }
    }
}
using CoffeeNChill.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoffeeNChill.Interface
{
    public interface IFileStorageService
    {
        Task<StaffDocument> UploadDocumentAsync(
            IFormFile file);

        Task<Stream?> DownloadDocumentAsync(
            string fileName);

        Task<bool> DeleteDocumentAsync(
            string fileName);

        Task<List<StaffDocument>> GetAllDocumentsAsync();
    }
}

using CoffeeNChill.Interface;
using CoffeeNChill.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using System.Net;

namespace CoffeeNChill.Function
{
    public class UploadStaffDocumentFunction
    {
        private readonly IFileStorageService _fileStorageService;

        private const long MaxFileSize = 10 * 1024 * 1024;

        private static readonly string[] AllowedExtensions =
        {
            ".pdf",
            ".doc",
            ".docx"
        };

        public UploadStaffDocumentFunction(
            IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        [Function("UploadStaffDocument")]
        public async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "post",
                Route = "documents/upload")]
            HttpRequest req)
        {
            try
            {
                if (!req.HasFormContentType)
                {
                    return new BadRequestObjectResult(new
                    {
                        message = "The request must use multipart/form-data."
                    });
                }

                var form = await req.ReadFormAsync();
                IFormFile? file = form.Files["file"];

                if (file == null)
                {
                    return new BadRequestObjectResult(new
                    {
                        message = "Please upload a file using the form-data field named 'file'."
                    });
                }

                if (file.Length == 0)
                {
                    return new BadRequestObjectResult(new
                    {
                        message = "The uploaded file is empty."
                    });
                }

                if (file.Length > MaxFileSize)
                {
                    return new BadRequestObjectResult(new
                    {
                        message = "The uploaded file exceeds the maximum allowed size of 10 MB."
                    });
                }

                if (string.IsNullOrWhiteSpace(file.FileName))
                {
                    return new BadRequestObjectResult(new
                    {
                        message = "The uploaded file must have a valid file name."
                    });
                }

                string extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!AllowedExtensions.Contains(extension))
                {
                    return new BadRequestObjectResult(new
                    {
                        message = "Invalid file type. Only PDF, DOC and DOCX files are allowed."
                    });
                }

                StaffDocument document = await _fileStorageService.UploadDocumentAsync(file);

                return new OkObjectResult(document);
            }
            catch (Exception)
            {
                return new ObjectResult(new
                {
                    message = "An unexpected error occurred while uploading the document."
                })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
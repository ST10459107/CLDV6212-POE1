using CoffeeNChill.DTOs;
using CoffeeNChill.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace CoffeeNChill.Function
{
    public class CreateMenuItemFunction
    {
        private readonly ITableStorageService _tableStorageService;
        private readonly ILogger<CreateMenuItemFunction> _logger;

        public CreateMenuItemFunction(
            ITableStorageService tableStorageService,
            ILogger<CreateMenuItemFunction> logger)
        {
            _tableStorageService = tableStorageService;
            _logger = logger;
        }

        [Function("CreateMenuItem")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "menu")]
            HttpRequestData req)
        {
            _logger.LogInformation("Creating a new menu item.");

            try
            {
                var request =
                    await JsonSerializer.DeserializeAsync<CreateMenuItemRequest>(
                        req.Body,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (request == null)
                {
                    var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                    await badRequest.WriteAsJsonAsync(new { error = "Invalid request body." });
                    return badRequest;
                }

                if (string.IsNullOrWhiteSpace(request.Category))
                {
                    var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                    await badRequest.WriteAsJsonAsync(new { error = "Category is required." });
                    return badRequest;
                }

                if (string.IsNullOrWhiteSpace(request.SKU))
                {
                    var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                    await badRequest.WriteAsJsonAsync(new { error = "SKU is required." });
                    return badRequest;
                }

                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                    await badRequest.WriteAsJsonAsync(new { error = "Name is required." });
                    return badRequest;
                }

                if (string.IsNullOrWhiteSpace(request.Description))
                {
                    var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                    await badRequest.WriteAsJsonAsync(new { error = "Description is required." });
                    return badRequest;
                }

                if (request.Price <= 0)
                {
                    var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                    await badRequest.WriteAsJsonAsync(new { error = "Price must be greater than zero." });
                    return badRequest;
                }

                var menuItem = await _tableStorageService.CreateMenuItemAsync(request);

                var response = req.CreateResponse(HttpStatusCode.Created);
                await response.WriteAsJsonAsync(menuItem);
                return response;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Invalid JSON received while creating menu item.");
                var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteAsJsonAsync(new { error = "The request body contains invalid JSON." });
                return badRequest;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating menu item.");
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteAsJsonAsync(new { error = "An unexpected error occurred while creating the menu item." });
                return response;
            }
        }
    }
}
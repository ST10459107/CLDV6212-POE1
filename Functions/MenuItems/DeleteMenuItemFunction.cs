using CoffeeNChill.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CoffeeNChill.Function
{
    public class DeleteMenuItemFunction
    {
        private readonly ITableStorageService _tableStorageService;
        private readonly ILogger<DeleteMenuItemFunction> _logger;

        public DeleteMenuItemFunction(
            ITableStorageService tableStorageService,
            ILogger<DeleteMenuItemFunction> logger)
        {
            _tableStorageService = tableStorageService;
            _logger = logger;
        }

        [Function("DeleteMenuItem")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "menu/{category}/{sku}")]
            HttpRequestData req,
            string category,
            string sku)
        {
            _logger.LogInformation("Deleting menu item. Category: {Category}, SKU: {SKU}", category, sku);

            try
            {
                bool deleted = await _tableStorageService.DeleteMenuItemAsync(category, sku);

                if (!deleted)
                {
                    var notFound = req.CreateResponse(HttpStatusCode.NotFound);
                    await notFound.WriteAsJsonAsync(new { error = "Menu item not found." });
                    return notFound;
                }

                return req.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting menu item.");
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteAsJsonAsync(new { error = "An unexpected error occurred while deleting the menu item." });
                return response;
            }
        }
    }
}
using CoffeeNChill.DTOs;
using CoffeeNChill.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using System.Text;

namespace CoffeeNChill.Interface
{
    public interface ITableStorageService
    {
        Task<MenuItem> CreateMenuItemAsync(CreateMenuItemRequest request);
        Task<List<MenuItem>> GetAllMenuItemsAsync();
        Task<List<MenuItem>> GetMenuItemsByCategoryAsync(string category);
        Task<MenuItem?> GetMenuItemAsync(string category, string sku);
        Task<MenuItem?> UpdateMenuItemAsync(
            string category,
            string sku,
            UpdateMenuItemRequest request);
        Task<bool> DeleteMenuItemAsync(
            string category,
            string sku);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CoffeeNChill.DTOs
{
    public class CreateMenuItemRequest
    {
        // Category of the Menu item used as Partition Key
        public string Category { get; set; } = string.Empty;

        //Unique Stock Keeping Unit used as Row Key
        public string SKU { get; set; } = string.Empty;

        //Name of the Menu Item 
        public string Name { get; set; } = string.Empty;

        //Description of the menu item
        public string Description { get; set; } = string.Empty;

        //Selling Price
        public double Price { get; set; }

        //Indicates whether the item is available
        public bool IsAvailable { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CoffeeNChill.DTOs
{
    public class UpdateMenuItemRequest
    {
        //Updated Menu Item Name
        public string Name { get; set; } = string.Empty;

        //Updated Description 
        public string Description { get; set; } = string.Empty;

        //Updated Price
        public double Price { get; set; }

        //Updated availability
        public bool IsAvailable { get; set; }
    }
}

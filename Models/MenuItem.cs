using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoffeeNChill.Models
{
    public class MenuItem : ITableEntity
    {
        // Azure Table Storage Partition Key
        public string PartitionKey { get; set; } = string.Empty;

        //Azure Table Storage Row Key
        public string RowKey { get; set; } = string.Empty;

        //Menu Item Name
        public string Name { get; set; } = string.Empty;

        //Description of the menu item
        public string Description { get; set; } = string.Empty;

        //Selling Price
        public double Price { get; set; }

        //Indicates whether the item is available
        public bool IsAvailable { get; set; }

        //Automatically maintaned by Azure Table Storage 
        public DateTimeOffset? Timestamp {  get; set; }

        //Entity Tag used for concurrecy 
        public ETag ETag { get; set; }
    }
}

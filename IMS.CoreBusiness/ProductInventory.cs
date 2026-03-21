using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IMS.CoreBusiness
{
    public class ProductInventory
    {
        public int ProductInventoryId { get; set; }
        public int ProductId { get; set; }
        public int InventoryId { get; set; }
        public int InventoryQuantity { get; set; }

        // Navigation property
        [JsonIgnore]
        public Product? Product { get; set; }
        [JsonIgnore]
        public Inventory? Inventory { get; set; }
    }
}

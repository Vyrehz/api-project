using Newtonsoft.Json;
using System;

namespace refactor_this.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        [JsonIgnore]
        public bool IsNew { get; set;  } = true;
    }
}
using Newtonsoft.Json;
using System;

namespace refactor_this.Models
{
    public class ProductOption
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore] public bool IsNew { get; set; } = true;
    }
}
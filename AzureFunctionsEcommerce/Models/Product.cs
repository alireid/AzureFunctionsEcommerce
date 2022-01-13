using Newtonsoft.Json;

namespace AzureFunctionsEcommerce.Models
{
    public class Product
    {
        [JsonProperty("productid")]
        public int Id { get; set; }

        [JsonProperty("productName")]
        public int Name { get; set; }

        [JsonProperty("productDescription")]
        public int Description { get; set; }

        [JsonProperty("StockCount")]
        public int StockCount { get; set; }
    }
}

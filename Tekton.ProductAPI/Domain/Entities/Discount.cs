using System.Text.Json.Serialization;

namespace Tekton.ProductAPI.Domain.Entities
{
    public class Discount
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("Percentage")]
        public int Percentage { get; set; }
        [JsonPropertyName("productId")]
        public string ProductId { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace OrderService.Api.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        
        [JsonIgnore] // Prevent circular reference
        public virtual Order? Order { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace OrderService.Api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        
        [JsonIgnore] // Prevent circular reference
        public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}

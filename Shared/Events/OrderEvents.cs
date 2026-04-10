using System.Text.Json.Serialization;

namespace Shared.Events
{
    public class OrderCreatedEvent
    {
        [JsonPropertyName("orderId")]
        public int OrderId { get; set; }
        
        [JsonPropertyName("customerId")]
        public int CustomerId { get; set; }
        
        [JsonPropertyName("items")]
        public List<OrderItemEvent> Items { get; set; } = new();
        
        [JsonPropertyName("totalAmount")]
        public decimal TotalAmount { get; set; }
        
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
    }

    public class OrderCancelledEvent
    {
        [JsonPropertyName("orderId")]
        public int OrderId { get; set; }
        
        [JsonPropertyName("customerId")]
        public int CustomerId { get; set; }
        
        [JsonPropertyName("items")]
        public List<OrderItemEvent> Items { get; set; } = new();
        
        [JsonPropertyName("cancelledAt")]
        public DateTime CancelledAt { get; set; }
    }

    public class OrderItemEvent
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }
        
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
        
        [JsonPropertyName("unitPrice")]
        public decimal UnitPrice { get; set; }
    }
}

namespace OrderService.Api.DTOs
{
    public class OrderReadDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<OrderItemReadDTO> Items { get; set; } = new();
    }

    public class OrderCreateDTO
    {
        public int CustomerId { get; set; }
        public List<OrderItemCreateDTO> Items { get; set; } = new();
    }

    public class OrderItemReadDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class OrderItemCreateDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

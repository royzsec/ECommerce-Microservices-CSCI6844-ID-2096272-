namespace PaymentService.Api.DTOs
{
    public class PaymentReadDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public string TransactionId { get; set; } = string.Empty;
    }

    public class PaymentCreateDTO
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;
    }

    public class PaymentProcessDTO
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}

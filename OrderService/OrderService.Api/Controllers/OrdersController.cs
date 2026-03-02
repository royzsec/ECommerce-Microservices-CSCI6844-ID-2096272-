using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Api.Data;
using OrderService.Api.Models;

namespace OrderService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersDbContext _db;
        private readonly IHttpClientFactory _httpClientFactory;

        public OrdersController(OrdersDbContext db, IHttpClientFactory httpClientFactory)
        {
            _db = db;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _db.Orders.ToListAsync();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest req)
        {
            var productClient = _httpClientFactory.CreateClient("ProductService");
            var customerClient = _httpClientFactory.CreateClient("CustomerService");
            var paymentClient = _httpClientFactory.CreateClient("PaymentService");

            // Validate Product
            var productRes = await productClient.GetAsync($"/api/products/{req.ProductId}");
            if (!productRes.IsSuccessStatusCode)
                return BadRequest($"Invalid productId: {req.ProductId}");

            // Validate Customer
            var customerRes = await customerClient.GetAsync($"/api/customers/{req.CustomerId}");
            if (!customerRes.IsSuccessStatusCode)
                return BadRequest($"Invalid customerId: {req.CustomerId}");

            // Create Order
            var order = new Order
            {
                CustomerId = req.CustomerId,
                ProductId = req.ProductId,
                Quantity = req.Quantity,
                Status = "CREATED",
                CreatedAt = DateTime.UtcNow
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            // Call Payment (adjust endpoint if your payment route differs)
            var paymentPayload = new
            {
                orderId = order.Id,
                amount = req.Quantity
            };

            var payRes = await paymentClient.PostAsJsonAsync("/api/payments", paymentPayload);

            if (!payRes.IsSuccessStatusCode)
            {
                order.Status = "PAYMENT_FAILED";
                await _db.SaveChangesAsync();
                return StatusCode(500, "Payment service failed.");
            }

            order.Status = "PAID";
            await _db.SaveChangesAsync();

            return Ok(order);
        }
    }
}
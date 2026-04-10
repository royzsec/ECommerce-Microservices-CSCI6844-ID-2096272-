using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Api.Data;
using OrderService.Api.Models;

namespace OrderService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimpleOrdersController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly ILogger<SimpleOrdersController> _logger;

        public SimpleOrdersController(OrderDbContext context, ILogger<SimpleOrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] SimpleOrderRequest request)
        {
            try
            {
                var order = new Order
                {
                    CustomerId = request.CustomerId,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = request.TotalAmount,
                    Status = "Pending",
                    Items = new List<OrderItem>()
                };
                
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation($"Simple order {order.Id} created");
                
                return Ok(new { 
                    message = "Order created successfully", 
                    orderId = order.Id,
                    order = order
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating simple order");
                return StatusCode(500, "Internal server error");
            }
        }
    }
    
    public class SimpleOrderRequest
    {
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

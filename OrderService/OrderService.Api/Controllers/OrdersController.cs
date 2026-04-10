using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Api.Data;
using OrderService.Api.Models;
using OrderService.Api.DTOs;

namespace OrderService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(OrderDbContext context, ILogger<OrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.Items)
                .ToListAsync();
            
            // Convert to DTOs to avoid circular references
            var orderDTOs = orders.Select(o => new OrderDTO
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                Items = o.Items.Select(i => new OrderItemDTO
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            }).ToList();
            
            return Ok(orderDTOs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);
            
            if (order == null)
                return NotFound();
            
            // Convert to DTO
            var orderDTO = new OrderDTO
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                Items = order.Items.Select(i => new OrderItemDTO
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };
            
            return Ok(orderDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                _logger.LogInformation($"Creating order for customer: {request.CustomerId}");
                
                // Create order without validation first
                var order = new Order
                {
                    CustomerId = request.CustomerId,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = request.TotalAmount,
                    Status = "Pending",
                    Items = new List<OrderItem>()
                };
                
                // Add order items if provided
                if (request.Items != null && request.Items.Any())
                {
                    foreach (var item in request.Items)
                    {
                        order.Items.Add(new OrderItem
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice
                        });
                    }
                }
                
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation($"Order {order.Id} created successfully");
                
                // Return DTO
                var orderDTO = new OrderDTO
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    Items = order.Items.Select(i => new OrderItemDTO
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    }).ToList()
                };
                
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, orderDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            if (id != order.Id)
                return BadRequest();
            
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();
            
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            
            if (order == null)
                return NotFound();
            
            if (order.Status != "Pending")
                return BadRequest("Only pending orders can be cancelled");
            
            order.Status = "Cancelled";
            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Order {id} cancelled");
            
            return Ok(new { message = "Order cancelled successfully", orderId = id });
        }
    }
    
    // Request DTOs
    public class CreateOrderRequest
    {
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemRequest>? Items { get; set; }
    }
    
    public class OrderItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}

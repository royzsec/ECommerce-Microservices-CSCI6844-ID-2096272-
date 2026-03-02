using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Api.Data;
using PaymentService.Api.Dtos;
using PaymentService.Api.Models;

namespace PaymentService.Api.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly PaymentDbContext _db;

    public PaymentsController(PaymentDbContext db)
    {
        _db = db;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var payment = await _db.Payments.FirstOrDefaultAsync(p => p.Id == id);
        if (payment is null) return NotFound();
        return Ok(payment);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var payments = await _db.Payments.ToListAsync();
        return Ok(payments);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePaymentRequest req)
    {
        if (req.OrderId <= 0) return BadRequest("OrderId must be > 0");
        if (req.Amount <= 0) return BadRequest("Amount must be > 0");

        var payment = new Payment
        {
            OrderId = req.OrderId,
            Amount = req.Amount
        };

        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();

        return Ok(payment);
    }
}
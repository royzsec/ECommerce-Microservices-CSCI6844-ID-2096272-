using Microsoft.EntityFrameworkCore;
using OrderService.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB
var conn = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<OrdersDbContext>(opt => opt.UseSqlite(conn));

builder.Services.AddHttpClient("ProductService", client =>
{
    var baseUrl = builder.Configuration["ProductService:BaseUrl"]!;
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddHttpClient("CustomerService", client =>
{
    var baseUrl = builder.Configuration["CustomerService:BaseUrl"]!;
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddHttpClient("PaymentService", client =>
{
    var baseUrl = builder.Configuration["PaymentService:BaseUrl"]!;
    client.BaseAddress = new Uri(baseUrl);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
    db.Database.EnsureCreated();
}

app.Run();
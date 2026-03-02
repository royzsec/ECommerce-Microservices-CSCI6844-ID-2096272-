using Microsoft.EntityFrameworkCore;
using ProductService.Api.Data;
using ProductService.Api.Messaging;
using ProductService.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB per service
var conn = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ProductDbContext>(opt => opt.UseSqlite(conn));



var app = builder.Build();

// Minimal seed (1 product)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    db.Database.EnsureCreated();

    if (!db.Products.Any())
    {
        db.Products.Add(new Product
        {
            Name = "Test Product",
            Price = 10.00m,
            Stock = 20
        });
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
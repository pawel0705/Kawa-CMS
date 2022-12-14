using KawaSklep.Data;
using KawaSklep.Services.Customer;
using KawaSklep.Services.Inventory;
using KawaSklep.Services.Order;
using KawaSklep.Services.Product;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<CaffeeDbContext>(options =>
{
    options.EnableDetailedErrors();
    options.UseNpgsql(builder.Configuration.GetConnectionString("coffee.dev"), b => b.MigrationsAssembly("KawaSklep.Web"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IInventoryService, InventoryService>();
builder.Services.AddTransient<IOrderService, OrderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(
    builder =>
    builder.
    WithOrigins(
        "http://localhost:8080",
        "http://localhost:8091",
        "http://localhost:8082")
.AllowAnyMethod()
.AllowAnyHeader()
.AllowCredentials()
    );

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

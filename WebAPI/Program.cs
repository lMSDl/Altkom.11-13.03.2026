using Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<List<int>>([.. Enumerable.Repeat(0, 100).Select(x => Random.Shared.Next())]);

builder.Services.AddSingleton<List<ShoppingList>>([
    new ShoppingList { Id = 1, Name = "Groceries" },
    new ShoppingList { Id = 2, Name = "Hardware Store" },
    new ShoppingList { Id = 3, Name = "Pharmacy" }
    ]);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();

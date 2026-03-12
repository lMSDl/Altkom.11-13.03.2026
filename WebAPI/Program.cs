using Bogus;
using Models;
using Services.Bogus;
using Services.Bogus.Fakers;
using Services.InMemory;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<List<int>>([.. Enumerable.Repeat(0, 100).Select(x => Random.Shared.Next())]);

//builder.Services.AddSingleton<IGenericService<ShoppingList>, ShoppingListsService>();
builder.Services.AddSingleton<IGenericService<ShoppingList>, BogusGenericService<ShoppingList>>();
//builder.Services.AddSingleton<IGenericService<Person>, GenericService<Person>>();
//builder.Services.AddSingleton<IGenericService<Person>, PeopleService>();
//builder.Services.AddSingleton<IPeopleService, PeopleService>();
builder.Services.AddSingleton<IPeopleService, BogusPeopleService>();
//udostępniamy serwis PeopleService jako implementację interfejsu IGenericService<Person>, dzięki czemu kontroler GenericController<Person> będzie mógł korzystać z metod tego serwisu do obsługi zapytań dotyczących osób
builder.Services.AddTransient<IGenericService<Models.Person>>(x => x.GetRequiredService<IPeopleService>());
//builder.Services.AddSingleton<IGenericService<Product>, GenericService<Product>>();
builder.Services.AddSingleton<IGenericService<Product>, BogusGenericService<Product>>();

builder.Services.AddTransient<Faker<Models.Person>, PersonFaker>();
builder.Services.AddTransient<Faker<ShoppingList>, ShoppingListFaker>();
builder.Services.AddTransient<Faker<Product>, ProductFaker>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();

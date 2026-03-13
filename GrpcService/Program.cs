using Bogus;
using GrpcService.Services;
using Services.Bogus;
using Services.Bogus.Fakers;
using Services.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IPeopleService>(x => new BogusPeopleService(x.GetService<Faker<Models.Person>>(),10));
builder.Services.AddTransient<Faker<Models.Person>>(x => new PersonFaker("pl"));

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<PeopleService>();
app.MapGrpcService<StreamService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

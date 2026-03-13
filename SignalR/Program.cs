using SignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

var app = builder.Build();


app.MapHub<DemoHub>("/demohub");

app.MapGet("/", () => "Hello World!");

app.Run();

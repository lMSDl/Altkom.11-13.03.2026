using HealthChecks.Checks;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddCheck(nameof(DirecotryAccessHealth), new DirecotryAccessHealth() { DirectoryPath = "C:\\SomeDirectory" })
    .AddSqlServer("Data Source=(local); Initial Catalog=dotnet;Integrated Security=true;TrustServerCertificate=true");


builder.Services.AddHealthChecksUI().AddInMemoryStorage();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(x => {
    x.UIPath = "/health-ui";
});

app.Run();

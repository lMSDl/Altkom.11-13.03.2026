var builder = WebApplication.CreateBuilder(args);
//miejsce na dodanie usług do kontenera DI, np. builder.Services.AddControllers();


//budowanie potoku przetwarzania żądań HTTP, czyli dodawanie middleware'ów do potoku
var app = builder.Build();



//gdzieś na początku (niejawnie) znajduje się middleware obsługujący routing do endpointów
//app.UseRouting();

//middleware pośredniczący, który będzie wykonywał kod przed i po wywołaniu następnego middleware'a w potoku
app.Use(async (context, next) =>
{
    // Do something before the next middleware
    Console.WriteLine("Before Use1");

    Console.WriteLine(context.GetEndpoint()?.DisplayName);

    await next.Invoke();
    
    // Do something after the next middleware
    Console.WriteLine("After use1");
});


//Map - pozwala na zdefiniowanie podścieżki, dla której będzie wykonywany określony kod
//podścieżka ma własny potok przetwarzania żądań, więc można w nim definiować middleware'y, które będą wykonywane tylko dla tej podścieżki
app.Map("/map", mapApp =>
{
    mapApp.Run(async (context) =>
    {
        await context.Response.WriteAsync("Hello from Map Run!");
    });

});


//jeśli jawnie dodamy Routing, to będzie on działał od momentu jego dodania, czyli middleware'y dodane przed UseRouting nie będą miały dostępu do informacji o endpointach, a middleware'y dodane po UseRouting będą miały dostęp do informacji o endpointach
app.UseRouting();

app.Use(async (context, next) =>
{
    // Do something before the next middleware
    Console.WriteLine("Before Use2");

    Console.WriteLine(context.GetEndpoint()?.DisplayName);

    await next.Invoke();

    // Do something after the next middleware
    Console.WriteLine("After use2");
});




//skrót do definiowania endpointów
//app.MapGet("/", () => "Hello World!");


//na końcu poroku niejawnie znajduje się middleware z endpointami
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    });
});

//middleware terminalny, który kończy przetwarzanie żądania i zwraca odpowiedź do klienta
app.Run(async (context) =>
{
    await context.Response.WriteAsync("Dead end!");
});

app.Run();

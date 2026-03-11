using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<List<int>>([.. Enumerable.Repeat(0, 100).Select(x => Random.Shared.Next())]);


var app = builder.Build();

/*if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}*/

//Minimal API - mapowanie endpointów do funkcji lambda, które obsługują żądania HTTP.
//parametry wyrażenia lambda są automatycznie wypełniane przez framework na podstawie danych z żądania HTTP (np. query string, route parameters, body itp.)
app.MapGet("/values", (List<int> values) => values );
//index - musi być liczbą całkowitą, większą lub równą 0 i mniejszą lub równą 99
//[Authorize] - możemy dodawać adnotacje do metod anonimowych, które są mapowane do endpointów
app.MapGet("/values/{index:int:min(0):max(99)}", /*[Authorize]*/ (List<int> values, int index) => values[index] );

app.MapDelete("/values/{index:int}", (List<int> values, int index) => values.RemoveAt(index) );

//jeśli nie podamy parametru w path, to będzie on szukany w query stringu, np. /values?index=5&newValue=10
app.MapPut("/values/{index:int}", (List<int> values, int index, int newValue) => values[index] = newValue );
app.MapPost("/values/{newValue:int}", (List<int> values, int newValue) => values.Add(newValue) );



app.Run();

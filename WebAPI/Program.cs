using Bogus;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Bogus;
using Services.Bogus.Fakers;
using Services.InMemory;
using Services.Interfaces;
using System.Text.Json.Serialization;
using WebAPI.Filters;
using WebAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    /*.AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.Always;
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    })*/
    .AddNewtonsoftJson(x =>
    {
        x.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        x.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
        x.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
    })
    //obsługa xml - Accept klienta musi być ustawiony na application/xml
    .AddXmlDataContractSerializerFormatters();




builder.Services.AddSingleton<List<int>>([.. Enumerable.Repeat(0, 100).Select(x => Random.Shared.Next())]);

//builder.Services.AddSingleton<IGenericService<ShoppingList>, ShoppingListsService>();
//builder.Services.AddSingleton<IGenericService<ShoppingList>, BogusGenericService<ShoppingList>>();
builder.Services.AddSingleton<IGenericService<ShoppingList>>(x => new BogusGenericService<ShoppingList>(
                                            x.GetRequiredService<Faker<ShoppingList>>(),
                                            //ręcznie wstrzykiwanie konfiguracji do serwisu BogusGenericService, dzięki czemu możemy łatwo zarządzać liczbą generowanych zasobów poprzez zmianę wartości w pliku konfiguracyjnym, bez konieczności modyfikowania kodu serwisu
                                            x.GetRequiredService<IConfiguration>().GetValue<int>("Bogus:NumberOfResources")   ));
//builder.Services.AddSingleton<IGenericService<Person>, GenericService<Person>>();
//builder.Services.AddSingleton<IGenericService<Person>, PeopleService>();
//builder.Services.AddSingleton<IPeopleService, PeopleService>();
builder.Services.AddSingleton<IPeopleService, BogusPeopleService>();
//udostępniamy serwis PeopleService jako implementację interfejsu IGenericService<Person>, dzięki czemu kontroler GenericController<Person> będzie mógł korzystać z metod tego serwisu do obsługi zapytań dotyczących osób
builder.Services.AddTransient<IGenericService<Models.Person>>(x => x.GetRequiredService<IPeopleService>());
//builder.Services.AddSingleton<IGenericService<Product>, GenericService<Product>>();
builder.Services.AddSingleton<IGenericService<Product>>(x => new BogusGenericService<Product>(
                                            x.GetRequiredService<Faker<Product>>(),
                                            x.GetRequiredService<IConfiguration>().GetSection("Bogus").GetValue<int>("NumberOfNestedResources")));

builder.Services.AddTransient<Faker<Models.Person>, PersonFaker>();
builder.Services.AddTransient<Faker<ShoppingList>, ShoppingListFaker>();
builder.Services.AddTransient<Faker<Product>, ProductFaker>();

//wstrzykiwanie konfiguracji przez binding
builder.Services.AddTransient<BogusConfig>(x => x.GetRequiredService<IConfiguration>().GetSection("Bogus").Get<BogusConfig>()!);

//wstrzykiwanie konfiguracji przez IOptions
builder.Services.AddOptions<BogusConfig>()
    .Bind(builder.Configuration.GetSection("Bogus"))
    //możliwa walidacja danych z konfiguracji, która pozwala na wczesne wykrycie błędów w konfiguracji, zanim aplikacja zacznie działać. Dzięki temu możemy mieć pewność, że nasza aplikacja będzie działać poprawnie i nie napotka problemów związanych z nieprawidłową konfiguracją podczas działania
    .ValidateDataAnnotations()
    .Validate(x => x.NumberOfResources > 0, "NumberOfResources must be greater than 0")
    .Validate(x => x.NumberOfNestedResources > 0, "NumberOfNestedResources must be greater than 0")
    .ValidateOnStart();

//zawieszenie automatycznej walidacji modelu, dzięki czemu będziemy mogli sami zdecydować, kiedy i jak chcemy walidować dane wejściowe w naszych kontrolerach, co daje nam większą kontrolę nad procesem walidacji i pozwala na bardziej elastyczne podejście do obsługi błędów walidacji
builder.Services.Configure<ApiBehaviorOptions>(x => x.SuppressModelStateInvalidFilter = true);

//podejscie legacy ( dla wersji < 12)
//builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IValidator<ShoppingList>, ShoppingListValidator>();
builder.Services.AddTransient<ConsoleFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();

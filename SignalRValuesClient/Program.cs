using Microsoft.AspNetCore.SignalR.Client;

var httpClient = new HttpClient();
var apiClient = new WebApi.ApiClient(httpClient);

var signalR = new HubConnectionBuilder()
    .WithUrl("https://localhost:5114/SignalR/values")
    .WithAutomaticReconnect()
    .Build();

var values = (await apiClient.ValuesAllAsync()).ToList();

foreach (var value in values)
{
    Console.WriteLine($"Value from API: {value}");
}

signalR.On<int>("ValueAdded", AddValue);
signalR.On<int>("ValueRemoved", RemoveValue);
signalR.On<int, int>("ValueUpdated", (oldValue, newValue) =>
{
    values.RemoveAt(oldValue);
    AddValue(newValue);
});

await signalR.StartAsync();


Console.ReadLine();

foreach (var value in values)
{
    Console.WriteLine($"Value from API: {value}");
}


Console.ReadLine();

void AddValue(int value)
{
    values.Add(value);
    Console.WriteLine($"Added value: {value}");
}

void RemoveValue(int value)
{
    values.Remove(value);
    Console.WriteLine($"Removed value: {value}");
}
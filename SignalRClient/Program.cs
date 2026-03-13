

using Microsoft.AspNetCore.SignalR.Client;

var signalR = new HubConnectionBuilder()
    .WithUrl("http://localhost:5105/demohub")
    .WithAutomaticReconnect()
        .Build();

signalR.On<string>("HelloMessage", message =>
{
    Console.WriteLine(message);
});
signalR.On<string>(nameof(UserConnected), UserConnected);
signalR.On<DemoRecord>("ReceiveDemo", ReadDemoRecord);

signalR.Reconnected += connectionId =>
{
    Console.WriteLine($"Reconnected with connection ID: {connectionId}");
    return Task.CompletedTask;
};

signalR.Reconnecting += error =>
{
    Console.WriteLine($"Reconnecting due to error: {error?.Message}");
    return Task.CompletedTask;
};

await signalR.StartAsync();

var groupName = Console.ReadLine();
await signalR.SendAsync("JoinToGroup", groupName);

var result = await signalR.InvokeAsync<DemoRecord>("SendDemo");
Console.WriteLine($"{result.Name} {result.Value}");



Console.ReadLine();


void UserConnected(string message)
{
    Console.WriteLine(message);
}

void ReadDemoRecord(DemoRecord demoRecord)
{
    Console.WriteLine($"Received DemoRecord: Name={demoRecord.Name}, Value={demoRecord.Value}");
}


class DemoRecord(string name, int value)
{
    public string Name { get; set; } = name;
    public int Value { get; set; } = value;
}
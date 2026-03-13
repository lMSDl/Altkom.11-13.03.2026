
using Grpc.Net.Client;
using GrpcService.Protos.StreamsProto;
var channel = GrpcChannel.ForAddress("https://localhost:7133");


var streamClient = new GrpcService.Protos.StreamsProto.GrpcStream.GrpcStreamClient(channel);
var downStream = streamClient.FromServer(new Request { Message = "Hello from client" }, deadline: DateTime.UtcNow.AddSeconds(10));

var token = new CancellationTokenSource();
//token.CancelAfter(10000);
token.CancelAfter(5000);
try
{
    while (await downStream.ResponseStream.MoveNext(token.Token))
    {
        Console.WriteLine(downStream.ResponseStream.Current.Message);
    }
}
catch (Exception e)
{
    Console.WriteLine("Streaming cancelled");
}
token.Dispose();


var upStream = streamClient.FromClient();
foreach (var letter in "ala ma kota")
{
    await upStream.RequestStream.WriteAsync(new Request { Message = letter.ToString() });
}
await upStream.RequestStream.CompleteAsync();
var response = await upStream.ResponseAsync;

Console.WriteLine(response.Message);



var streams = streamClient.Bidirectional();

_ = Task.Run(async () =>
{
    for (int i = 0; i < int.MaxValue; i++)
    {
        if (i % 2 == 0)
        {
            await streams.RequestStream.WriteAsync(new Request { Message = $"Message {i} from client" });
        }
        else
        {
            await streams.RequestStream.WriteAsync(new Request { Message = $"Inny komunikat {i} od klienta" });
        }
        await Task.Delay(750);
    }
});

_ = Task.Run(async () =>
{
    try
    {
        while (await streams.ResponseStream.MoveNext(CancellationToken.None))
        {
            Console.WriteLine(streams.ResponseStream.Current.Message);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("Bidirectional streaming cancelled");
    }
});


Console.ReadLine();



static async Task Unary(GrpcChannel channel)
{
    var client = new GrpcService.Protos.PeopleProto.PeopleGrpcService.PeopleGrpcServiceClient(channel);


    var people = await client.ReadAsync(new GrpcService.Protos.PeopleProto.Void());

    foreach (var item in people.Collection)
    {
        Console.WriteLine(item.FirstName + " " + item.LastName);
    }

    Console.ReadLine();

    var p1 = new GrpcService.Protos.PeopleProto.Person { FirstName = "John", LastName = "Doe" };
    p1.Id = (await client.CreateAsync(p1)).Id_;

    var p2 = await client.ReadByIdAsync(new GrpcService.Protos.PeopleProto.Id { Id_ = 4 });
    p1.LastName = p2.Person.LastName;
    await client.UpdateAsync(p1);
    try
    {
        await client.UpdateAsync(new GrpcService.Protos.PeopleProto.Person() { Id = 124 });
    }
    catch (Grpc.Core.RpcException ex)
    {
        Console.WriteLine(ex.Status);
    }
}
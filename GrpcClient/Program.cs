
using Grpc.Net.Client;

var channel = GrpcChannel.ForAddress("https://localhost:7133");
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
    await client.UpdateAsync(new GrpcService.Protos.PeopleProto.Person() { Id = 124});
}
catch (Grpc.Core.RpcException ex)
{
    Console.WriteLine(ex.Status);
}
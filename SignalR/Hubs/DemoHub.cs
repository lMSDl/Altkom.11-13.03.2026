using Microsoft.AspNetCore.SignalR;

namespace SignalR.Hubs
{
    public class DemoHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            Console.WriteLine(Context.ConnectionId);

            await Clients.Caller.SendAsync("HelloMessage", "Welcome to the DemoHub! Your connection ID is: " + Context.ConnectionId);
            await Clients.Others.SendAsync("UserConnected", $"A new user has connected: {Context.ConnectionId}");
        }



        public async Task JoinToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.OthersInGroup(groupName).SendAsync("UserConnected", $"User {Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task<DemoRecord> SendDemo()
        {
            var demoRecord = new DemoRecord("DemoName", 42);
            await Clients.All.SendAsync("ReceiveDemo", demoRecord);
            return demoRecord;
        }



    }
    public record DemoRecord(string name, int value);
}

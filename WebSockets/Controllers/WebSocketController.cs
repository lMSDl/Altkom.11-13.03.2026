using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;

namespace WebSockets.Controllers
{
    public class WebSocketController : ControllerBase
    {

        //[HttpGet("/ws")] //http/1.1
        [Route("/ws")] //http/2
        public async Task Ws()
        {
            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            var helloMessage = "Hello WebSocket!";

            {

                var helloBytes = System.Text.Encoding.UTF8.GetBytes(helloMessage);
                await webSocket.SendAsync(
                    //new ArraySegment<byte>(helloBytes),
                    new ArraySegment<byte>(helloBytes, 0, 5),
                    System.Net.WebSockets.WebSocketMessageType.Text,
                    false, //flaga oznaczająca, że to nie jest ostatnia część wiadomości
                    CancellationToken.None);
                await webSocket.SendAsync(
                    //new ArraySegment<byte>(helloBytes),
                    new ArraySegment<byte>(helloBytes, 5, helloBytes.Length - 5),
                    System.Net.WebSockets.WebSocketMessageType.Text,
                    true, //flaga oznaczająca, że to jest ostatnia część wiadomości
                    CancellationToken.None);
            }

            var buffer = new byte[1024];
            WebSocketReceiveResult receiveResult;
            StringBuilder stringBuilder = new();
            do
            {
                stringBuilder.Clear();
                do
                {
                    receiveResult = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                    stringBuilder.Append(Encoding.UTF8.GetString(buffer, 0, receiveResult.Count));
                } while (!receiveResult.EndOfMessage);

                await webSocket.SendAsync(
                    new ArraySegment<byte>(Encoding.UTF8.GetBytes(stringBuilder.ToString())),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None);

                /*var message = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                Console.WriteLine(message);

                await webSocket.SendAsync(
                    new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                    receiveResult.MessageType,
                    receiveResult.EndOfMessage,
                    CancellationToken.None);*/

            } while (!receiveResult.CloseStatus.HasValue);
        }
    }
}

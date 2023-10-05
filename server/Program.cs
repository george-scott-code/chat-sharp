// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;
using shared;

Console.WriteLine("Hello, World!");
IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("localhost");
IPAddress ipAddress = ipHostInfo.AddressList[0];
IPEndPoint ipEndPoint = new(ipAddress, 11_000);

using Socket listener = new(
    ipEndPoint.AddressFamily,
    SocketType.Stream,
    ProtocolType.Tcp);

listener.Bind(ipEndPoint);
listener.Listen(100);

var handler = await listener.AcceptAsync();
while (true)
{
    // Receive message.
    var buffer = new byte[1_024];
    var received = await handler.ReceiveAsync(buffer, SocketFlags.None); 
    var response = Encoding.UTF8.GetString(buffer, 0, received);

    if (response.IndexOf(MessageDelimeters.END_OF_MESSAGE) > -1) // is end of message
    {
        System.Console.WriteLine(
            $"Socket server recieved message: {response.Replace(MessageDelimeters.END_OF_MESSAGE, string.Empty)}"
        );
    }

    // Send acknowledgment
    var echoBytes = Encoding.UTF8.GetBytes(MessageDelimeters.ACK_MESSAGE);
    await handler.SendAsync(echoBytes, 0);
    System.Console.WriteLine(
        $"Socket server sent acknowledgment: {MessageDelimeters.ACK_MESSAGE}"
    );
    break;
}
listener.Close();
// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;
using shared;

Console.WriteLine("Hello, World!");

IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("localhost");
IPAddress ipAddress = ipHostInfo.AddressList[0];
IPEndPoint ipEndPoint = new(ipAddress, 11_000);

using Socket client = new(
    ipEndPoint.AddressFamily,
    SocketType.Stream,
    ProtocolType.Tcp);

await client.ConnectAsync(ipEndPoint);
string? message = string.Empty;
while (true)
{
    // Send message
    message = Console.ReadLine();
    if(message == "q")
    {
        break;
    }
    if (message != null)
    {
        var messageBytes = Encoding.UTF8.GetBytes($"{message}{MessageDelimeters.END_OF_MESSAGE}");

        _ = await client.SendAsync(messageBytes, SocketFlags.None);
        System.Console.WriteLine($"Socket client sent message {message}");
        
        // Recieve acknowledgment
        var buffer = new byte[1_024];
        var received = await client.ReceiveAsync(buffer, SocketFlags.None);
        var response = Encoding.UTF8.GetString(buffer, 0, received);
        if(response == MessageDelimeters.ACK_MESSAGE)
        {
            System.Console.WriteLine($"Socket client recieved acknowledgment: {response}");
        }
        else
        {
            System.Console.WriteLine("no acknowledgment received");
        }
    }
}

client.Shutdown(SocketShutdown.Receive);
client.Close();
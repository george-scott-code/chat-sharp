// See https://aka.ms/new-console-template for more information
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
// https://stackoverflow.com/questions/177856/how-do-i-trap-ctrlc-sigint-in-a-c-sharp-console-app

//TODO:
public class Server
{
    // access from multiple threads
    ConcurrentBag<Client> users = new ();

    // accept multiple connections
    // add event handler to connected users
    public async void AcceptClients()
    {
        
        IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("localhost");
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint ipEndPoint = new(ipAddress, 11_000);

        using Socket listener = new(
            ipEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);

        listener.Bind(ipEndPoint);
        while(true)
        {
            var socket = await listener.AcceptAsync();
            users.Add(new Client(socket));
        }
    }
}

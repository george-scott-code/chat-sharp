// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;

public class Client
{
    Socket clientSocket;
    public Client(Socket socket)
    {
        clientSocket = socket;
    }
}

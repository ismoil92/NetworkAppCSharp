using NetworkAppCSharp.Abstracts;
using NetworkAppCSharp.Models;
using NetworkAppCSharp.Services;
using System.Net;

namespace ServerTest;

public class MockMessageSource : IMessageSource
{

    private Queue<NetMessage> messages = new();
    private Server server;
    private IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);


    public MockMessageSource()
    {
        messages.Enqueue(new NetMessage { Command = Command.Register, NickNameFrom = "Вася" });
        messages.Enqueue(new NetMessage { Command = Command.Register, NickNameFrom = "Юля" });
        messages.Enqueue(new NetMessage { Command = Command.Message, NickNameFrom = "Юля", NickNameTo = "Вася", Text = "От Юли" });
        messages.Enqueue(new NetMessage { Command = Command.Message, NickNameFrom = "Вася", NickNameTo = "Юля", Text = "От Васи" });
    }

    public void AddServer(Server srv) => server=srv;




    public NetMessage Receive(ref IPEndPoint ep)
    {
        ep = endPoint;

        if(messages.Count == 0)
        {
            server.Stop();
            return null!;
        }

        var msg = messages.Dequeue();

        return msg;
    }

    public Task SendAsync(NetMessage message, IPEndPoint ep)
    {
        throw new NotImplementedException();
    }
}
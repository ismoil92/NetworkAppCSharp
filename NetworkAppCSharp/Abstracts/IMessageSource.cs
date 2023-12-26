using NetworkAppCSharp.Models;
using System.Net;

namespace NetworkAppCSharp.Abstracts;

public interface IMessageSource
{
    Task SendAsync(NetMessage message, IPEndPoint ep);

    NetMessage Receive(ref IPEndPoint ep);
}
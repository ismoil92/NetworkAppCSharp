using ChatCommon;
using System.Net;
using System.Security;

namespace ChatApp.Abstracts;

public interface IMessageSourceServer<T>
{
    Task SendAsync(NetMessage message, T ep);
    NetMessage Receive(ref T ep);
    T CreateEndpoint();
    T CopyEndpoint(IPEndPoint ep);
}
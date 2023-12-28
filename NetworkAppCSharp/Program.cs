using ChatApp.UDPClient;
using ChatApp.UDPServer;
using System.Net;

Console.WriteLine("Введите строку");
string input = Console.ReadLine()!;
string[] texts = input.Split(" ");

if(texts.Length == 0 || string.IsNullOrWhiteSpace(input))
{
    var s = new Server<IPEndPoint>(new UdpMessageSourceServer());
    await s.Start();
}
else if(texts.Length == 1)
{
    var c = new Client<IPEndPoint>(new UdpMessageSourceClient(), texts[0]);
    await c.Start();
}
else
{
    Console.WriteLine("Для старта сервера передайте в конструктор типа класс UdpMessageSourceServer");
    Console.WriteLine("Для старта клиента передайте в конструктор типа класс UdpMessageSourceClient и имя типа строки");
}

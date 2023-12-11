using System.Net;
using System.Net.Sockets;
using System.Text;
using Server;

namespace Client;

public class ClientApplication
{
    /// <summary>
    /// Статический метод, для отправление сообщение
    /// </summary>
    /// <param name="from">отправитель сообщение</param>
    /// <param name="IP">IP адрес сервера</param>
    private static void SendMessage(string from, string IP)
    {
        UdpClient udpClient = new UdpClient();
        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(IP), 12345);

        string messageText = string.Empty;
            do
            {
                Console.Clear();
                Console.Write("Введите сообщение:");
                messageText = Console.ReadLine()!;
            } while (string.IsNullOrEmpty(messageText));


      var message = new Message() { Text =messageText, NicknameFrom = from, DateTime = DateTime.Now, NicknameTo = "Server" };
      var json =  message.SerializeMessageToJson();

        message.PrintInformation("Сервер отправил сообщение");
        byte[] buffer = Encoding.UTF8.GetBytes(json);
        udpClient.Send(buffer, buffer.Length, iPEndPoint);
    }

    /// <summary>
    /// Статический метод, для ввода через консоль строку и передает две строку на другой статический метод SendMessage()
    /// </summary>
    public static void ConsoleReadlineToString()
    {
        Console.Write("Введите имя отправителя:");
        string? nicknameFrom = Console.ReadLine();
        Console.Write("Введите IP адрес:");
        string? ipAddress = Console.ReadLine();

        SendMessage(nicknameFrom!, ipAddress!);
    }
}
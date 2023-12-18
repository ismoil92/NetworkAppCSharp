using Server;
using System.Net.Sockets;
using System.Net;
using System.Text;

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
        MessageBuilder messageBuilder = new MessageBuilder();
        string messageText = string.Empty;
        do
        {
            Console.WriteLine();
            Console.Write("Введите сообщение:");
            messageText = Console.ReadLine()!;


            messageBuilder.SetText(messageText);
            messageBuilder.SetNickNameTo(from);
            messageBuilder.SetNickNameTo("Server");
            messageBuilder.SetDateTime(DateTime.Now);

            var message = messageBuilder.Build();
            var json = message.SerializeMessageToJson();

            message.PrintInformation();
            byte[] data = Encoding.UTF8.GetBytes(json);
            udpClient.Send(data, data.Length, iPEndPoint);

            byte[] buffer = udpClient.Receive(ref iPEndPoint);
            var answer = Encoding.UTF8.GetString(buffer);

            Console.WriteLine(answer);


        } while (!(messageText == "Exit"));
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
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;

public class ServerApplication
{
    /// <summary>
    /// Статический метод, для работы с сервером. Сервер получает информацию и выводет на консоль
    /// </summary>
    public static void Server()
    {
        UdpClient udpClient = new UdpClient(12345);
        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);

        Console.WriteLine("Сервер ждёт сообщение от клиента");

        while(true)
        {


            byte[] buffer = udpClient.Receive(ref iPEndPoint);
            if(buffer == null) break;

            var messageText = Encoding.UTF8.GetString(buffer);

            Message? message = Message.DesearializeMessageFromJson(messageText);

            message?.PrintInformation("Клиент отправил сообщение");
        }
    }
}
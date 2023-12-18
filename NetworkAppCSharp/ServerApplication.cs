using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server;

public class ServerApplication
{
    /// <summary>
    /// Статический метод, для работы с сервером. Сервер получает информацию и выводет на консоль
    /// </summary>
    public static void Server()
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;

        UdpClient udpClient = new UdpClient(12345);
        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);

        Console.WriteLine("Сервер ждёт сообщение от клиента");

        while (!cts.IsCancellationRequested)
        {
            byte[] buffer = udpClient.Receive(ref iPEndPoint);
            if (buffer == null) break;

            var messageText = Encoding.UTF8.GetString(buffer);

            Task.Run(() =>
            {
                Message? message = Message.DesearializeMessageFromJson(messageText);
                if (message?.Text == "Exit")
                {
                    cts.Cancel();
                }
                message?.PrintInformation();
                byte[] reply = Encoding.UTF8.GetBytes("Сообщение получено");
                udpClient.Send(reply, reply.Length, iPEndPoint);
            }).Wait();
        }


    }
}
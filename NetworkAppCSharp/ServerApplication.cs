using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server;

public class ServerApplication
{

   static Dictionary<string, IPEndPoint> clients = new Dictionary<string, IPEndPoint>();
   static UdpClient? udpClient;

    /// <summary>
    /// Статический метод, для регистрация пользовтеля и добавление в базу
    /// </summary>
    /// <param name="message">Объект типа NetMessage для получение информация о сообщений</param>
    /// <param name="fromep">Объект типа IPEndPoint, для получение ip пользователя</param>
   private static void Register(NetMessage message, IPEndPoint fromep)
    {
        Console.WriteLine("Message Register, name = " + message.NickNameFrom);
        clients.Add(message.NickNameFrom!, fromep);


        using (var ctx = new ChatContext())
        {
            if (ctx.Users.FirstOrDefault(x => x.FullName == message.NickNameFrom) != null) return;

            ctx.Add(new User { FullName = message.NickNameFrom });

            ctx.SaveChanges();
        }
    }
    
    /// <summary>
    /// Статический метод, для потверждение получение сообщение
    /// </summary>
    /// <param name="id">id пользователя</param>
    private static void ConfirmMessageReceived(int? id)
    {
        Console.WriteLine("Message confirmation id=" + id);

        using (var ctx = new ChatContext())
        {
            var msg = ctx.Messages.FirstOrDefault(x => x.MessageId == id);

            if (msg != null)
            {
                msg.IsSent = true;
                ctx.SaveChanges();
            }
        }
    }


    /// <summary>
    /// Статический метод, информация получен ли сообщение или нет
    /// </summary>
    /// <param name="message">Объект типа NetMessage информация о сообщение</param>
    private static void RelyMessage(NetMessage message)
    {
        int? id = null;
        if (clients.TryGetValue(message.NickNameTo!, out IPEndPoint? ep))
        {
            using (var ctx = new ChatContext())
            {
                var fromUser = ctx.Users.First(x => x.FullName == message.NickNameFrom);
                var toUser = ctx.Users.First(x => x.FullName == message.NickNameTo);
                var msg = new Message() { UserFrom = fromUser, UserTo = toUser, IsSent = false, Text = message.Text };
                ctx.Messages.Add(msg);

                ctx.SaveChanges();

                id = msg.MessageId;
            }


            var forwardMessageJson = new NetMessage()
            {
                Id = id,
                Commands = Commands.Message,
                NickNameTo = message.NickNameTo,
                NickNameFrom = message.NickNameFrom,
                Text = message.Text
            }.SerialazeMessageToJSON();

            byte[] forwardBytes = Encoding.ASCII.GetBytes(forwardMessageJson);

            udpClient?.Send(forwardBytes, forwardBytes.Length, ep);
            Console.WriteLine($"Message Relied, from = {message.NickNameFrom} to = {message.NickNameTo}");
        }
        else
        {
            Console.WriteLine("Пользователь не найден.");
        }
    }

    /// <summary>
    /// Статический метод, Процесс сообщенение получение
    /// </summary>
    /// <param name="message">Объект типа NetMessage для получение сообщение пользователя</param>
    /// <param name="fromep">Объекти типа IPEndPoint ip пользователя</param>
  private static  void ProcessMessage(NetMessage message, IPEndPoint fromep)
    {
        Console.WriteLine($"Получено сообщение от {message.NickNameFrom} для {message.NickNameTo} с командой {message.Commands}:");
        Console.WriteLine(message.Text);


        if (message.Commands == Commands.Register)
        {
            Register(message, new IPEndPoint(fromep.Address, fromep.Port));

        }
        if (message.Commands == Commands.Confirmation)
        {
            Console.WriteLine("Confirmation receiver");
            ConfirmMessageReceived(message.Id);
        }
        if (message.Commands == Commands.Message)
        {
            RelyMessage(message);
        }
    }


    /// <summary>
    /// Статический метод, для запуска сервера
    /// </summary>
    public static void Work()
    {

        IPEndPoint remoteEndPoint;

        udpClient = new UdpClient(12345);
        remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

        Console.WriteLine("Клиент ожидает сообщений...");

        while (true)
        {
            byte[] receiveBytes = udpClient.Receive(ref remoteEndPoint);
            string receivedData = Encoding.ASCII.GetString(receiveBytes);

            Console.WriteLine(receivedData);

            try
            {
                var message = NetMessage.DeserializeMessgeFromJSON(receivedData);

                ProcessMessage(message!, remoteEndPoint);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при обработке сообщения: " + ex.Message);
            }
        }

    }
}
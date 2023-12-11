using System.Text.Json;

namespace Server;

public class Message
{
    #region PROPERTIES
    public string? Text { get; set; }
    public DateTime DateTime { get; set; }
    public string? NicknameFrom {  get; set; }
    public string? NicknameTo { get; set; }
    #endregion

    /// <summary>
    /// Метод, для сериализация объекта в json строку
    /// </summary>
    /// <returns>Возвращает строку json</returns>
    public string SerializeMessageToJson() => JsonSerializer.Serialize(this);

    /// <summary>
    /// Статический метод, для Десирилизация из json строку в объект Message
    /// </summary>
    /// <param name="message">строка json</param>
    /// <returns>Возвращает объект типа Message</returns>
    public static Message? DesearializeMessageFromJson(string message) => JsonSerializer.Deserialize<Message>(message);

    /// <summary>
    /// Метод, для вывода информации
    /// </summary>
    public void PrintInformation(string message)
    {
        Console.WriteLine(message);
        Console.WriteLine($"Дата {DateTime.Now}, Получение  текст сообщение {Text}, от {NicknameFrom}");
    }
}
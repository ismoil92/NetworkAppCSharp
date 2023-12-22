using System.Text.Json;

namespace Server;

public class NetMessage
{
    #region PROPERTIES
    public int? Id { get; set; }
    public string? Text { get; set; }
    public DateTime DateTime { get; set; }
    public string? NickNameFrom { get; set; }
    public string? NickNameTo { get; set; }

    public Commands Commands { get; set; }
    #endregion


    #region METHODS
    /// <summary>
    /// Метод, для серилизация из объекта Класса NetMessage в строку json
    /// </summary>
    /// <returns>Возвращает строку json</returns>
    public string SerialazeMessageToJSON() => JsonSerializer.Serialize(this);

    /// <summary>
    /// Статический метод, для десерилизации из строку json в объекта класса NetMessage
    /// </summary>
    /// <param name="message">строка json</param>
    /// <returns>Возвращает объект класса NetMessage</returns>
    public static NetMessage? DeserializeMessgeFromJSON(string message) => JsonSerializer.Deserialize<NetMessage>(message);

    /// <summary>
    /// Метод, для вывода информацию о сообщении
    /// </summary>
    public void PrintGetMessageFrom()
    {
        Console.WriteLine(ToString());
    }

    /// <summary>
    /// Переопределенный метод ToString информацию в строку
    /// </summary>
    /// <returns>Возвращает строку информацию</returns>
    public override string ToString()
    {
        return $"{DateTime} \n Получено сообщение {Text} \n от {NickNameFrom}  ";
    }
    #endregion
}
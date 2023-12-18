using Server;

namespace Client;

public class MessageBuilder : Builder
{
    #region FIELDS
    private string? text;
    private string? nickNameFrom;
    private string? nickNameTo;
    private DateTime dateTime;
    #endregion


    /// <summary>
    /// Метод, для установки строку текста сообщений
    /// </summary>
    /// <param name="text">строка текста сообщений</param>
    public void SetText(string text) => this.text = text;

    /// <summary>
    /// Метод, для установки строку имени отправителя
    /// </summary>
    /// <param name="nickNameFrom">имя отправителя</param>
    public void SetNickNameFrom(string nickNameFrom) => this.nickNameFrom = nickNameFrom;

    /// <summary>
    /// Метод, для установки строки имени получателя
    /// </summary>
    /// <param name="nickNameTo">имя получателя</param>
    public void SetNickNameTo(string nickNameTo) => this.nickNameTo = nickNameTo;

    /// <summary>
    /// Метод, для установки Дату и времени сообщение
    /// </summary>
    /// <param name="dateTime">Дата и время</param>
    public void SetDateTime(DateTime dateTime) => this.dateTime = dateTime;


    /// <summary>
    /// Переопределенный метод, для создание класса Message
    /// </summary>
    /// <returns>Возвращает объект типа Message</returns>
    public override Message Build() => new Message
    {
        Text = this.text,
        NicknameFrom = this.nickNameFrom,
        NicknameTo = this.nickNameTo,
        DateTime = this.dateTime
    };
}
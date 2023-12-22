namespace Server;

public class User
{
    #region PROPERTIES
    public virtual List<Message>? MessagesTo { get; set; } = new();
    public virtual List<Message>? MessagesFrom { get; set; } = new();
    public int Id { get; set; }
    public string? FullName { get; set; }
    #endregion
}
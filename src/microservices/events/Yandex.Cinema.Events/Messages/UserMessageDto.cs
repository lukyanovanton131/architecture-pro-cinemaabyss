namespace Yandex.Cinema.Events.Messages;

public class UserMessageDto
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Action { get; set; }
    public DateTime Timestamp { get; set; }
}
namespace Yandex.Cinema.Events.Response;

public class EventDto
{
    public string Id { get; set; }
    public string Type { get; set; }
    public DateTime Timestamp { get; set; }
    public object Payload { get; set; }
}
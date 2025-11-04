namespace Yandex.Cinema.Events.Response;

public class Response
{
    public string Status { get; set; }
    public int Partition { get; set; }
    public long Offset { get; set; }
    public EventDto Event { get; set; }
}
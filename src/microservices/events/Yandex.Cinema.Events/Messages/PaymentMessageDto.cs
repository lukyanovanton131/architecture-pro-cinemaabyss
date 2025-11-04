namespace Yandex.Cinema.Events.Messages;

public class PaymentMessageDto
{
    public int PaymentId { get; set; }
    public int UserId { get; set; }
    public float Amount { get; set; }
    public string Status { get; set; }
    public string MethodType { get; set; }
    public DateTime Timestamp { get; set; }
}
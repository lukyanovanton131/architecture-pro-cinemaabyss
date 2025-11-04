using System.Text.Json.Serialization;

namespace Yandex.Cinema.Events.Requests;

public class PaymentRequest
{
    [JsonPropertyName("payment_id")] public int PaymentId { get; set; }
    [JsonPropertyName("user_id")] public int UserId { get; set; }
    [JsonPropertyName("amount")] public float Amount { get; set; }
    [JsonPropertyName("status")] public string Status { get; set; }
    [JsonPropertyName("method_type")] public string MethodType { get; set; }
    [JsonPropertyName("timestamp")] public DateTime Timestamp { get; set; }
    
}

using System.Text.Json.Serialization;

namespace Yandex.Cinema.Events.Requests;

public class UserRequest
{
    [JsonPropertyName("user_id")] public int UserId { get; set; }
    [JsonPropertyName("username")] public string Username { get; set; }
    [JsonPropertyName("action")] public string Action { get; set; }
    [JsonPropertyName("timestamp")] public DateTime Timestamp { get; set; }
}
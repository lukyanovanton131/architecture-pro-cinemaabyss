using System.Text.Json.Serialization;

namespace Yandex.Cinema.Events.Requests;

public class MovieRequest
{
    [JsonPropertyName("movie_id")] public int MovieId { get; set; }
    [JsonPropertyName("title")] public string Title { get; set; }
    [JsonPropertyName("action")] public string Action { get; set; }
    [JsonPropertyName("user_id")] public int UserId { get; set; }
}
namespace Yandex.Cinema.Events;

public class KafkaOptions
{
    public const string SectionTitle = "Kafka";

    public string BootstrapServers { get; set; }
    public string Username { get; set; }
    
    public string Password { get; set; }
}
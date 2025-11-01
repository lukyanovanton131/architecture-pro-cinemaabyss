namespace Yandex.ApiGateway;

public class MigrationConfig
{
    public int MoviesMigrationPercentage { get; set; }
    public string MoviesServiceUrl { get; set; }
    public string MonolithServiceUrl { get; set; }
}
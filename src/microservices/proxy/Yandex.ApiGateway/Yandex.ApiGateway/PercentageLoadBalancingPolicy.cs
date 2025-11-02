using Microsoft.Extensions.Options;
using Yarp.ReverseProxy.LoadBalancing;
using Yarp.ReverseProxy.Model;

namespace Yandex.ApiGateway;

public class PercentageLoadBalancingPolicy : ILoadBalancingPolicy
{
    private readonly MigrationConfig _migrationConfig;

    public PercentageLoadBalancingPolicy(IOptionsMonitor<MigrationConfig> migrationConfig)
    {
        _migrationConfig = migrationConfig.CurrentValue;
    }
    private readonly Random _random = new Random();
    
    public string Name => "Percentage";

    public DestinationState? PickDestination(
        HttpContext context,
        ClusterState cluster,
        IReadOnlyList<DestinationState> availableDestinations)
    {
        if (availableDestinations.Count == 0)
            return null;

        // Получаем конфигурацию процентов из метаданных кластера
        var percentageConfig = _migrationConfig.MoviesMigrationPercentage;//GetPercentageConfiguration(cluster);
        
        // Генерируем случайное число от 0 до 99
        var randomValue = _random.Next(100);
        
        // Определяем, в какой диапазон попадает случайное число
        var currentPercentage = 0;
        
        if (randomValue < percentageConfig)
        {
            return availableDestinations.FirstOrDefault(d => d.DestinationId == "monolith");
        }
        else
        {
            return availableDestinations.FirstOrDefault(d => d.DestinationId == "movies-service");
        }
    }
}
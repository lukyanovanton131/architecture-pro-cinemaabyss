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
        
        // if(randomValue < percentageConfig)
        //     return new DestinationState("id", new DestinationModel(new DestinationConfig(){Address = _migrationConfig.MoviesServiceUrl}));
        
        if (randomValue < percentageConfig)
        {
            return availableDestinations.FirstOrDefault(d => d.DestinationId == "monolith");
        }
        else
        {
            return availableDestinations.FirstOrDefault(d => d.DestinationId == "movies-service");
        }
        
        
        // foreach (var destination in availableDestinations)
        // {
        //     var percentage = percentageConfig.GetValueOrDefault(destination.DestinationId, 0);
        //     
        //     if (randomValue >= currentPercentage && randomValue < currentPercentage + percentage)
        //     {
        //         return destination;
        //     }
        //     
        //     currentPercentage += percentage;
        // }
        
        // Fallback: возвращаем первое доступное назначение
        //return availableDestinations[0];
    }

//     private Dictionary<string, int> GetPercentageConfiguration(ClusterState cluster)
//     {
//         var config = new Dictionary<string, int>();
//         
//         foreach (var destination in cluster.Destinations.Values)
//         {
//             if (destination.DestinationConfig.Metadata?.TryGetValue("Percentage", out var percentageObj) == true)
//             {
//                 if (int.TryParse(percentageObj?.ToString(), out int percentage))
//                 {
//                     config[destination.DestinationId] = percentage;
//                 }
//             }
//         }
//         
//         return config;
//     }
}
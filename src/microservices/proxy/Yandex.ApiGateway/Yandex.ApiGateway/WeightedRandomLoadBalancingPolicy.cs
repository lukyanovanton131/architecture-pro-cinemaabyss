// using Yarp.ReverseProxy.LoadBalancing;
// using Yarp.ReverseProxy.Model;
//
// namespace Yandex.ApiGateway;
//
// public class WeightedRandomLoadBalancingPolicy : ILoadBalancingPolicy
// {
//     public WeightedRandomLoadBalancingPolicy()
//     {
//         
//     }
//     private readonly Random _random = new Random();
//
//     public string Name => "WeightedRandom";
//
//     public DestinationState? PickDestination(HttpContext context, ClusterState cluster, IReadOnlyList<DestinationState> availableDestinations)
//     {
//         // Если нет доступных назначений, возвращаем null
//         if (availableDestinations.Count == 0)
//         {
//             return null;
//         }
//
//         // Считаем общий вес
//         int totalWeight = 0;
//         foreach (var destination in availableDestinations)
//         {
//             var weight = GetWeight(destination);
//             totalWeight += weight;
//         }
//
//         // Если общий вес 0, то используем равномерное распределение
//         if (totalWeight <= 0)
//         {
//             return availableDestinations[_random.Next(availableDestinations.Count)];
//         }
//
//         // Выбираем случайное число от 0 до totalWeight (не включая)
//         int randomWeight = _random.Next(totalWeight);
//         int accumulatedWeight = 0;
//
//         foreach (var destination in availableDestinations)
//         {
//             var weight = GetWeight(destination);
//             accumulatedWeight += weight;
//             if (randomWeight < accumulatedWeight)
//             {
//                 return destination;
//             }
//         }
//
//         // Это не должно случиться, но на всякий случай возвращаем последний
//         return availableDestinations[availableDestinations.Count - 1];
//     }
//
//     // private static int GetWeight(DestinationState destination)
//     // {
//     //     // Пытаемся получить вес из метаданных, по умолчанию 1
//     //     if (destination.DestinationConfig.Metadata?.TryGetValue("Weight", out var weightObj) ?? false)
//     //     {
//     //         if (weightObj is int weight)
//     //         {
//     //             return weight;
//     //         }
//     //         else if (weightObj is string weightStr && int.TryParse(weightStr, out weight))
//     //         {
//     //             return weight;
//     //         }
//     //     }
//     //
//     //     return 1;
//     // }
// }
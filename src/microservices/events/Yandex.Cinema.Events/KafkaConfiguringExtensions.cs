using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Yandex.Cinema.Events;

public static class KafkaConfiguringExtensions
{
    public static IServiceCollection AddKafkaConsumer<TKey, TValue>(this IServiceCollection services, string groupId,
        JsonSerializerOptions jsonOptions = null)
    {
        return services.AddSingleton<IConsumer<TKey, TValue>>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<KafkaOptions>>().Value;

            var consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = options.BootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                AllowAutoCreateTopics = true,
            };
			
            var builder = new ConsumerBuilder<TKey, TValue>(consumerConfig);
            builder.SetValueDeserializer(new MessageSerializer<TValue>(jsonOptions));
            builder.SetKeyDeserializer(new MessageSerializer<TKey>(jsonOptions));
            return builder.Build();
        });
    }
    public static IServiceCollection AddKafkaProducer<TKey, TValue>(this IServiceCollection services)
    {
        return services.AddSingleton<IProducer<TKey, TValue>>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<KafkaOptions>>().Value;
            var producerConfig = new ProducerConfig()
            {
                Acks = Acks.Leader,
                BootstrapServers = options.BootstrapServers,
                MessageTimeoutMs = 10000,
                SocketTimeoutMs = 5000,
                ApiVersionRequestTimeoutMs = 5000,
                ReconnectBackoffMs = 10000,
                ReconnectBackoffMaxMs = 10000,
                RetryBackoffMs = 1000,
                QueueBufferingMaxMessages = 10,
                QueueBufferingMaxKbytes = 1024,
                LingerMs = 0,
            };
			
            var builder = new ProducerBuilder<TKey, TValue>(producerConfig);
            builder.SetValueSerializer(new MessageSerializer<TValue>());
            builder.SetKeySerializer(new MessageSerializer<TKey>());
            return builder.Build();
        });
    }
}
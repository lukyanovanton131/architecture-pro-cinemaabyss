using Confluent.Kafka;
using Yandex.Cinema.Events.Messages;

namespace Yandex.Cinema.Events;

public class MovieConsumerJob : BaseConsumerBackgroundService<Guid, MovieMessageDto>
{
    public MovieConsumerJob(
        IConsumer<Guid, MovieMessageDto> consumer,
        ILogger<MovieConsumerJob> logger) : base("movie-events", consumer, logger)
    {
    }


    protected override async Task ProcessMessageAsync(ConsumeResult<Guid, MovieMessageDto> consumeResult,
        CancellationToken stoppingToken)
    {
        var messageValue = consumeResult.Message.Value;

        Logger.LogInformation($"Payment: {messageValue}");
    }
}
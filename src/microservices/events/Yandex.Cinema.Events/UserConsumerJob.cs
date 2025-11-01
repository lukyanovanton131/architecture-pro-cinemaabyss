using Confluent.Kafka;
using Yandex.Cinema.Events.Messages;

namespace Yandex.Cinema.Events;

public class UserConsumerJob : BaseConsumerBackgroundService<Guid, UserMessageDto>
{
    public UserConsumerJob(
        IConsumer<Guid, UserMessageDto> consumer,
        ILogger<UserConsumerJob> logger) : base("user-events", consumer, logger)
    {
    }


    protected override async Task ProcessMessageAsync(ConsumeResult<Guid, UserMessageDto> consumeResult,
        CancellationToken stoppingToken)
    {
        var messageValue = consumeResult.Message.Value;

        Logger.LogInformation($"User: {messageValue}");
    }
}
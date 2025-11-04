using Confluent.Kafka;
using Yandex.Cinema.Events.Messages;

namespace Yandex.Cinema.Events;

public class PaymentConsumerJob : BaseConsumerBackgroundService<Guid, PaymentMessageDto>
{
    public PaymentConsumerJob(
        IConsumer<Guid, PaymentMessageDto> consumer,
        ILogger<PaymentConsumerJob> logger) : base("payment-events", consumer, logger)
    {
    }


    protected override async Task ProcessMessageAsync(ConsumeResult<Guid, PaymentMessageDto> consumeResult,
        CancellationToken stoppingToken)
    {
        var messageValue = consumeResult.Message.Value;

        Logger.LogInformation($"Payment: {messageValue}");
    }
}
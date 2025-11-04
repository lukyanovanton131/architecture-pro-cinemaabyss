using Confluent.Kafka;

namespace Yandex.Cinema.Events;

public abstract class BaseConsumerBackgroundService<TMessageKey, TMessageValue> : BackgroundService
{
	private readonly string _topicName;
	private readonly IConsumer<TMessageKey, TMessageValue> _consumer;
	protected readonly ILogger<BaseConsumerBackgroundService<TMessageKey, TMessageValue>> Logger;

	public BaseConsumerBackgroundService(
		string topicName,
		IConsumer<TMessageKey, TMessageValue> consumer,
		ILogger<BaseConsumerBackgroundService<TMessageKey, TMessageValue>> logger)
	{
		_topicName = topicName;
		_consumer = consumer;
		Logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await Task.Run(async () =>
		{
			try
			{
				_consumer.Subscribe(_topicName);
				while (!stoppingToken.IsCancellationRequested)
				{
					ConsumeResult<TMessageKey, TMessageValue> consumeResult = null;
					try
					{
						consumeResult = _consumer.Consume(stoppingToken);

						await ProcessMessageAsync(consumeResult, stoppingToken);

						_consumer.Commit(consumeResult);
					}
					catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
					{
						Logger.LogInformation("Consumption cancelled");
						break;
					}
					catch (Exception ex) when (consumeResult != null)
					{
						Logger.LogError(ex, "Message processing failed. Key: {Key}", consumeResult.Message.Key);

						// Коммитим оффсет, чтобы продолжить обработку следующих сообщений
						_consumer.Commit(consumeResult);
					}
					catch (Exception ex) when (consumeResult != null)
					{
						Logger.LogError(ex, "Unexpected error processing message");
					}
				}

				_consumer.Unsubscribe();
			}
			catch (Exception exc)
			{
				Logger.LogError(exc, "Unexpected exception while receiving messages from topic {topicName}",
					_topicName);
			}
			finally
			{
				_consumer.Close();
				_consumer.Dispose();
				Logger.LogDebug("Consume {name} message is stopping.", typeof(TMessageValue).Name);
			}
		}, stoppingToken);
	}

	protected abstract Task ProcessMessageAsync(ConsumeResult<TMessageKey, TMessageValue> consumeResult,
		CancellationToken stoppingToken);
	
}
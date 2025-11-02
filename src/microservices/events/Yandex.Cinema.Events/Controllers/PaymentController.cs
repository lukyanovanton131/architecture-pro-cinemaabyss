using Confluent.Kafka;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Yandex.Cinema.Events.Messages;
using Yandex.Cinema.Events.Requests;
using Yandex.Cinema.Events.Response;

namespace Yandex.Cinema.Events.Controllers;

[ApiController]
[Route("api/events/payment")]
public class PaymentController(IProducer<Guid, PaymentMessageDto> producer) : ControllerBase
{
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] PaymentRequest request, CancellationToken cancellationToken)
    {
        var message = new Message<Guid, PaymentMessageDto>() {Key = Guid.NewGuid(), Value = request.Adapt<PaymentMessageDto>()};
        var deliveryResult = await producer.ProduceAsync("payment-events", message, cancellationToken);
        var response = new Response.Response()
        {
            Status = "success",
            Partition = deliveryResult.Partition,
            Offset = deliveryResult.Offset.Value,
            Event = new EventDto()
            {
                Id = message.Key.ToString(),
                Type = "payment",
                Timestamp = DateTime.Now,
                Payload = request
            }
        };
        return Created("", response);
    }
}
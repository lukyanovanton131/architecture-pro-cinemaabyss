using Confluent.Kafka;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Yandex.Cinema.Events.Messages;
using Yandex.Cinema.Events.Requests;

namespace Yandex.Cinema.Events.Controllers;

[ApiController]
[Route("api/events/payment")]
public class PaymentController(IProducer<Guid, PaymentMessageDto> producer) : ControllerBase
{
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] PaymentRequest request, CancellationToken cancellationToken)
    {
        var message = new Message<Guid, PaymentMessageDto>() {Key = Guid.NewGuid(), Value = request.Adapt<PaymentMessageDto>()};
        await producer.ProduceAsync("payment-events", message, cancellationToken);
        return Ok();
    }
}
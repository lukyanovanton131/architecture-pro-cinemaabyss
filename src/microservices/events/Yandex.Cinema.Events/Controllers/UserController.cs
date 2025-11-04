using Confluent.Kafka;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Yandex.Cinema.Events.Messages;
using Yandex.Cinema.Events.Requests;
using Yandex.Cinema.Events.Response;

namespace Yandex.Cinema.Events.Controllers;

[ApiController]
[Route("api/events/user")]
public class UserController(IProducer<Guid, UserMessageDto> producer) : ControllerBase
{
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] UserRequest request, CancellationToken cancellationToken)
    {
        var message = new Message<Guid, UserMessageDto>() {Key = Guid.NewGuid(), Value = request.Adapt<UserMessageDto>()};
        var deliveryResult = await producer.ProduceAsync("user-events", message, cancellationToken);
        var response = new Response.Response()
        {
            Status = "success",
            Partition = deliveryResult.Partition,
            Offset = deliveryResult.Offset.Value,
            Event = new EventDto()
            {
                Id = message.Key.ToString(),
                Type = "user",
                Timestamp = DateTime.Now,
                Payload = request
            }
        };
        return Created("", response);
    }
}
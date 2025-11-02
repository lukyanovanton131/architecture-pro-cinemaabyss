using Confluent.Kafka;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Yandex.Cinema.Events.Messages;
using Yandex.Cinema.Events.Requests;

namespace Yandex.Cinema.Events.Controllers;

[ApiController]
[Route("api/events/user")]
public class UserController(IProducer<Guid, UserMessageDto> producer) : ControllerBase
{
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] UserRequest request, CancellationToken cancellationToken)
    {
        var message = new Message<Guid, UserMessageDto>() {Key = Guid.NewGuid(), Value = request.Adapt<UserMessageDto>()};
        await producer.ProduceAsync("user-events", message, cancellationToken);
        return Ok();
    }
}
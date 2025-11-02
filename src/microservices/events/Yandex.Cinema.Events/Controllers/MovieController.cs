using Confluent.Kafka;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Yandex.Cinema.Events.Messages;
using Yandex.Cinema.Events.Requests;

namespace Yandex.Cinema.Events.Controllers;

[ApiController]
[Route("api/events/movie")]
public class MovieController(IProducer<Guid, MovieMessageDto> producer) : ControllerBase
{
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] MovieRequest request, CancellationToken cancellationToken)
    {
        var message = new Message<Guid, MovieMessageDto>() {Key = Guid.NewGuid(), Value = request.Adapt<MovieMessageDto>()};
        await producer.ProduceAsync("movie-events", message, cancellationToken);
        return Ok();
    }
}
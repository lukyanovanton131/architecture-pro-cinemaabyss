using Confluent.Kafka;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Yandex.Cinema.Events.Messages;
using Yandex.Cinema.Events.Requests;
using Yandex.Cinema.Events.Response;

namespace Yandex.Cinema.Events.Controllers;

[ApiController]
[Route("api/events/movie")]
public class MovieController(IProducer<Guid, MovieMessageDto> producer) : ControllerBase
{
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] MovieRequest request, CancellationToken cancellationToken)
    {
        var message = new Message<Guid, MovieMessageDto>()
            {Key = Guid.NewGuid(), Value = request.Adapt<MovieMessageDto>()};
        var deliveryResult = await producer.ProduceAsync("movie-events", message, cancellationToken);
        var response = new Response.Response()
        {
            Status = "success",
            Partition = deliveryResult.Partition,
            Offset = deliveryResult.Offset.Value,
            Event = new EventDto()
            {
                Id = message.Key.ToString(),
                Type = "movie",
                Timestamp = DateTime.Now,
                Payload = request
            }
        };
        return Created("", response);
    }
}
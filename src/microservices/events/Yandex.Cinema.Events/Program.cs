using Mapster;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Yandex.Cinema.Events;
using Yandex.Cinema.Events.Messages;


TypeAdapterConfig.GlobalSettings.Apply(new MapsterConfig());
var builder = WebApplication.CreateBuilder(args);
    
builder.Services.AddKafkaConsumer<Guid, MovieMessageDto>("movies");
builder.Services.AddKafkaConsumer<Guid, PaymentMessageDto>("payments");
builder.Services.AddKafkaConsumer<Guid, UserMessageDto>("users");
        
builder.Services.AddKafkaProducer<Guid, MovieMessageDto>();
builder.Services.AddKafkaProducer<Guid, PaymentMessageDto>();
builder.Services.AddKafkaProducer<Guid, UserMessageDto>();
        
builder.Services.AddHostedService<UserConsumerJob>();
builder.Services.AddHostedService<PaymentConsumerJob>();
builder.Services.AddHostedService<MovieConsumerJob>();

builder.Services.AddControllers();    
builder.Services.AddHealthChecks();
builder.Services.AddOptions<KafkaOptions>().BindConfiguration("Kafka");

var app = builder.Build();

app.MapHealthChecks("/api/events/health", new HealthCheckOptions 
{ 
    ResponseWriter =async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = true,
        });
        await context.Response.WriteAsync(result);
    } 
});
app.MapControllers();
await app.RunAsync();
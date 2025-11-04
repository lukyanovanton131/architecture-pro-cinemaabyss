using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Yandex.ApiGateway;
using Yarp.ReverseProxy.LoadBalancing;

var builder = WebApplication.CreateBuilder(args);


//app.MapGet("/", () => "Hello World!");



//MOVIES_MIGRATION_PERCENT

var monolithUrls = builder.Configuration.GetSection("MONOLITH_URL").Get<string>();
var moviesServiceUrls = builder.Configuration.GetSection("MOVIES_SERVICE_URL").Get<string>();
var migrationPercent = builder.Configuration.GetSection("MOVIES_MIGRATION_PERCENT").Get<int>();

//builder.Services.AddOptions<MigrationConfig>(OptionsBuilderExtensions=>).BindConfiguration("MOVIES_MIGRATION_PERCENT");
builder.Services.Configure<MigrationConfig>(opt =>
{
    opt.MoviesMigrationPercentage = migrationPercent;
    opt.MonolithServiceUrl = monolithUrls;
    opt.MoviesServiceUrl = moviesServiceUrls;
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddSingleton<ILoadBalancingPolicy, PercentageLoadBalancingPolicy>();

builder.Services.AddHealthChecks();

var app = builder.Build();
app.MapHealthChecks("/health", new HealthCheckOptions 
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
app.MapReverseProxy();
app.Run();
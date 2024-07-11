using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMqWebApi;
using RabbitMqWebApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection("RabbitMqConfig"));
// Add services to the container.
builder.Services.AddSingleton(sp =>
{
    var rabbitMqConfig = sp.GetRequiredService<IOptions<RabbitMqConfig>>().Value;
    var factory = new ConnectionFactory() { HostName = rabbitMqConfig.HostName, UserName = rabbitMqConfig.UserName, Password = rabbitMqConfig.Password };
    return factory.CreateConnection();
});
builder.Services.AddSingleton<RabbitMqConsumer>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<RabbitMqConsumer>());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.Run();
using RabbitMQ.Client;
using RabbitMqWebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton(sp =>
{
    var factory = new ConnectionFactory() { HostName = "localhost", UserName = "user", Password = "password" };
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
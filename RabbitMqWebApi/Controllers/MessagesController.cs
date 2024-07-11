using Microsoft.AspNetCore.Mvc;

namespace RabbitMqWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MessagesController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> GetLatestMessage()
    {
        return RabbitMqConsumer.LatestMessage!;
    }
}
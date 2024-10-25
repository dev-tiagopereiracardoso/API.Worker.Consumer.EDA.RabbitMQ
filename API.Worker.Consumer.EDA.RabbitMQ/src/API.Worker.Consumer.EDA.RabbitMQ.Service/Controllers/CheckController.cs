using Microsoft.AspNetCore.Mvc;

namespace API.Worker.Consumer.EDA.RabbitMQ.Service.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("check")]
    public class CheckController : ControllerBase
    {
        public CheckController()
        {
        }

        [HttpGet("system")]
        public bool System()
        {
            return true;
        }
    }
}
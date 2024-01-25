using Microsoft.AspNetCore.Mvc;

namespace UdemySignalR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignalRController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetNames()
        {
            return Ok("Success");
        }
    }
}

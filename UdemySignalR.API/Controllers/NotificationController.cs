using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using UdemySignalR.API.Hubs;

namespace UdemySignalR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<SignalRHub> _hubContext;

        public NotificationController(IHubContext<SignalRHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet("{teamCount}")]
        public async Task<IActionResult> SetTeamCount(int teamCount)
        {
            SignalRHub.TeamCount = teamCount;
            await _hubContext.Clients.All.SendAsync("NotifyTeamCount", $"Arkadaşlar Takım {teamCount} kişiden oluşacak");

            return Ok();
        }
    }
}

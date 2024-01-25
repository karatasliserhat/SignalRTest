using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using TestSignalR.Hubs;
using TestSignalR.Models;

namespace TestSignalR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<SignalRTestHub> _hubContext;
        public HomeController(ILogger<HomeController> logger, IHubContext<SignalRTestHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(int team)
        {
            await _hubContext.Clients.All.SendAsync("SetCountTeam", $"Takım Sayınız {team} kadar olması gerekiyor");
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

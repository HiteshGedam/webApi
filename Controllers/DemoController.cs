using Microsoft.AspNetCore.Mvc;
using WebApiApp.MyLogging;

namespace WebApiApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : Controller
    {
        private readonly ILogger<DemoController> _Logger;

        public DemoController(ILogger<DemoController> logger)
        {
            _Logger  = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _Logger.LogTrace("Index Method Logged");
            _Logger.LogDebug("Index Method Logged");
            _Logger.LogInformation("Index Method Logged");
            _Logger.LogWarning("Index Method Logged");
            _Logger.LogError("Index Method Logged");
            _Logger.LogCritical("Critical Method Logged");
            return Ok();
        }
    }
}

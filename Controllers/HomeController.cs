using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineAuction.Services;

namespace OnlineAuction.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MandarinService _mandarinService;

        public HomeController(MandarinService mandarinService) 
        {
            _mandarinService = mandarinService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet("api/mandarins")]
        public JsonResult GetMandarins()
        {
            var mandarins = _mandarinService.GetMandarins();
            var mandarinsJson = JsonConvert.SerializeObject(mandarins);
            return Json(mandarins);
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace myprojajax.Controllers
{
    public class SignalRController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

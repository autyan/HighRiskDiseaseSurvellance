using Microsoft.AspNetCore.Mvc;

namespace aspnetapp.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

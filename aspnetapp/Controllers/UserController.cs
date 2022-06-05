using Microsoft.AspNetCore.Mvc;

namespace aspnetapp.Controllers
{
    public class UserController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace aspnetapp.Controllers
{
    public class RecordController : BaseController
    {
        public IActionResult Index([FromQuery]string userId)
        {
            ViewBag.UserId = userId;
            return View();
        }
    }
}

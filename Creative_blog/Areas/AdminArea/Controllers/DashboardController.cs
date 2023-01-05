using Microsoft.AspNetCore.Mvc;

namespace Creative_blog.Areas.AdminArea.Controllers
{
    public class DashboardController : Controller
    {
        [Area("AdminArea")]
        public IActionResult Index()
        {
            return View();
        }


    }
}

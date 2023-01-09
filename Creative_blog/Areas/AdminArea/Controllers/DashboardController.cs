using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Creative_blog.Areas.AdminArea.Controllers
{
    public class DashboardController : Controller
    {
        [Authorize(Roles ="Admin")]
        [Area("AdminArea")]
        public IActionResult Index()
        {
            return View();
        }


    }
}

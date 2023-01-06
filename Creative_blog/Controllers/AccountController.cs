using Microsoft.AspNetCore.Mvc;

namespace Creative_blog.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }





        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }



    }
}

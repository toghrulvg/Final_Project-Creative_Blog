using Creative_blog.Models;
using Creative_blog.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Creative_blog.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            AppUser user = new AppUser
            {
                Email = registerVM.Email,
                UserName = registerVM.Username
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(registerVM);
            }

            

            return RedirectToAction(nameof(Login));

        }

        

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

           return RedirectToAction("Index", "Home");
        }





        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            AppUser user = await _userManager.FindByEmailAsync(loginViewModel.UsernameOrEmail);

            if(user is null)
            {
                user = await _userManager.FindByNameAsync(loginViewModel.UsernameOrEmail);
            }
            if(user is null)
            {
                ModelState.AddModelError("", "Email or Password is incorrect");
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email or Password is incorrect");
                return View(loginViewModel);
            }


            return RedirectToAction("Index", "Home");
        }



    }
}

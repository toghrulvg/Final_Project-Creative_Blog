using Creative_blog.Models;
using Creative_blog.ViewModels.AccountViewModels;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
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

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            string link = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, token }, Request.Scheme, Request.Host.ToString());


            // create email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("fidannm@code.edu.az"));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Test Email Subject";
            string body = $"<a href='{link}'>Click here to verify your email</a>";
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            //sendmail
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("fidannm@code.edu.az", "W71Rn9h.");
            smtp.Send(email);
            smtp.Disconnect(true);







            return RedirectToAction(nameof(VerifyEmail));
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId is null || token is null) return BadRequest();

            AppUser user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            await _userManager.ConfirmEmailAsync(user, token);

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home");




        }


        public IActionResult VerifyEmail()
        {
            return View();
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

        public IActionResult Verify()
        {
            return View();
        }

    }

}

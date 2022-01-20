using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using UsingIdentity.ViewModels;

namespace UsingIdentity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailService emailService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _roleManager = roleManager;
        }

        public IActionResult Login(string returnUrl = "/")
        {
            var loginVM = new LoginVM()
            {
                ReturnUrl = returnUrl
            };

            return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, login.RememberMe, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View(login);
            }

            return LocalRedirect(login.ReturnUrl);
        }

        public IActionResult Register()
        {
            var regVm = new RegisterVM();
            return View(regVm);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            var user = new IdentityUser()
            {
                Email = register.Email,
                UserName = register.Username,
            };

            var result = await _userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(register);
            }

            var userClaims = new List<Claim>()
            {
                new Claim("Age", register.Age.ToString()),
                new Claim(ClaimTypes.GivenName, register.FirstName),
                new Claim(ClaimTypes.Surname, register.LastName),
                new Claim(ClaimTypes.Role, "Subscriber")
            };

            _ = await _userManager.AddClaimsAsync(user, userClaims);

            _ = await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Legacy"
            });

            _ = await _userManager.AddToRoleAsync(user, "Legacy");

            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var link = Url.Action(nameof(VerifyEmail), "Account", new { userId = user.Id, code = emailToken }, HttpContext.Request.Scheme, HttpContext.Request.Host.ToString());

            var emailContent = $"<a href=\"{link}\">Verify Email</a>";

            await _emailService.SendAsync(user.Email, "Email Verification", emailContent, true);

            return LocalRedirect("/");
        }

        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return BadRequest();
            }

            var verificationResult = await _userManager.ConfirmEmailAsync(user, code);

            if (!verificationResult.Succeeded)
            {
                return BadRequest();
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Redirect("/");
        }
    }
}

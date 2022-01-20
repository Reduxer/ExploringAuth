using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Basic.Controllers
{
    public class AuthController : Controller
    {
        public async Task<IActionResult> Login(string returnUrl = "/")
        {
            var defaultClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,"1"),
                new Claim(ClaimTypes.Name, "Bard")
            };

            var defaultClaimsId = new ClaimsIdentity(defaultClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(new[] { defaultClaimsId });

            await HttpContext.SignInAsync(claimsPrincipal);

            return LocalRedirect(returnUrl);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}

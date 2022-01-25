using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;

namespace WebAppMVC.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            return SignOut("oidc", CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}

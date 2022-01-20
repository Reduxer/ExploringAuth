using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UsingIdentity.Authorization.Requirements;
using UsingIdentity.Core.Entities;

namespace UsingIdentity.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthorizationService _authorizationService;

        public HomeController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Subscriber")]
        public IActionResult SecuredPage()
        {
            return View();
        }

        public async Task<IActionResult> AccessDenied()
        {
            return await Task.FromResult(View());
        }

        public async Task<IActionResult> SecuredResource()
        {
            var post = new Post()
            {
                AuthorId = "123",
                Content = "Test post",
                DatePosted = DateTime.UtcNow
            };

            var authResult = await _authorizationService.AuthorizeAsync(User, post, Operations.Update(21));

            if (!authResult.Succeeded)
            {
                return Forbid();
            }

            return Json(post);
        }
    }
}

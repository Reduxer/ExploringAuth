using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using UsingIdentity.Authorization.Requirements;

namespace UsingIdentity.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly IAuthorizationService _authorizationService;

        public OrganizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [Authorize(Roles = "SystemtAdmin")]
        public async Task<IActionResult> GetProjects(int orgId)
        {

            //this is called imperative authorization, when you rely for dynamic data you cant use the authorize attribute
            //but you can make your custom attribute and middleware

            var authResult = await _authorizationService.AuthorizeAsync(User, null, new OrgSysAdminRequirement() { OrganizationId = orgId });

            if (!authResult.Succeeded)
            {
                return Forbid();
            }

            var projects = new[]
            {
                new
                {

                    Name = "Project 88",
                    DateStarted = DateTime.UtcNow
                },
                new
                {

                    Name = "Avengers Iniatiative",
                    DateStarted = DateTime.UtcNow
                },
            };

            return Json(projects);
        }
    }
}

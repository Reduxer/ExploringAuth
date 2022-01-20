using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace UsingIdentity.Authorization.Requirements
{
    public class OrgSysAdminRequirement : IAuthorizationRequirement
    {
        public int OrganizationId { get; set; }

    }

    public class OrgSysAdminRequirementHandler : AuthorizationHandler<OrgSysAdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OrgSysAdminRequirement requirement)
        {
            //context has the ClaimsPrincipal 
            var user = context.User;

            var isSysAdminForOrg = user.HasClaim(c => c.Type == "Organization" && c.Value == requirement.OrganizationId.ToString());
            
            if(isSysAdminForOrg)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

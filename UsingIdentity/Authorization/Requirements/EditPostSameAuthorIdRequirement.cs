using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UsingIdentity.Core.Entities;


namespace UsingIdentity.Authorization.Requirements
{
    public class EditPostSameAuthorIdRequirement : IAuthorizationRequirement
    {
    }

    public class EditPostSameAuthorIdRequirementHandler : AuthorizationHandler<EditPostSameAuthorIdRequirement, Post>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EditPostSameAuthorIdRequirement requirement, Post resource)
        {
            var user = context.User;
            var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userId?.Value == resource.AuthorId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

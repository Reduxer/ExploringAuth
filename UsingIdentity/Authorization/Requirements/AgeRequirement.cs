using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using UsingIdentity.Core.Entities;

namespace UsingIdentity.Authorization.Requirements
{
    public class AgeRequirement : OperationAuthorizationRequirement 
    {
        public int Age { get; set; }
    }

    public class AgeRequirementHandler : AuthorizationHandler<AgeRequirement, Post>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirement requirement, Post resource)
        {
            var hasAgeOverClaim = context.User.Claims.Any(c => c.Type == "Age" && int.Parse(c.Value) >= requirement.Age);

            if (hasAgeOverClaim || requirement.Name == Operations.Read().Name)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public static class HasAgeOver21RequirementAuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder AddAgeRequirement(this AuthorizationPolicyBuilder builder)
        {
            builder.AddRequirements(new AgeRequirement());
            return builder;
        }
    }

    public static class Operations
    {
        public static AgeRequirement Create(int age = default)
        {
            return new AgeRequirement { Name = nameof(Create), Age = age};
        }

        public static AgeRequirement Update(int age = default)
        {
            return new AgeRequirement { Name = nameof(Update), Age = age };
        }

        public static AgeRequirement Read(int age = default)
        {
            return new AgeRequirement { Name = nameof(Read), Age = age };
        }

        public static AgeRequirement Delete(int age = default)
        {
            return new AgeRequirement { Name = nameof(Delete), Age = age };
        }
    }
}

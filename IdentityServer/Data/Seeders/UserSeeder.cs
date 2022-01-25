using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IdentityServer.Data.Seeders
{
    public static class UserSeeder
    {
        public static void SeedUsers(this IServiceProvider serviceProvider)
        {
            using var serviceScope = serviceProvider.CreateScope();

            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var user = userManager.FindByNameAsync("bob").GetAwaiter().GetResult();

            if(user is null)
            {
                userManager.CreateAsync(new IdentityUser("bob"), "Bob123").GetAwaiter().GetResult();
            }
        }
    }
}

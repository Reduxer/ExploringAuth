using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityServer.ViewModels;

namespace IdentityServer.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext() {}

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    }
}

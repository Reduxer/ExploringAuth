using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using UsingIdentity.Authorization.CLaimsTransformer;
using UsingIdentity.Authorization.Requirements;
using UsingIdentity.Data;

namespace UsingIdentity
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(setup =>
            {
                setup.UseSqlite("Data Source = UsingIdentity.db");
            });

            services.AddIdentity<IdentityUser, IdentityRole>(setup =>
            {
                setup.SignIn.RequireConfirmedEmail = true;
                setup.Password.RequireNonAlphanumeric = false;
                setup.Password.RequireDigit = false;
                setup.Password.RequireUppercase = false;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthorization(setup =>
            {
                setup.AddPolicy("AgePolicy", policyBuilder =>
                {
                    policyBuilder.AddAgeRequirement();
                });

                setup.AddPolicy("EditPostPolicy", policyBuilder =>
                {
                    policyBuilder.AddRequirements(new EditPostSameAuthorIdRequirement());
                });
            });

            services.AddScoped<IAuthorizationHandler, AgeRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, EditPostSameAuthorIdRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, OrgSysAdminRequirementHandler>();
            services.AddScoped<IClaimsTransformation, ClaimsTransformation>();

            services.ConfigureApplicationCookie(setup =>
            {
                setup.Cookie.Name = "App.Cookie";
                setup.LoginPath = "/account/login";
                setup.LogoutPath = "/account/logout";
                setup.AccessDeniedPath = "/home/accessdenied";
            });

            services.AddMailKit(setup =>
            {
                var opts = Configuration.GetSection("Email").Get<MailKitOptions>();
                setup.UseMailKit(opts);
            });

            services.AddControllersWithViews();

            services.AddRazorPages()
                .AddRazorPagesOptions(setup =>
                {
                    setup.Conventions.AuthorizePage("/razor/Secured");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using IdentityServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using System.Linq;

namespace IdentityServer
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
                setup.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            services.AddIdentity<IdentityUser, IdentityRole>(setup =>
            {
                setup.SignIn.RequireConfirmedEmail = false;
                setup.Password.RequireUppercase = false;
                setup.Password.RequireNonAlphanumeric = false;

            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            
            var assembly = typeof(Startup).Assembly.GetName().Name;

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<IdentityUser>()
                .AddConfigurationStore(setup => 
                {
                    setup.ConfigureDbContext = b => b.UseSqlServer(Configuration.GetConnectionString("Default"), sqlSetup =>
                    {
                        sqlSetup.MigrationsAssembly(assembly);
                    });
                })
                .AddOperationalStore(setup => 
                {
                    setup.ConfigureDbContext = b => b.UseSqlServer(Configuration.GetConnectionString("Default"), sqlSetup =>
                    {
                        sqlSetup.MigrationsAssembly(assembly);
                    });
                });

            services.ConfigureApplicationCookie(setup =>
            {
                setup.Cookie.Name = "IdentityServer.Cookie";
                setup.LoginPath = "/Auth/Login";
                setup.LogoutPath = "/Auth/Logout";
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var sp = scope.ServiceProvider;

            var configDbContext = sp.GetRequiredService<ConfigurationDbContext>();

            if (!configDbContext.Clients.Any())
            {
                foreach (var client in Config.Clients)
                {
                    configDbContext.Clients.Add(client.ToEntity());
                }
            }

            if (!configDbContext.ApiResources.Any())
            {
                foreach(var ar in Config.ApiResources)
                {
                    configDbContext.ApiResources.Add(ar.ToEntity());
                }
            }

            if (!configDbContext.IdentityResources.Any())
            {
                foreach(var ir in Config.IdentityResources)
                {
                    configDbContext.IdentityResources.Add(ir.ToEntity());
                }
            }

            if(!configDbContext.ApiScopes.Any())
            {
                foreach(var apiScope in Config.ApiScopes)
                {
                    configDbContext.ApiScopes.Add(apiScope.ToEntity());
                }
            }
            
            configDbContext.SaveChanges();
        }
    }
}

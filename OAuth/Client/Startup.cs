using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Text;
using System.Text.Encodings;
using System.Text.Json;
using System.Security.Claims;

namespace Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddAuthentication(setup =>
            {
                setup.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                setup.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                setup.DefaultChallengeScheme = "OurServer";
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, setup => 
                {
                    setup.LogoutPath = "/home/logout";
                })
                .AddOAuth("OurServer", setup =>
                {
                    setup.ClientId = "clientid";
                    setup.ClientSecret = "clientsecret";
                    setup.CallbackPath = "/oauth/callback";
                    setup.AuthorizationEndpoint = "https://localhost:44374/oauth/authorize";
                    setup.TokenEndpoint = "https://localhost:44374/oauth/token";
                    
                    setup.SaveTokens = true;

                    setup.Events = new OAuthEvents()
                    {
                        OnCreatingTicket = (context) =>
                        {
                            var accessToken = context.AccessToken;
                            var payload = accessToken.Split('.')[1];

                            payload = payload.Replace('_', '/').Replace('-', '+');
                            switch (payload.Length % 4)
                            {
                                case 2: payload += "=="; break;
                                case 3: payload += "="; break;
                            }

                            var payloadBytes = Convert.FromBase64String(payload);
                            var payloadString = Encoding.UTF8.GetString(payloadBytes);

                            var claimDict = JsonSerializer.Deserialize<Dictionary<string, object>>(payloadString);

                            foreach(var c in claimDict)
                            {
                                context.Identity.AddClaim(new Claim(c.Key, c.Value.ToString()));
                            }
                            
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SampleApi.Providers.AuthHandlers.Scheme;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Security.Claims;
using SampleApi.Providers.AuthHandlers.Constants;

namespace SampleApi.Providers.AuthHandlers
{
    public class OAuthServerAuthHandler : AuthenticationHandler<OAuthServerAuthSchemeOptions>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OAuthServerAuthHandler(
            IOptionsMonitor<OAuthServerAuthSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IHttpClientFactory httpClientFactory) : base(options, logger, encoder, clock)
        {
            _httpClientFactory = httpClientFactory;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorization))
            {
                return AuthenticateResult.Fail("Header Not Found.");
            }

            try
            {
                var httpCLient = _httpClientFactory.CreateClient();
                httpCLient.DefaultRequestHeaders.Add(HeaderNames.Authorization, authorization.ToString());
                var response  = await httpCLient.GetAsync(Options.TokenValidationEndpoint);

                response.EnsureSuccessStatusCode();

                var token = authorization.ToString().Split(' ').Last();
                var payload = token.Split('.')[1];

                payload = payload.Replace('_', '/').Replace('-', '+');

                switch (payload.Length % 4)
                {
                    case 2: payload += "=="; break;
                    case 3: payload += "="; break;
                }

                var payloadBytes = Convert.FromBase64String(payload);
                var payloadString = Encoding.UTF8.GetString(payloadBytes);

                var claimDict = JsonSerializer.Deserialize<Dictionary<string, object>>(payloadString);
                var claims = claimDict.Select(c => new Claim(c.Key, c.Value.ToString()));

                ClaimsIdentity identity  = new ClaimsIdentity(claims, nameof(OAuthServerAuthHandler));
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                var ticket = new AuthenticationTicket(principal, this.Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "TokenValidationException");
                return AuthenticateResult.Fail("TokenValidationException");
            }
        }
    }


    public static class OAuthServerExtensions
    {
        public static AuthenticationBuilder AddOAuthServer(this AuthenticationBuilder builder, Action<OAuthServerAuthSchemeOptions> configureOptions)
        {
            builder.AddScheme<OAuthServerAuthSchemeOptions, OAuthServerAuthHandler>(OAuthServerAuthenticationDefaults.AuthenticationScheme, configureOptions);
            return builder;
        }

        public static AuthenticationBuilder AddOAuthServer(this AuthenticationBuilder builder, string authenticationScheme,
            Action<OAuthServerAuthSchemeOptions> configureOptions)
        {
            builder.AddScheme<OAuthServerAuthSchemeOptions, OAuthServerAuthHandler>(authenticationScheme, configureOptions);
            return builder;
        }
    }
}

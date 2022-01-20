using Microsoft.AspNetCore.Authentication;

namespace SampleApi.Providers.AuthHandlers.Scheme
{
    public class OAuthServerAuthSchemeOptions : AuthenticationSchemeOptions
    {
        public string TokenValidationEndpoint { get; set; }
    }
}

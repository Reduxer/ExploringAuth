using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace UsingIdentity.Authorization.CLaimsTransformer
{
    public class ClaimsTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var hasSecretClaim = principal.Claims.Any(c => c.Type == "Secret");
            
            if(!hasSecretClaim)
            {
                var claimsIdentity = principal.Identity as ClaimsIdentity;
                claimsIdentity.AddClaim(new Claim("Secret", "this is a secret text"));
            }

            return Task.FromResult(principal);
        }
    }
}

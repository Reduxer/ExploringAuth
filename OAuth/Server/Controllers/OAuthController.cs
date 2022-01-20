using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Server.ViewModels;
using System;
using Server.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace Server.Controllers
{
    public class OAuthController : Controller
    {
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public OAuthController(IJwtTokenProvider jwtTokenProvider)
        {
            _jwtTokenProvider = jwtTokenProvider;
        }

        [HttpGet]
        public IActionResult Authorize(AuthorizationRequestVM authorizationRequest)
        {
            var vm = AuthorizationRequestLoginVM.CreateInstance(authorizationRequest);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Authorize(AuthorizationRequestLoginVM authorizationRequestLogin)
        {
            if (!ModelState.IsValid)
            {
                return View(authorizationRequestLogin);
            }

            string code = "213nlln1l2n3l1kn23";

            var query = new QueryBuilder
            {
                { "code", code },
                { "state", authorizationRequestLogin.State }
            };

            var uri = new UriBuilder(authorizationRequestLogin.RedirectURI)
            {
                Query = query.ToString()
            };

            return Redirect(uri.Uri.ToString());
        }

        [HttpPost]
        public IActionResult Token(TokenRequest tokenRequest)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, "12o12")
            };

            var expires = tokenRequest.GrantType == "refresh_token" ? DateTime.UtcNow.AddMinutes(5) : DateTime.UtcNow.AddMilliseconds(1);

            var token = _jwtTokenProvider.CreateTokenAsync(claims, expires);

            var tokenResponse = new TokenResponse()
            {
                AccessToken = token,
                TokenType = "Bearer",
                RawClaim = "oauth",
                RefreshToken = "AAAAABBBBBBCCCCC"
            };

            return Json(tokenResponse);
        }
    }
}

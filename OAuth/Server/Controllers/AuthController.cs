using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;
using System;
using Server.Services;
using Microsoft.AspNetCore.Authorization;

namespace Server.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public AuthController(IConfiguration configuration, IJwtTokenProvider jwtTokenProvider)
        {
            _configuration = configuration;
            _jwtTokenProvider = jwtTokenProvider;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, "12o12")
            };

            var token = _jwtTokenProvider.CreateTokenAsync(claims, DateTime.UtcNow.AddDays(1));

            return Content(token);
        }

        [Authorize]
        public IActionResult Validate()
        {
            return Ok();
        }
    }
}

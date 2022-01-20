using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Server.Services
{
    public class JwtTokenProvider : IJwtTokenProvider
    {
        private readonly IConfiguration _configuration;

        public JwtTokenProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateTokenAsync(List<Claim> claims, DateTime? expires = null)
        {
            var secret = _configuration["JWT:Key"];
            var secretInBytes = Encoding.UTF8.GetBytes(secret);

            var securityKey = new SymmetricSecurityKey(secretInBytes);
            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jst = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                signingCredentials: signingCred
            );

            var jstHandler = new JwtSecurityTokenHandler();
            var token = jstHandler.WriteToken(jst);
            return token;
        }
    }
}

using System.Collections.Generic;
using System.Security.Claims;
using System;

namespace Server.Services
{
    public interface IJwtTokenProvider
    {
        string CreateTokenAsync(List<Claim> claims, DateTime? expires = null);
    }
}
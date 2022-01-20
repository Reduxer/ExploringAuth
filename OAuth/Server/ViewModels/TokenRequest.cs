using Microsoft.AspNetCore.Mvc;

namespace Server.ViewModels
{
    public class TokenRequest
    {
        [FromQuery(Name = "grant_type")]
        public string GrantType { get; set; }

        [FromQuery]
        public string Code { get; set; }

        [FromQuery(Name = "redirect_uri")]
        public string RedirectURI { get; set; }

        [FromQuery(Name = "client_id")]
        public string CLientId { get; set; }

        [FromQuery(Name = "refresh_token")]
        public string RefreshToken { get; set; }
    }
}

﻿using Microsoft.AspNetCore.Mvc;

namespace Server.ViewModels
{
    public class AuthorizationRequestVM
    {
        [FromQuery(Name = "response_type")]
        public string ResponseType { get; set; }

        [FromQuery(Name = "client_id")]
        public string ClientId { get; set; }

        [FromQuery(Name = "redirect_uri")]
        public string RedirectURI { get; set; }

        [FromQuery(Name = "state")]
        public string State { get; set; }

        [FromQuery(Name = "scope")]
        public string Scope { get; set; }
    }
}

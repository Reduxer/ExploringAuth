using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class AuthorizationRequestLoginVM : AuthorizationRequestVM
    {
        public static AuthorizationRequestLoginVM CreateInstance(AuthorizationRequestVM authorizationRequest)
        {
            return new AuthorizationRequestLoginVM()
            {
                ClientId = authorizationRequest.ClientId,
                RedirectURI = authorizationRequest.RedirectURI,
                ResponseType = authorizationRequest.ResponseType,
                State = authorizationRequest.State,
                Scope = authorizationRequest.Scope
            };
        }

        [Required]
        public string Username { get; set; }
    }
}

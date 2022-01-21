using System.ComponentModel.DataAnnotations;

namespace IdentityServer.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string  Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}

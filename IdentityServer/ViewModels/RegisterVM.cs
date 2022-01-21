using System.ComponentModel.DataAnnotations;

namespace IdentityServer.ViewModels
{
    public class RegisterVM
    {
        [DataType(DataType.Text)]
        [Required]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Text)]
        [Required]
        public string Fullname { get; set; }

        [DataType(DataType.Text)]
        public string ReturnUrl { get; set; }
    }
}

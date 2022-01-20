using System.ComponentModel.DataAnnotations;

namespace UsingIdentity.ViewModels
{
    public class UserCredentialVM
    {
        [Required]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

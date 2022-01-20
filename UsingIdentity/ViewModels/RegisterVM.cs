using System.ComponentModel.DataAnnotations;

namespace UsingIdentity.ViewModels
{
    public class RegisterVM : UserCredentialVM
    {
        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Range(8, 120, ErrorMessage = "You must be at least 8 years old to register for an account")]
        public int Age { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
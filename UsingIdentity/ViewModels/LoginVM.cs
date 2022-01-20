namespace UsingIdentity.ViewModels
{
    public class LoginVM : UserCredentialVM
    {
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}

namespace PowerLifting.Domain.Models.UserData.Auth
{
    public class RegistrationModel
    {
        public string Login { get; set; }

        public string? OldPassword { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }
    }
}

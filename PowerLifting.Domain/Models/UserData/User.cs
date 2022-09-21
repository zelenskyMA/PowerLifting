using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.UserData
{
    public class UserModel : Entity
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public bool Blocked { get; set; }
    }
}

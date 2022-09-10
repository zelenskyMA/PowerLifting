using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.UserData
{
    public class User : Entity
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }
    }
}

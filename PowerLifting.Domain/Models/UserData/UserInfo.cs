using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.UserData
{
    public class UserInfo : Entity
    {
        public int UserId { get; set; }

        public string? FirstName { get; set; }

        public string? Surname { get; set; }

        public string? Patronimic { get; set; }

        public int? Weight { get; set; } = 0;

        public int? Height { get; set; }

        public int? Age { get; set; }

        public int? CoachId { get; set; }
    }
}

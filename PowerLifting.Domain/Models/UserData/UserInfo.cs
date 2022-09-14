using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.UserData
{
    public class UserInfo
    {
        public string? FirstName { get; set; }

        public string? Surname { get; set; }

        public string? Patronimic { get; set; }

        public int? Weight { get; set; } = 0;

        public int? Height { get; set; }

        public int? Age { get; set; }

        public int? CoachId { get; set; }

        /// <summary>
        /// Formatted full name, ex: Иванов И.И.
        /// </summary>
        public string? LegalName { get; set; }
    }
}

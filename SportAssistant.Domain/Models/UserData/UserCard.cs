using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Domain.Models.UserData
{
    public class UserCard
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Login { get; set; }

        public UserInfo BaseInfo { get; set; }

        public List<UserAchivement> Achivements { get; set; }

        public TrainingGroup GroupInfo { get; set; }

        public UserBlockHistory? BlockReason { get; set; }

        public string? CoachLegalName { get; set; }
    }
}

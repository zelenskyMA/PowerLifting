using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.Models.Coaching
{
    public class GroupUser : Entity
    {
        public string FullName { get; set; }

        public int ActivePlansCount { get; set; }
    }
}

using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.Coaching
{
    public class GroupUser : Entity
    {
        public string FullName { get; set; }

        public int ActivePlansCount { get; set; }
    }
}

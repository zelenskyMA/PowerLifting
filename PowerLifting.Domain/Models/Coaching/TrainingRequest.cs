using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.Coaching
{
    public class TrainingRequest : Entity
    {
        public int UserId { get; set; }

        public int CoachId { get; set; }

        public string? CoachName { get; set; }

        public DateTime CreationDate { get; set; }
    }
}

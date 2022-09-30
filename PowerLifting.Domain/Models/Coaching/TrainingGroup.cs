using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.Coaching
{
    public class TrainingGroup : Entity
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public int CoachId { get; set; }
    }
}

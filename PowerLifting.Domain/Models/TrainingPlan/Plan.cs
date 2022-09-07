using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.TrainingPlan
{
    public class Plan : Entity
    {
        public int UserId { get; set; }

        public DateTime StartDate { get; set; }

        public string? Comments { get; set; }

        public List<PlanDay>? TrainingDays { get; set; } = new List<PlanDay>();
    }
}

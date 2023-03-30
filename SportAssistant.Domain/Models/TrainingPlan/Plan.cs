using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.Models.TrainingPlan
{
    public class Plan : Entity
    {
        public int UserId { get; set; }

        public DateTime StartDate { get; set; }

        public bool IsMyPlan { get; set; }

        public List<PlanDay> TrainingDays { get; set; } = new List<PlanDay>();

        public DateTime FinishDate => StartDate.Date.AddDays(6);

        public CountersPlan Counters { get; set; } = new CountersPlan();
    }
}

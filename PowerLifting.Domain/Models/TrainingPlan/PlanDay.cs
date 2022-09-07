using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.TrainingPlan
{
    public class PlanDay : Entity
    {
        public DateTime ActivityDate { get; set; }

        public int LiftCounterSum { get; set; }

        public int WeightLoadSum { get; set; }

        public int IntensitySum { get; set; }

        public List<PlanExercise>? Exercises { get; set; } = new List<PlanExercise>();
    }
}

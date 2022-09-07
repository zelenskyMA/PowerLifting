using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.TrainingPlan
{
    public class PlanExercise : Entity
    {
        public int PlanDayId { get; set; }

        public int Order { get; set; }

        public int LiftCounter { get; set; }

        public int WeightLoad { get; set; }

        public int Intensity { get; set; }

        public Exercise? Exercise { get; set; } = new Exercise();

        public List<PlanExerciseSettings>? Settings { get; set; } = new List<PlanExerciseSettings>();
    }
}

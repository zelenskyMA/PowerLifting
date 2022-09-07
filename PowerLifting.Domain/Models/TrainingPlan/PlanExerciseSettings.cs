using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.TrainingPlan
{
    public class PlanExerciseSettings : Entity
    {
        public int PlanExerciseId { get; set; }

        public int Weight { get; set; }

        public int Iterations { get; set; }

        public int ExercisePart1 { get; set; }

        public int ExercisePart2 { get; set; }

        public int ExercisePart3 { get; set; }

        public string? Comments { get; set; }

        public Percentage? Percentage { get; set; }
    }
}

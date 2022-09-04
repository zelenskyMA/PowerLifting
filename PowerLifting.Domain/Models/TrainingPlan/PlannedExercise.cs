using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.TrainingPlan
{
    public class PlannedExercise : Entity
    {
        public int ExerciseId { get; set; }

        public string ExerciseName { get; set; }

        public int Order { get; set; }

        public int LiftCounter { get; set; }

        public int WeightLoad { get; set; }

        public int Intensity { get; set; }

        public List<ExercisePercentage> ExerciseData { get; set; }
    }
}

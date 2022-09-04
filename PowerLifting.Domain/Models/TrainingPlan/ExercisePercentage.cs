using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.TrainingPlan
{
    public class ExercisePercentage : Entity
    {
        public Percentage Percentage { get; set; }

        public ExerciseSettings Values { get; set; }

    }
}

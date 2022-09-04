using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.TrainingPlan
{
    public class Exercise : NamedEntity
    {
        public int ExerciseTypeId { get; set; }

        public string ExerciseTypeName { get; set; }

        public int ExerciseSubTypeId { get; set; }

        public string ExerciseSubTypeName { get; set; }
    }
}

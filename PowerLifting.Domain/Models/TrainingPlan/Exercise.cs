using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.TrainingPlan
{
    public class Exercise : NamedEntity
    {
        public int UserId { get; set; } = 0;

        public bool Closed { get; set; }

        /// <summary>
        /// Держим в модели, чтобы позволить изменять упражнения в тренировочном дне.
        /// </summary>
        public int PlannedExerciseId { get; set; } = 0;

        public int ExerciseTypeId { get; set; }

        public string? ExerciseTypeName { get; set; }

        public int ExerciseSubTypeId { get; set; }

        public string? ExerciseSubTypeName { get; set; }

        public virtual Exercise Clone()
        {
            return (Exercise)MemberwiseClone();
        }
    }
}

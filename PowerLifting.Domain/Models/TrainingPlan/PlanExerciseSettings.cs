using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.TrainingPlan
{
    public class PlanExerciseSettings : Entity
    {
        public int PlanExerciseId { get; set; }

        /// <summary>
        /// Вес, который будет подниматься при выполнении упражнения
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// Количество подходов к штанге
        /// </summary>
        public int Iterations { get; set; }

        /// <summary>
        /// Количество повторов первой части упражнения
        /// </summary>
        public int ExercisePart1 { get; set; }

        /// <summary>
        /// Количество повторов второй части упражнения
        /// </summary>
        public int ExercisePart2 { get; set; }

        /// <summary>
        /// Количество повторов третьей части упражнения
        /// </summary>
        public int ExercisePart3 { get; set; }

        public string? Comments { get; set; }

        public Percentage? Percentage { get; set; }

        public Exercise? Exercise { get; set; } = new Exercise();
    }
}

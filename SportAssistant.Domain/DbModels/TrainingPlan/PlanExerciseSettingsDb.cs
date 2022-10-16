using SportAssistant.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.TrainingPlan
{
    [Table("PlanExerciseSettings", Schema = "plan")]
    public class PlanExerciseSettingsDb : EntityDb
    {
        public int PercentageId { get; set; }

        public int PlanExerciseId { get; set; }

        public bool Completed { get; set; }

        public int Weight { get; set; } = 0;

        public int Iterations { get; set; } = 0;

        public int ExercisePart1 { get; set; } = 0;

        public int ExercisePart2 { get; set; } = 0;

        public int ExercisePart3 { get; set; } = 0;
    }
}

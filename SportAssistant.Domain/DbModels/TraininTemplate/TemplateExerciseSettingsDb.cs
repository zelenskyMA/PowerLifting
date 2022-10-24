using SportAssistant.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.TrainingPlan
{
    [Table("TemplateExerciseSettings", Schema = "plan")]
    public class TemplateExerciseSettingsDb : EntityDb
    {
        public int PercentageId { get; set; }

        public int TemplateExerciseId { get; set; }

        public int WeightPercentage { get; set; }

        public int Iterations { get; set; } = 0;

        public int ExercisePart1 { get; set; } = 0;

        public int ExercisePart2 { get; set; } = 0;

        public int ExercisePart3 { get; set; } = 0;
    }
}

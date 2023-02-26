using SportAssistant.Domain.Models.Common;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Domain.Models.TrainingTemplate
{
    public class TemplateExerciseSettings : Entity
    {
        public int TemplateExerciseId { get; set; }

        public int WeightPercentage { get; set; }

        public int Iterations { get; set; } = 0;

        public int ExercisePart1 { get; set; } = 0;

        public int ExercisePart2 { get; set; } = 0;

        public int ExercisePart3 { get; set; } = 0;

        public Percentage? Percentage { get; set; }
    }
}

using SportAssistant.Domain.Models.Common;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Domain.Models.TraininTemplate
{
    public class TemplateExercise : Entity
    {
        public int ExerciseId { get; set; }

        public int Order { get; set; }

        public string? Comments { get; set; }

        public Exercise? Exercise { get; set; } = new Exercise();

        public List<TemplateExerciseSettings>? Settings { get; set; } = new List<TemplateExerciseSettings>();

        public TemplateExerciseSettings SettingsTemplate { get; set; } = new TemplateExerciseSettings();
    }
}

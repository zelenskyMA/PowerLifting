using SportAssistant.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.TrainingTemplate
{
    [Table("TemplateExercises", Schema = "plan")]
    public class TemplateExerciseDb : EntityDb
    {
        public int TemplateDayId { get; set; }

        public int ExerciseId { get; set; }

        public int Order { get; set; }

        public string? Comments { get; set; }
    }
}
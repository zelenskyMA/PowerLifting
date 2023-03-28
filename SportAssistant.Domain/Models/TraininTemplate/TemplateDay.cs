using SportAssistant.Domain.Models.Common;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Domain.Models.TraininTemplate;

namespace SportAssistant.Domain.Models.TrainingTemplate;

public class TemplateDay : Entity
{
    public int TemplatePlanId { get; set; }

    public int DayNumber { get; set; }

    /// <summary>
    /// Упражнения, назначенные на тренировочный день.
    /// </summary>
    public List<TemplateExercise>? Exercises { get; set; } = new List<TemplateExercise>();

    /// <summary>
    /// Процентовки, которые задействованы в плане на день
    /// </summary>
    public List<Percentage>? Percentages { get; set; }

    public CountersTemplateDay Counters { get; set; } = new CountersTemplateDay();
}

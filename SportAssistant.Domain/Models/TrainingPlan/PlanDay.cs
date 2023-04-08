using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.Models.TrainingPlan;

public class PlanDay : Entity
{
    public int? PlanId { get; set; }

    /// <summary>
    /// Дата тренировочного дня.
    /// </summary>
    public DateTime ActivityDate { get; set; }

    /// <summary>
    /// Упражнения, назначенные на тренировочный день.
    /// </summary>
    public List<PlanExercise>? Exercises { get; set; } = new List<PlanExercise>();

    /// <summary>
    /// Процентовки, которые задействованы в плане на день
    /// </summary>
    public List<Percentage>? Percentages { get; set; }

    public CountersPlanDay Counters { get; set; } = new CountersPlanDay();

    public OwnerOfPlan Owner { get; set; } = new OwnerOfPlan();
}

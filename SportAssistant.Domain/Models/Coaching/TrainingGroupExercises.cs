using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Domain.Models.Coaching;

public class TrainingGroupWorkout
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string GroupName { get; set; } = string.Empty;

    public PlanDay PlanDay { get; set; }
}

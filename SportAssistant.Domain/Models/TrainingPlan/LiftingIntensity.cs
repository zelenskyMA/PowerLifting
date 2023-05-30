namespace SportAssistant.Domain.Models.TrainingPlan;

/// <summary>
/// КПШ по зонам интенсивности
/// </summary>
public class LiftIntensity
{
    public Percentage? Percentage { get; set; }

    public int Value { get; set; } = 0;
}

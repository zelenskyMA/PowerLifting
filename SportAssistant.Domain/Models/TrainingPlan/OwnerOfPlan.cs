namespace SportAssistant.Domain.Models.TrainingPlan;

public class OwnerOfPlan
{
    /// <summary>
    /// Имя владельца плана. Нужно тренерам при планировании в группе
    /// </summary>
    public string Name { get; set; } = string.Empty;

}

namespace SportAssistant.Domain.Models.Basic;

public class AppSettings
{
    /// <summary>
    /// Максимум активных планов
    /// </summary>
    public int MaxActivePlans { get; set; }

    /// <summary>
    /// Максимум упражнений в день
    /// </summary>
    public int MaxExercises { get; set; }

    /// <summary>
    /// Максимум поднятий в упражнении
    /// </summary>
    public int MaxLiftItems { get; set; }
}

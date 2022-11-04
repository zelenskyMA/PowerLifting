using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.Models.Coaching;

public class TrainingGroupExercises
{
    public string Name { get; set; } = string.Empty;

    public int ParticipantsCount { get; set; }

    /// <summary>
    /// Количество упражнений по подтипам.
    /// </summary>
    public List<ValueEntity> ExerciseTypeCounters { get; set; } = new List<ValueEntity>();
}

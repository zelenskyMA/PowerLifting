namespace SportAssistant.Domain.Models.ReportGeneration;

public class ReportExercise
{
    public int OrderNumber { get; set; }

    public string Name { get; set; }

    public List<ReportExerciseSettings> ExerciseSettings { get; set; } = new List<ReportExerciseSettings>();
}

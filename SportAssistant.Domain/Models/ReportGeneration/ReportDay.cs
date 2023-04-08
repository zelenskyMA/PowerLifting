namespace SportAssistant.Domain.Models.ReportGeneration;

public class ReportDay
{
    public string DayDate { get; set; } = string.Empty;

    public List<ReportExercise> Exercises { get; set; } = new List<ReportExercise>();
}
